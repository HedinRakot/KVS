﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KVSCommon.Database;
using Telerik.Web.UI;
using System.Data.SqlClient;

namespace KVSWebApplication.Nachbearbeitung_Abmeldung
{
    /// <summary>
    /// Codebehind fuer den Reiter Lieferschein Abmeldung
    /// </summary>
    public partial class LieferscheinAbmeldung : System.Web.UI.UserControl
    {
        RadScriptManager script = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string target = Request["__EVENTTARGET"];
            NachbearbeitungAbmeldung auftragNeu = Page as NachbearbeitungAbmeldung;
            script = auftragNeu.getScriptManager() as RadScriptManager;
            script.RegisterPostBackControl(Button1);
            if (!String.IsNullOrEmpty(target))
            {
                if (target.Equals("UserValueConfirmLieferscheine") || target.Equals("UserValueDontConfirmLieferscheine"))
                {
                    UserValueConfirm.Value = null;
                    userAuswahl.Value = target;
                    if (!string.IsNullOrEmpty(userAuswahl.Value) && userAuswahl.Value.Equals("UserValueDontConfirmLieferscheine"))
                    {
                        OffenePanel.Visible = true;
                        NochOffenAuftraegeRadGrid.Enabled = true;
                        NochOffenAuftraegeRadGrid.Rebind();
                    }
                    else if (target.Equals("UserValueConfirmLieferscheine"))
                    {
                        LieferscheinErstellen();
                    }
                }
                if (!target.Contains("LieferungButton") && !target.Contains("Button1"))
                {
                }   
            }                    
        }    
     protected void LieferscheineLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
                DataClasses1DataContext con = new DataClasses1DataContext();
                var largeCustomerQuery = from ord in con.Order
                                         join ordst in con.OrderStatus on ord.Status equals ordst.Id
                                         join cust in con.Customer on ord.CustomerId equals cust.Id
                                         join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                         join loc in con.Location on ord.LocationId equals loc.Id
                                         join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                         join reg in con.Registration on derord.RegistrationId equals reg.Id
                                         join veh in con.Vehicle on derord.VehicleId equals veh.Id
                                         where ord.Status == 600 && ordtype.Name == "Abmeldung" && (ord.ReadyToSend == false || ord.ReadyToSend == null)
                                         select new 
                                         {
                                             OrderId = ord.Id,
                                             locationId = loc.Id,
                                             OrderNumber = ord.Ordernumber,
                                             CreateDate = ord.CreateDate,
                                             Status = ordst.Name,
                                             CustomerName = cust.Name,
                                             Kennzeichen = reg.Licencenumber,
                                             VIN = veh.VIN,
                                             TSN = veh.TSN,
                                             HSN = veh.HSN,
                                             CustomerLocation = loc.Name,
                                             CustomerLocationId = loc.Id,
                                             Kundenname = cust.Name,
                                             CustomerId = ord.CustomerId,
                                             Standort = loc.Name,
                                             OrderTyp = ordtype.Name
                                         };              
                e.Result = largeCustomerQuery;      
        }
     protected void LieferItems_Selected(object sender, EventArgs e)
     {
         if (RadGridLieferscheine.SelectedItems.Count > 0)
         {
             BitteTextBox.Visible = false;
             GridDataItem itemToCheck = RadGridLieferscheine.SelectedItems[0] as GridDataItem;
             bool statusFromCheck = CheckForOpenValues(itemToCheck["CustomerLocation"].Text);
             //Falls es gibt noch values - start javascript und raus
             if (statusFromCheck == true)
             {
                 ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "UserResponce", "Closewindow()", true);
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "UserResponce2", "CreatePacking()", true);               
             }
         }
         else
         {
             BitteTextBox.Visible = true;
         }
     }
     protected void OnAddAdressButton_Clicked(object sender, EventArgs e)
     {
         AllesIstOkeyLabelLieferschein.Visible = true;
         LieferscheinErstellen();
         RadGridLieferscheine.Rebind(); 
     }
        /// <summary>
        /// Prüft die Datenbank für Aufträge, die nicht geschlossen sind.
        /// </summary>
        /// <param name="location">Location des Kundes</param>
        /// <returns>Bool (Gefunden oder nicht)</returns>
        protected bool CheckForOpenValues(string location)
        {
            bool statusFromCheck = false;
            if (!String.IsNullOrEmpty(location))
            {                     
                DataClasses1DataContext con = new DataClasses1DataContext();
                var values = (from ord in con.Order
                             join loc in con.Location on ord.LocationId equals loc.Id
                             join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                             where loc.Name == location && ord.Status == 400 && ordtype.Name == "Abmeldung"
                             select ord.LocationId).ToList();

                if (values.Count > 0)
                {
                    statusFromCheck = true;
                    LocationIdHiddenField.Value = location;
                }
            }
            return statusFromCheck;
        }
        //erstellt die Lieferscheine
        protected void LieferscheinErstellen()
        {           
            OffenePanel.Visible = false;
            AllesIstOkeyLabelLieferschein.Visible = false;
            ErrorLabelLieferschein.Visible = false;
            Guid orderId;
            if (RadGridLieferscheine.SelectedItems.Count > 0)
            {
                try
                {
                    DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));

                    List<LocationOrderJoins> locationIdList = new List<LocationOrderJoins>();
                    foreach (GridDataItem item in RadGridLieferscheine.SelectedItems)
                    {
                        var myOrder = dbContext.Order.FirstOrDefault(q => q.Id == new Guid(item["OrderId"].Text));
                        LocationOrderJoins orJ = new LocationOrderJoins();
                        orJ.LocationId = new Guid(item["locationId"].Text);
                        orJ.Order = myOrder;
                        locationIdList.Add(orJ);
                    }
                   var groupedOrder = locationIdList.GroupBy(q => q.LocationId);
                    foreach (var gr in groupedOrder)
                    {
                        var locationQuery = dbContext.Location.SingleOrDefault(q => q.Id == gr.First().LocationId);
                        var packingList = PackingList.CreatePackingList(locationQuery.Name, locationQuery.AdressId, dbContext);
                        packingList.LogDBContext = dbContext;
                        // für alle, die selected sind - ins package list hinzufügen
                        foreach (var orders in gr)
                        {
                            packingList.AddOrderById(orders.Order.Id, dbContext);
                            orders.Order.LogDBContext = dbContext;
                            orders.Order.PackingListId = packingList.Id;
                            orders.Order.ReadyToSend = true;
                        }
                        dbContext.SubmitChanges();
                        RadGridLieferscheine.DataBind();
                        OffenePanel.Visible = false;
                        NochOffenAuftraegeRadGrid.Enabled = false;
                        AllesIstOkeyLabelLieferschein.Visible = true;
                    }
                }
                catch (SqlException ex)
                {
                    ErrorLabelLieferschein.Visible = true;
                    ErrorLabelLieferschein.Text = "Fehler: " + ex.Message;
                }             
            }                  
        }
        protected void LieferscheineOffeneLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            Guid locationId = Guid.Empty;
            if (RadGridLieferscheine.SelectedItems.Count > 0)
            {
                GridDataItem item = RadGridLieferscheine.SelectedItems[0] as GridDataItem;
                locationId = new Guid(item["locationId"].Text);
                DataClasses1DataContext con = new DataClasses1DataContext();
                var largeCustomerQuery = from ord in con.Order
                                         join ordst in con.OrderStatus on ord.Status equals ordst.Id
                                         join cust in con.Customer on ord.CustomerId equals cust.Id
                                         join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                         join loc in con.Location on ord.LocationId equals loc.Id
                                         join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                         join reg in con.Registration on derord.RegistrationId equals reg.Id
                                         join veh in con.Vehicle on derord.VehicleId equals veh.Id
                                         where ord.Status == 400 && ordtype.Name == "Abmeldung" && loc.Id == locationId
                                         select new
                                         {
                                             OrderId = ord.Id,
                                             customerID = cust.Id,
                                             OrderNumber = ord.Ordernumber,
                                             CreateDate = ord.CreateDate,
                                             Status = ordst.Name,
                                             CustomerName =  cust.Name,
                                             Kennzeichen = reg.Licencenumber,
                                             VIN = veh.VIN,
                                             TSN = veh.TSN,
                                             HSN = veh.HSN,
                                             CustomerLocation = loc.Name,
                                             OrderTyp = ordtype.Name,
                                             HasError = ord.HasError,
                                             ErrorReason = ord.ErrorReason
                                         };
                e.Result = largeCustomerQuery;
            }
            else if (!String.IsNullOrEmpty(LocationIdHiddenField.Value))
            {
                DataClasses1DataContext con = new DataClasses1DataContext();
                var largeCustomerQuery = from ord in con.Order
                                         join ordst in con.OrderStatus on ord.Status equals ordst.Id
                                         join cust in con.Customer on ord.CustomerId equals cust.Id
                                         join ordtype in con.OrderType on ord.OrderTypeId equals ordtype.Id
                                         join loc in con.Location on ord.LocationId equals loc.Id
                                         join derord in con.DeregistrationOrder on ord.Id equals derord.OrderId
                                         join reg in con.Registration on derord.RegistrationId equals reg.Id
                                         join veh in con.Vehicle on derord.VehicleId equals veh.Id
                                         where ord.Status == 400 && ordtype.Name == "Abmeldung" && loc.Name == LocationIdHiddenField.Value
                                         select new
                                         {
                                             OrderId = ord.Id,
                                             customerID = cust.Id,
                                             OrderNumber = ord.Ordernumber,
                                             CreateDate = ord.CreateDate,
                                             Status = ordst.Name,
                                             CustomerName = cust.SmallCustomer.Person != null ? cust.SmallCustomer.Person.FirstName + "  " + cust.SmallCustomer.Person.Name : cust.Name,
                                             Kennzeichen = reg.Licencenumber,
                                             VIN = veh.VIN,
                                             TSN = veh.TSN,
                                             HSN = veh.HSN,
                                             CustomerLocation = loc.Name,
                                             OrderTyp = ordtype.Name,
                                             HasError = ord.HasError,
                                             ErrorReason = ord.ErrorReason
                                         };
                e.Result = largeCustomerQuery;
            }
            else
            {
                OffenePanel.Visible = false;
                NochOffenAuftraegeRadGrid.Enabled = false;
                AllesIsOkeyBeiOffene.Visible = false;
            }             
        }
        protected void FertigstellenButton_Clicked(object sender, GridCommandEventArgs e)
        {
            AllesIsOkeyBeiOffene.Visible = false;
            if (e.Item is GridDataItem)
            {
                GridDataItem fertigStellenItem = e.Item as GridDataItem;
                Guid orderId = new Guid(fertigStellenItem["OrderId"].Text);
                Guid customerID = new Guid(fertigStellenItem["customerID"].Text);
                if (!CheckDienstleistungAndAmtGebuhr(orderId))
                {
                    ErrorOffeneLabel.Text = "Bei den ausgewählten Auftrag fehlt noch die Dienstleistung und/oder amtliche Gebühr! In dem Reiter 'Zulassungstelle' können Sie den Auftrag bearbeiten. ";
                }
                else if (!CheckIfAllExistsToUpdate(fertigStellenItem))
                {
                    ErrorOffeneLabel.Text = "Fahrzeugdaten sind nicht vollständig! In dem Reiter 'Zulassungstelle' können Sie den Auftrag bearbeiten. ";
                }
                else
                {
                    try
                    {
                        DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString()));
                        var newOrder = dbContext.Order.Single(q => q.CustomerId == customerID && q.Id == orderId);
                        if (newOrder != null)
                        {
                            //updating order status
                            newOrder.LogDBContext = dbContext;
                            newOrder.Status = 600;
                            newOrder.ExecutionDate = DateTime.Now;
                            //updating orderitems status                          
                            foreach (OrderItem ordItem in newOrder.OrderItem)
                            {
                                ordItem.LogDBContext = dbContext;
                                if (ordItem.Status != (int)OrderItemState.Storniert)
                                {
                                    ordItem.Status = 600;
                                }
                            }
                            dbContext.SubmitChanges();
                            AllesIsOkeyBeiOffene.Visible = true;
                        }
                        AllesIstOkeyLabel.Text = "Lieferschein wurde erfolgreich erstellt.";
                    }
                    catch (SqlException ex)
                    {
                        ErrorOffeneLabel.Text = "Fehler: " + ex.Message; ;
                    }
                    RadGridLieferscheine.Rebind();
                    NochOffenAuftraegeRadGrid.Rebind();
                }
            }
        }
        //Checked if amt.gebühr UND mind.eine Dienstleistung vorhanden ist
        protected bool CheckDienstleistungAndAmtGebuhr(Guid orderId)
        {
            bool DienstVorhanden = false;
            bool AmtGebuhVorhanden = false;
            using (DataClasses1DataContext dbContext = new DataClasses1DataContext(new Guid(Session["CurrentUserId"].ToString())))
            {
                var searchOrderQuery = dbContext.Order.SingleOrDefault(q => q.Id == orderId);
                if (searchOrderQuery != null)
                {
                    foreach (OrderItem item in searchOrderQuery.OrderItem)
                    {
                        if (item.IsAuthorativeCharge == true)
                        {
                            AmtGebuhVorhanden = true;
                        }
                        else if (item.IsAuthorativeCharge == false)
                        {
                            DienstVorhanden = true;
                        }
                    }
                }
            }
            if (AmtGebuhVorhanden == true && DienstVorhanden == true)
                return true;
            else
                return false;
        }
        //Prüfen ob alle Werte da sind um den Auftrag auf "Zulassungstelle" zu setzen
        private bool CheckIfAllExistsToUpdate(GridDataItem fertigStellenItem)
        {
            bool shouldBeUpdated = true;
            if (String.IsNullOrEmpty(fertigStellenItem["VIN"].Text))
            {
                shouldBeUpdated = false;
            }            
            return shouldBeUpdated;
        }
    }
}