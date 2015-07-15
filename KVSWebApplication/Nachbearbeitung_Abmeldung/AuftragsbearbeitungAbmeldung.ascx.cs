﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KVSCommon.Database;
using Telerik.Web.UI;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Configuration;
using System.Transactions;
namespace KVSWebApplication.Nachbearbeitung_Abmeldung
{
    /// <summary>
    /// Codebehind fuer die Abmeldung Maske
    /// </summary>
    public partial class AuftragsbearbeitungAbmeldung : System.Web.UI.UserControl
    {
        private string customer = string.Empty;        
        RadScriptManager script = null;
        protected void RadGridAbmeldung_PreRender(object sender, EventArgs e)
        {
            HideExpandColumnRecursive(RadGridAbmeldung.MasterTableView);
        
        }
        public void HideExpandColumnRecursive(GridTableView tableView)
        {
            thisUserPermissions.AddRange(KVSCommon.Database.User.GetAllPermissionsByID(((Guid)Session["CurrentUserId"])));
            GridItem[] nestedViewItems = tableView.GetItems(GridItemType.NestedView);
            foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
            {
                foreach (GridTableView nestedView in nestedViewItem.NestedTableViews)
                {
                    nestedView.ParentItem.Expanded = true;
                    HideExpandColumnRecursive(nestedView);
                }
            }
        }
        List<string> thisUserPermissions = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            thisUserPermissions.AddRange(KVSCommon.Database.User.GetAllPermissionsByID(((Guid)Session["CurrentUserId"])));
            bool canDeleteItem = thisUserPermissions.Contains("LOESCHEN_AUFTRAGSPOSITION");
            if (canDeleteItem == false)
            {
                foreach (var table in RadGridAbmeldung.MasterTableView.DetailTables)
                {
                    if (table.GetColumn("RemoveOrderItem") != null)
                        table.GetColumn("RemoveOrderItem").Visible = false;
                }
            }
            NachbearbeitungAbmeldung auftragNeu = Page as NachbearbeitungAbmeldung;
            script = auftragNeu.getScriptManager() as RadScriptManager;
            script.RegisterPostBackControl(ZulassungsstelleLieferscheineButton);      
            string target = Request["__EVENTTARGET"] == null ? "" : Request["__EVENTTARGET"];
            if (String.IsNullOrEmpty(target))
                if (Session["orderNumberSearch"] != null)
                    if(!String.IsNullOrEmpty(Session["orderNumberSearch"].ToString()))
                       target = "IamFromSearch";
            StornierungErfolgLabel.Visible = false;
            if (Session["CustomerIndex"] != null)
            {                  
                if (!target.Contains("RadComboBoxCustomerAbmeldungOffen") && !target.Contains("CustomerDropDownListAbmeldungOffen") && !target.Contains("StornierenButton") && !target.Contains("NewPositionButton"))
                {                 
                    if (Session["CustomerId"] != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            if (CustomerDropDownListAbmeldungOffen.Items.Count > 0 && RadComboBoxCustomerAbmeldungOffen.Items.Count() > 0)
                            {
                                RadComboBoxCustomerAbmeldungOffen.SelectedValue = Session["CustomerIndex"].ToString();
                                CustomerDropDownListAbmeldungOffen.SelectedValue = Session["CustomerId"].ToString();
                            }
                         }
                        if (Session["orderStatusSearch"] != null)
                            if (!Session["orderStatusSearch"].ToString().Contains("Zulassungsstelle"))

                                if (target.Contains("ZulassungNachbearbeitung") || target.Contains("RadTabStrip1") || target.Contains("IamFromSearch"))                              
                                {
                                    RadGridAbmeldung.Enabled = true;
                                    RadGridAbmeldung.Rebind();
                                }                      
                    }
                }
            }
        }
        protected void CheckOpenedOrders()
        {
            ordersCount.Text = Order.getUnfineshedOrdersCount(new DataClasses1DataContext(), "Abmeldung", 100).ToString();
            if (ordersCount.Text == "" || ordersCount.Text == "0")
            {
                go.Visible = false;
            }
            else
            {
                go.Visible = true;
            }
        }
        protected void AbmeldungenLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {         
            //select all values for small customers
            if ( RadComboBoxCustomerAbmeldungOffen.SelectedValue == "1")
            {
                DataClasses1DataContext con = new DataClasses1DataContext();
                var abmeldungQuery = from ord in con.Order
                                     join ordst in con.OrderStatus on ord.Status equals ordst.Id
                                     join cust in con.Customer on ord.CustomerId equals cust.Id
                                     join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                     join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                     join reg in con.Registration on derord.RegistrationId equals reg.Id
                                     join veh in con.Vehicle on derord.VehicleId equals veh.Id
                                     join smc in con.SmallCustomer on cust.Id equals smc.CustomerId
                                     orderby ord.Ordernumber descending
                                     where ord.Status == 100 && ordtype.Name == "Abmeldung" && ord.HasError.GetValueOrDefault(false) != true
                                     select new
                                     {
                                         OrderId = ord.Id,
                                         OrderNumber = ord.Ordernumber,
                                         customerID = ord.CustomerId,
                                         CreateDate = ord.CreateDate,
                                         Status = ordst.Name,
                                         CustomerLocation = "",
                                         CustomerName = cust.SmallCustomer.Person != null ? cust.SmallCustomer.Person.FirstName + "  " + cust.SmallCustomer.Person.Name : cust.Name,
                                         Kennzeichen = reg.Licencenumber,
                                         VIN = veh.VIN,
                                         TSN = veh.TSN,
                                         HSN = veh.HSN,
                                         OrderTyp = ordtype.Name,
                                         Freitext = ord.FreeText,
                                         Geprueft = ord.Geprueft == null ? "Nein" : "Ja",
                                         Datum = ord.DeregistrationOrder.Registration.RegistrationDate
                                     };
                if (CustomerDropDownListAbmeldungOffen.SelectedValue != string.Empty)
                {
                    Guid guid = new Guid(CustomerDropDownListAbmeldungOffen.SelectedValue);
                    abmeldungQuery = abmeldungQuery.Where(q => q.customerID == guid);
                }
                e.Result = abmeldungQuery;
            }
            //select all values for large customers
            else if (RadComboBoxCustomerAbmeldungOffen.SelectedValue == "2")
            {
                DataClasses1DataContext con = new DataClasses1DataContext();
                var abmeldungQuery = from ord in con.Order
                                     join ordst in con.OrderStatus on ord.Status equals ordst.Id
                                     join cust in con.Customer on ord.CustomerId equals cust.Id
                                     join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                     join loc in con.Location on ord.LocationId equals loc.Id
                                     join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                     join reg in con.Registration on derord.RegistrationId equals reg.Id
                                     join veh in con.Vehicle on derord.VehicleId equals veh.Id
                                     join lmc in con.LargeCustomer on cust.Id equals lmc.CustomerId
                                     orderby ord.Ordernumber descending
                                     where ord.Status == 100 && ordtype.Name == "Abmeldung" && ord.HasError.GetValueOrDefault(false) != true
                                     select new
                                     {
                                         OrderId = ord.Id,
                                         locationId = loc.Id,
                                         OrderNumber = ord.Ordernumber,
                                         customerID = ord.CustomerId,
                                         CreateDate = ord.CreateDate,
                                         Status = ordst.Name,
                                         CustomerName = cust.Name,
                                         Kennzeichen = reg.Licencenumber,
                                         VIN = veh.VIN,
                                         TSN = veh.TSN,
                                         HSN = veh.HSN,
                                         CustomerLocation = loc.Name,
                                         OrderTyp = ordtype.Name,
                                         Freitext = ord.FreeText,
                                         Geprueft = ord.Geprueft == null ? "Nein" : "Ja",
                                         Datum = ord.DeregistrationOrder.Registration.RegistrationDate
                                     };
                if (CustomerDropDownListAbmeldungOffen.SelectedValue != string.Empty)
                {
                    Guid guid = new Guid(CustomerDropDownListAbmeldungOffen.SelectedValue);
                    abmeldungQuery = abmeldungQuery.Where(q => q.customerID == guid);
                }
                if (Session["orderNumberSearch"] != null && Session["orderStatusSearch"] != null)
                {
                    if (!String.IsNullOrEmpty(Session["orderNumberSearch"].ToString()))
                    {
                        if (Session["orderStatusSearch"].ToString().Contains("Offen"))
                        {
                            int orderNumber = Convert.ToInt32(Session["orderNumberSearch"].ToString());
                            abmeldungQuery = abmeldungQuery.Where(q => q.OrderNumber == orderNumber);
                        }
                    }
                }
                e.Result = abmeldungQuery;
            }
            CheckOpenedOrders();
        }
        protected void ShowAllButton_Click(object sender, EventArgs e)
        {
            Session["customerIndexSearch"] = null;
            Session["orderStatusSearch"] = null;
            Session["customerIdSearch"] = null;
            Session["orderNumberSearch"] = null;

            CustomerDropDownListAbmeldungOffen.ClearSelection();
            RadGridAbmeldung.Enabled = true;
            RadGridAbmeldung.Rebind();
        }
        // Small oder Large -> Auswahl der KundenName
        protected void CustomerLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            if (RadComboBoxCustomerAbmeldungOffen.SelectedValue == "1") //Small Customers
            {
                var customerQuery = from cust in con.Customer
                                    where cust.Id == cust.SmallCustomer.CustomerId
                                    select new
                                    {
                                        Name = cust.SmallCustomer.Person != null ? cust.SmallCustomer.Person.FirstName + " " + cust.SmallCustomer.Person.Name : cust.Name,
                                        Value = cust.Id, Matchcode = cust.MatchCode, Kundennummer = cust.CustomerNumber };
                e.Result = customerQuery;
            }
            else if (RadComboBoxCustomerAbmeldungOffen.SelectedValue == "2") //Large Customers
            {
                var customerQuery = from cust in con.Customer
                                    where cust.Id == cust.LargeCustomer.CustomerId
                                    select new { Name = cust.Name, Value = cust.Id, Matchcode = cust.MatchCode, Kundennummer = cust.CustomerNumber };
                e.Result = customerQuery;
            }           
        }
        // Large oder small Customer
        protected void SmallLargeCustomerIndex_Changed(object sender, EventArgs e)
        {
            CustomerDropDownListAbmeldungOffen.Enabled = true;
            this.CustomerDropDownListAbmeldungOffen.DataBind();
            this.RadGridAbmeldung.DataBind();
            Session["CustomerIndex"] = RadComboBoxCustomerAbmeldungOffen.SelectedValue;
        }
        // Auswahl von Kunde
        protected void CustomerIndex_Changed(object sender, EventArgs e)
        {
            RadGridAbmeldung.Enabled = true;
            this.RadGridAbmeldung.DataBind();
            Session["CustomerIndex"] = RadComboBoxCustomerAbmeldungOffen.SelectedValue;
            Session["CustomerId"] = CustomerDropDownListAbmeldungOffen.SelectedValue;     
        }
        protected void ProductLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var productQuery = from prod in con.Product
                               select new {
                                   ItemNumber = prod.ItemNumber,
                                   Name = prod.Name,
                                   Value = prod.Id,
                                   Category = prod.ProductCategory.Name
                               };
            e.Result = productQuery;           
        }
        protected void CostCenterDataSourceLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var costCenterQuery = from cost in con.CostCenter
                                  select new { 
                                  Name = cost.Name,
                                  Value = cost.Id
                                  };
            e.Result = costCenterQuery;
        }
        // Automatische Suche nach HSN
        protected void HSNBox_TextChanged(object sender, EventArgs e)
        {
            TextBox hsnTextBox = sender as TextBox;
            Label hsnLabel = hsnTextBox.Parent.FindControl("HSNSearchLabel") as Label;
            TextBox tsnBox = hsnTextBox.Parent.FindControl("TSNAbmFormBox") as TextBox;
            hsnLabel.Text = "";
            if (!String.IsNullOrEmpty(hsnTextBox.Text))
            {
                hsnLabel.Visible = true;
                hsnLabel.Text = Make.GetMakeByHSN(hsnTextBox.Text);
            }
            tsnBox.Focus();
        }
        protected void OnItemCommand_Fired(object sender, GridCommandEventArgs e)
        {
            try
            {
                AbmeldungErrLabel.Text = "";
                AbmeldungErrLabel.Visible = false;
                if (e.CommandName == "AmtGebuhrSetzen")
                {
                    GridEditableItem editedItem = e.Item as GridEditableItem;
                    RadTextBox tbEditPrice = editedItem["ColumnPrice"].FindControl("tbEditPrice") as RadTextBox;
                    string itemId = editedItem["ItemIdColumn"].Text;
                    RadTextBox tbAuthPrice = editedItem["AuthCharge"].FindControl("tbAuthChargePrice") as RadTextBox;
                    Guid authId = new Guid(editedItem["AuthChargeId"].Text);
                    using (DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString())))
                    {
                        if (Order.GenerateAuthCharge(dbContext, authId, itemId, tbAuthPrice.Text))
                        {
                            dbContext.SubmitChanges();
                            tbAuthPrice.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    UpdatePosition(itemId, tbEditPrice.Text);
                    tbEditPrice.ForeColor = System.Drawing.Color.Green;
                }
                else if (e.CommandName == "RemoveOrderItem")
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                        
                            try
                            {

                                GridEditableItem editedItem = e.Item as GridEditableItem;
                                string itemId = editedItem["ItemIdColumn"].Text;
                                OrderItem.RemoveOrderItem(dbContext, new Guid(itemId));
                                dbContext.SubmitChanges();
                                ts.Complete();
                              

                            }
                            catch (Exception ex)
                            {
                                if (ts != null)
                                    ts.Dispose();

                                AbmeldungErrLabel.Text = "Fehler: " + ex.Message;
                                AbmeldungErrLabel.Visible = true;
                                dbContext = new DataClasses1DataContext(((Guid)Session["CurrentUserId"]));
                                dbContext.WriteLogItem("Delete OrderItem Error " + ex.Message, LogTypes.ERROR, "OrderItem");
                                dbContext.SubmitChanges();
                            }
                        

                    }
                    RadGridAbmeldung.Rebind();

                }
                else
                {
                    if (e.Item is GridDataItem)
                    {
                        var button = sender as RadButton;
                        GridDataItem dataItem = e.Item as GridDataItem;
                        dataItem.Selected = true;
                        itemIndexHiddenField.Value = dataItem.ItemIndex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                AbmeldungErrLabel.Text = "Fehler: " + ex.Message;
                AbmeldungErrLabel.Visible = true;

            }
        }
 
        // Updating ausgewählten OrderItem
        protected void UpdatePosition(string itemId, string amount)
        {
            Guid orderItemId = Guid.Empty;
            string amoutToSave = amount;
            if (amoutToSave.Contains("."))
                amoutToSave = amoutToSave.Replace(".", ",");
            if (!EmptyStringIfNull.IsNumber(amount))
                throw new Exception("Achtung, Sie haben keinen gültigen Preis eingegeben");
            orderItemId = new Guid(itemId);
            if (orderItemId != Guid.Empty)
            {
                try
                {
                    DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                    var positionUpdateQuery = dbContext.OrderItem.SingleOrDefault(q => q.Id == orderItemId);
                    positionUpdateQuery.LogDBContext = dbContext;
                    positionUpdateQuery.Amount = Convert.ToDecimal(amoutToSave);
                    dbContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Die ausgewählte Position kann nicht updatet werden <br /> Error: " + ex.Message);
                }
            }
        }
        // Neue freie Position wird hinzugefügt
        protected void NewPositionButton_Clicked(object sender, EventArgs e)
        {
            if (RadGridAbmeldung.SelectedItems.Count > 0)
            {
                  try
                {
                AbmeldungErrLabel.Visible = false;
                DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                Button button = sender as Button;
                OrderItem newOrderItem1 = null;
                Guid orderId = Guid.Empty, locationId = Guid.Empty;
                Price newPrice = null;
                RadComboBox productDropDown = button.NamingContainer.FindControl("NewProductDropDownList") as RadComboBox;
                DropDownList costCenterDropDown = button.NamingContainer.FindControl("CostCenterDropDownList") as DropDownList;              
                Guid product = new Guid(productDropDown.SelectedValue);          
                foreach (GridDataItem item in RadGridAbmeldung.SelectedItems)
                {
                    //hier
                    KVSCommon.Database.Product newProduct = dbContext.Product.SingleOrDefault(q => q.Id == new Guid(productDropDown.SelectedValue));

                    orderId = new Guid(item["OrderId"].Text);
                    if (!String.IsNullOrEmpty(item["locationId"].Text) && EmptyStringIfNull.IsGuid(item["locationId"].Text))
                    {
                        locationId = new Guid(item["locationId"].Text);
                        newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == locationId);
                    }
                     
                        if (newPrice == null)
                        {
                            newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == null);
                        }
                   
                        var orderToUpdate = dbContext.Order.SingleOrDefault(q => q.Id == orderId);
                        orderToUpdate.LogDBContext = dbContext;
                        if (newPrice == null || newProduct == null || orderToUpdate == null)
                            throw new Exception("Achtung, die Position kann nicht hinzugefügt werden, es konnte entweder kein Preis, Produkt oder der Auftrag gefunden werden!");
                        if (orderToUpdate != null)
                        {
                            var orderItemCostCenter = orderToUpdate.OrderItem.FirstOrDefault(q => q.CostCenter != null);


                            newOrderItem1 = orderToUpdate.AddOrderItem(newProduct.Id, newPrice.Amount, 1,
                                 (orderItemCostCenter != null) ? orderItemCostCenter.CostCenterId : null, 
                                 null, false, dbContext);
                            if (newPrice.AuthorativeCharge.HasValue)
                            {
                                orderToUpdate.AddOrderItem(newProduct.Id, newPrice.AuthorativeCharge.Value, 1, (orderItemCostCenter != null) ? orderItemCostCenter.CostCenterId : null,
                                    newOrderItem1.Id, newPrice.AuthorativeCharge.HasValue, dbContext);
                            }
                            dbContext.SubmitChanges();
                        }
                  
                }
                RadGridAbmeldung.Rebind();
                }
                  catch (Exception ex)
                  {
                      AbmeldungErrLabel.Visible = true;
                      AbmeldungErrLabel.Text = "Fehler: " + ex.Message;
                  }
            }
            else 
            {
                AbmeldungErrLabel.Text = "Sie haben keinen Auftrag ausgewählt!";
                AbmeldungErrLabel.Visible = true;
            }                  
        }
        protected void RadGridOffen_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            DataClasses1DataContext dbContext = new DataClasses1DataContext();
            GridDataItem item = (GridDataItem)e.DetailTableView.ParentItem;
            Guid orderId = new Guid(item["OrderId"].Text.ToString());
            var positionQuery = from ord in dbContext.Order
                                join orditem in dbContext.OrderItem on ord.Id equals orditem.OrderId
                                let authCharge = dbContext.OrderItem.FirstOrDefault(s => s.SuperOrderItemId == orditem.Id)
                                where ord.Id == orderId && (orditem.SuperOrderItemId == null)
                                select new
                                {
                                    OrderItemId = orditem.Id,
                                    Amount = orditem.Amount == null ? "kein Preis" : (Math.Round(orditem.Amount, 2, MidpointRounding.AwayFromZero)).ToString(),
                                    ProductName = orditem.IsAuthorativeCharge ? orditem.ProductName + " (Amtl.Gebühr)" : orditem.ProductName,
                                    AmtGebuhr = authCharge == null ? false:true,
                                    AuthCharge = authCharge == null || authCharge.Amount == null ? "kein Preis" : (Math.Round(authCharge.Amount, 2, MidpointRounding.AwayFromZero)).ToString(),
                                    AuthChargeId = authCharge == null ? Guid.Empty : authCharge.Id
                                    //AmtGebuhr2 = orditem.IsAuthorativeCharge ? "Ja" : "Nein"
                                };
            e.DetailTableView.DataSource = positionQuery;
        }       
        //Row Index wird in hiddenfield gespeichert
        protected void EditButton_Clicked(object sender, GridCommandEventArgs  e)
        {
            var button = sender as RadButton;
            GridDataItem dataItem = e.Item as GridDataItem;
            dataItem.Selected = true;
            itemIndexHiddenField.Value = dataItem.ItemIndex.ToString();
        }
        protected void ZulassungsstelleLieferscheineButton_Clicked(object sender, EventArgs e)
        {

            TransactionScope ts = null;

            try
            {
                if (String.IsNullOrEmpty(ZulassungsDatumPicker.SelectedDate.ToString()))
                {
                    LieferscheinePath.Text = "Wählen Sie bitte das Zulassungsdatum aus!";
                    LieferscheinePath.Visible = true;
                }
                else
                {

                

                    List<string> laufzettel = new List<string>();
                    using (DataClasses1DataContext con = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString())))
                    {
                        using (ts = new TransactionScope())
                        {

                            var zulQuery2 = from ord in con.Order
                                            join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                            join regLoc in con.RegistrationLocation on ord.Zulassungsstelle equals regLoc.ID
                                            join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                            where ord.Status == 100 && ordtype.Name == "Abmeldung" && ord.HasError.GetValueOrDefault(false) != true
                                            && ord.DeregistrationOrder.Registration.RegistrationDate <= ZulassungsDatumPicker.SelectedDate
                                            select ord;
                            var grouptedOrders = zulQuery2.GroupBy(q => q.Zulassungsstelle.Value);
                            foreach (var location in grouptedOrders)
                            {
                                DocketList docketList = new DocketList();
                                MemoryStream ms = new MemoryStream();
                                if (location.Count() > 0)
                                {

                                    docketList = DocketList.CreateDocketList(location.First().RegistrationLocation.RegistrationLocationName, location.First().RegistrationLocation.Adress.Id, con);
                                    docketList.LogDBContext = con;
                                    docketList.IsSelfDispatch = true;
                                }
                                foreach (var order in location)
                                {
                                    Guid orderId = Guid.Empty;
                                    foreach (var order2 in location)
                                    {
                                        if (order2 != null)
                                        {
                                            orderId = order2.Id;
                                            docketList.AddOrderById(orderId, con);
                                            //updating order status
                                            order2.LogDBContext = con;
                                            order2.Status = 400;
                                            //updating orderitems status                          
                                            foreach (OrderItem ordItem in order2.OrderItem)
                                            {
                                                ordItem.LogDBContext = con;
                                                if (ordItem.Status != (int)OrderItemState.Storniert)
                                                {
                                                    ordItem.Status = 300;
                                                }
                                            }
                                        }
                                    }
                                    con.SubmitChanges();
                                }
                                string myPackListFileName = EmptyStringIfNull.CheckIfFolderExistsAndReturnPathForPdf(Session["CurrentUserId"].ToString(), true);
                                docketList.Print(ms, string.Empty, con, "/UserData/" + Session["CurrentUserId"].ToString() + "/" + Path.GetFileName(myPackListFileName), true);
                                con.SubmitChanges();
                                string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                                string host = ConfigurationManager.AppSettings["smtpHost"];
                                PdfDocument d = PdfReader.Open(new MemoryStream(ms.ToArray(), 0, Convert.ToInt32(ms.Length)));
                                d.Save(myPackListFileName);
                                docketList.SendByEmail(ms, fromEmail, host);
                                docketList = null;
                                d = null;
                                ms.Close();
                                ms = null;
                                laufzettel.Add(myPackListFileName);
                            }
                            ts.Complete();
                        }
                            RadGridAbmeldung.MasterTableView.ClearChildEditItems();
                            RadGridAbmeldung.MasterTableView.ClearEditItems();
                            RadGridAbmeldung.Rebind();
                            if (laufzettel.Count > 1)
                            {
                                string myMergedFileName = EmptyStringIfNull.CheckIfFolderExistsAndReturnPathForPdf(Session["CurrentUserId"].ToString(), true);
                                DocketList.MergeDocketLists(laufzettel.ToArray(), myMergedFileName);
                                myMergedFileName = myMergedFileName.Replace(ConfigurationManager.AppSettings["BasePath"], ConfigurationManager.AppSettings["BaseUrl"]);
                                myMergedFileName = myMergedFileName.Replace(@"\\", @"/");
                                myMergedFileName = myMergedFileName.Replace(@"\", @"/");
                                LieferscheinePath.Text = "<a href=" + '\u0022' + myMergedFileName + '\u0022' + " target=" + '\u0022' + "_blank" + '\u0022' + "> Laufzettel öffnen</a>";
                                LieferscheinePath.Visible = true;
                            }
                            else if (laufzettel.Count == 1)
                            {
                                string myMergedFileName = laufzettel[0];
                                myMergedFileName = myMergedFileName.Replace(ConfigurationManager.AppSettings["BasePath"], ConfigurationManager.AppSettings["BaseUrl"]);
                                myMergedFileName = myMergedFileName.Replace(@"\\", @"/");
                                myMergedFileName = myMergedFileName.Replace(@"\", @"/");
                                LieferscheinePath.Text = "<a href=" + '\u0022' + myMergedFileName + '\u0022' + " target=" + '\u0022' + "_blank" + '\u0022' + "> Laufzettel öffnen</a>";
                                LieferscheinePath.Visible = true;
                            }
                            else
                            {
                                LieferscheinePath.Text = "Keine Laufzettel vorhanden!";
                                LieferscheinePath.Visible = true;
                            }
                          

                    }
                }
                CheckOpenedOrders();
            }
            catch (Exception ex)
            {
                if (ts != null)
                    ts.Dispose();

                AbmeldungErrLabel.Visible = true;
                AbmeldungErrLabel.Text = "Fehler: " + ex.Message;
              
            }
        }
        private void UpdateOrderAfterZulassungsstelle(Guid customerIdToUpdate, Guid orderIdToUpdate)
        {
            Guid customerID = customerIdToUpdate;
            Guid orderId = orderIdToUpdate;
            try
            {
                DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                var newOrder = dbContext.Order.Single(q => q.CustomerId == customerID && q.Id == orderId);
                if (newOrder != null)
                {
                    //updating order status
                    newOrder.LogDBContext = dbContext;
                    newOrder.Status = 400;
                    //updating orderitems status                          
                    foreach (OrderItem ordItem in newOrder.OrderItem)
                    {
                        ordItem.LogDBContext = dbContext;
                        if (ordItem.Status != (int)OrderItemState.Storniert)
                        {
                            ordItem.Status = 300;
                        }
                    }
                    dbContext.SubmitChanges();
                }
            }
            catch(Exception ex)
            {
                AbmeldungErrLabel.Text = "Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator. <br />" + "Error: " + ex.Message;
                AbmeldungErrLabel.Visible = true;
            }
        }
        protected void AbmeldungZulassen_Command(object sender, EventArgs e)
        {
            if (itemIndexHiddenField.Value != null) // falls ausgewählte Row Index gesetz wurde
            {
                GridDataItem selectedItem = RadGridAbmeldung.MasterTableView.Items[Convert.ToInt32(itemIndexHiddenField.Value)];
                selectedItem.Selected = true;            
                string VIN = string.Empty,
                    kennzeichen = string.Empty,
                    HSN = string.Empty,
                    TSN = string.Empty;
                Guid orderId, customerId;
                Button editButton = sender as Button;
                GridEditFormItem item = editButton.NamingContainer as GridEditFormItem;
                TextBox vinBox = item.FindControl("VINBox") as TextBox;
                TextBox orderIdBox = item.FindControl("orderIdBox") as TextBox;
                TextBox kennzeichenBox = item.FindControl("KennzeichenBox") as TextBox;
                CheckBox errorCheckBox = item.FindControl("ErrorCheckBox") as CheckBox;
                TextBox errorReasonTextBox = item.FindControl("ErrorReasonTextBox") as TextBox;
                TextBox HSNBox = item.FindControl("HSNAbmFormBox") as TextBox;
                TextBox TSNBox = item.FindControl("TSNAbmFormBox") as TextBox;
                orderId = new Guid(orderIdBox.Text);
                if (!String.IsNullOrEmpty(CustomerDropDownListAbmeldungOffen.SelectedValue.ToString()))
                    customerId = new Guid(CustomerDropDownListAbmeldungOffen.SelectedValue);
                else
                {
                    TextBox customerid = item.FindControl("customerIdBox") as TextBox;
                    customerId = new Guid(customerid.Text);
                }
                AbmeldungErrLabel.Visible = false;
                if (errorCheckBox.Checked) // falls Auftrag als Fehler gemeldet sollte
                {
                    string errorReason = errorReasonTextBox.Text;
                    try
                    {
                        DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                        var OrderToUpdate = dbContext.Order.SingleOrDefault(q => q.Id == orderId && q.CustomerId == customerId);
                        OrderToUpdate.LogDBContext = dbContext;
                        OrderToUpdate.HasError = true;
                        OrderToUpdate.ErrorReason = errorReason;
                        dbContext.SubmitChanges();
                    }
                    catch(Exception ex)
                    {
                        AbmeldungErrLabel.Text = "Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator. <br /> " + "Error: " + ex.Message;
                        AbmeldungErrLabel.Visible = true;                   
                    }
                }
                else // falls normales Update 
                {
                    VIN = vinBox.Text;
                    TSN = TSNBox.Text;
                    HSN = HSNBox.Text;
                    kennzeichen = kennzeichenBox.Text;
                    try
                    {
                        updateDataBase(VIN, TSN, HSN, orderId, customerId, kennzeichen);
                    }
                    catch (Exception ex)
                    {
                        AbmeldungErrLabel.Text = "Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator. <br /> " + "Error: " + ex.Message;
                        AbmeldungErrLabel.Visible = true;
                    }                
               }
                if(Session["orderNumberSearch"] != null)
                  Session["orderNumberSearch"] = string.Empty; //after search should be empty
                RadGridAbmeldung.MasterTableView.ClearChildEditItems();
                RadGridAbmeldung.MasterTableView.ClearEditItems();
                RadGridAbmeldung.Rebind();
            }         
        }
        // Order wird updated mit Status 400 (Zulassungstelle)
        protected void UpdateOrderAndItemsStatus()
        {
            if (RadGridAbmeldung.SelectedItems.Count > 0)
            {
                AbmeldungErrLabel.Visible = false;
                AbmeldungOkLabel.Visible = false;
                //if (CheckIfAllExistsToUpdate())
                //{
                    foreach (GridDataItem item in RadGridAbmeldung.SelectedItems)
                    {
                        // Vorbereitung für Update
                        Guid customerID;
                        if (!String.IsNullOrEmpty(CustomerDropDownListAbmeldungOffen.SelectedValue.ToString()))
                            customerID = new Guid(CustomerDropDownListAbmeldungOffen.SelectedValue);
                        else
                            customerID = new Guid(item["customerID"].Text);
                        Guid orderId = new Guid(item["OrderId"].Text);
                        try
                        {
                            DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                            var newOrder = dbContext.Order.Single(q => q.CustomerId == customerID && q.Id == orderId);
                            if (newOrder != null)
                            {
                                //updating order status
                                newOrder.LogDBContext = dbContext;
                                newOrder.Status = 400;

                                //updating orderitems status                          
                                foreach (OrderItem ordItem in newOrder.OrderItem)
                                {
                                    ordItem.LogDBContext = dbContext;
                                    if (ordItem.Status != (int)OrderItemState.Storniert)
                                    {
                                        ordItem.Status = 300;
                                    }
                                }
                                dbContext.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            AbmeldungErrLabel.Text = "Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator. <br /> " + "Error: " + ex.Message;
                            AbmeldungErrLabel.Visible = true;
                        }
                    }
                    // erfolgreich
                    RadGridAbmeldung.DataBind();
                    AbmeldungOkLabel.Visible = true;
            }
            else
            {
                AbmeldungErrLabel.Text = "Sie haben keinen Auftrag ausgewählt!";
                AbmeldungErrLabel.Visible = true;
            }
        }
        // Updating Order before Zulassungstelle
        protected void updateDataBase(string vin, string tsn, string hsn, Guid orderId, Guid customerId, string kennzeichen)
        {
            using (DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString())))
            {
                var updateQuery = dbContext.DeregistrationOrder.Single(q => q.OrderId == orderId);
                updateQuery.LogDBContext = dbContext;
                updateQuery.Vehicle.LogDBContext = dbContext;
                updateQuery.Registration.LogDBContext = dbContext;
                updateQuery.Order.LogDBContext = dbContext;
                updateQuery.Vehicle.VIN = vin;
                updateQuery.Registration.Licencenumber = kennzeichen;
                updateQuery.Vehicle.TSN = tsn;
                updateQuery.Vehicle.HSN = hsn;               
                dbContext.SubmitChanges();               
            }
        }
        //Prüfen ob alle Werte da sind um den Auftrag auf "Zulassungstelle" zu setzen
        private bool CheckIfAllExistsToUpdate()
        {
            bool shouldBeUpdated = true;
            foreach (GridDataItem item in RadGridAbmeldung.SelectedItems)
            { 
                if(String.IsNullOrEmpty(item["VIN"].Text))
                {
                    shouldBeUpdated = false;
                    AbmeldungErrLabel.Text = "Bitte fügen Sie FIN ein";
                    AbmeldungErrLabel.Visible = true;
                }                
                if(String.IsNullOrEmpty(item["TSN"].Text))
                {
                    shouldBeUpdated = false;
                    AbmeldungErrLabel.Text = "Bitte fügen Sie TSN ein";
                    AbmeldungErrLabel.Visible = true;
                }
                if(String.IsNullOrEmpty(item["HSN"].Text))
                {
                    shouldBeUpdated = false;
                    AbmeldungErrLabel.Text = "Bitte fügen Sie HSN ein";
                    AbmeldungErrLabel.Visible = true;
                }
                if (String.IsNullOrEmpty(item["CustomerLocation"].Text))
                {
                    shouldBeUpdated = false;
                    AbmeldungErrLabel.Text = "Bitte fügen Sie Standort ein";
                    AbmeldungErrLabel.Visible = true;
                }                
            }
            return shouldBeUpdated;
        }
        // Ändert das Text von Button entweder nach Fehler oder Zulassung
        protected void ErrorCheckBox_Clicked(object sender, EventArgs e)
        {
            CheckBox errorCheckBox = sender as CheckBox;
            Button saveButton = errorCheckBox.FindControl("ZulassenButton") as Button;
            if (errorCheckBox.Checked)
                saveButton.Text = "Als Fehler markieren";
            else
                saveButton.Text = "Speichern und zulassen";
        }
        protected void StornierenButton_Clicked(object sender, EventArgs e)
        {
            if (RadGridAbmeldung.SelectedItems.Count > 0)
            {
                AbmeldungErrLabel.Visible = false;
                StornierungErfolgLabel.Visible = false;

                foreach (GridDataItem item in RadGridAbmeldung.SelectedItems)
                {
                    Guid orderId = new Guid(item["OrderId"].Text);
                    try
                    {
                        DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                        var newOrder = dbContext.Order.SingleOrDefault(q => q.Id == orderId);
                        //updating order status
                        newOrder.LogDBContext = dbContext;
                        newOrder.Status = (int)OrderItemState.Storniert;
                        //updating orderitems status                          
                        foreach (OrderItem ordItem in newOrder.OrderItem)
                        {
                            ordItem.LogDBContext = dbContext;
                            ordItem.Status = (int)OrderItemState.Storniert;
                        }
                        dbContext.SubmitChanges();
                        RadGridAbmeldung.Rebind();
                        StornierungErfolgLabel.Visible = true;
                    }
                    catch
                    {
                        AbmeldungErrLabel.Text = "Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator";
                        AbmeldungErrLabel.Visible = true;
                    }
                }
            }
            else
            {
                AbmeldungErrLabel.Visible = true;
            }
        }
    }
}