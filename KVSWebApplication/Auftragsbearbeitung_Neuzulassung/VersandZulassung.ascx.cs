﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KVSCommon.Database;
using Telerik.Web.UI;
using System.IO;
using System.Configuration;
using System.Transactions;
using KVSCommon.Enums;
namespace KVSWebApplication.Auftragsbearbeitung_Neuzulassung
{
    /// <summary>
    /// Codebehind fuer den Reiter Zulassung
    /// </summary>
    public partial class VersandZulassung : System.Web.UI.UserControl
    {
        RadScriptManager script = null;
        RadAjaxManager man = null;
        protected void RadGridVersand_PreRender(object sender, EventArgs e)
        {
            HideExpandColumnRecursive(RadGridVersand.MasterTableView);
        }
        public void HideExpandColumnRecursive(GridTableView tableView)
        {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            string target = Request["__EVENTTARGET"];
            AuftragsbearbeitungNeuzulassung auftragNeu = Page as AuftragsbearbeitungNeuzulassung;
            script = auftragNeu.getScriptManager() as RadScriptManager;
            man = auftragNeu.getRadAjaxManager1() as RadAjaxManager;        
            if (!String.IsNullOrEmpty(target))
                if (!target.Contains("LieferscheinDruckenButton") && !target.Contains("EditOffenColumn") && !target.Contains("DispatchOrderNumberBox") && !target.Contains("isSelfDispathCheckBox") && !target.Contains("EditButton"))
                    RadGridVersand.Rebind();
        }
        protected void VersandLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var versandQuery = from packList in con.PackingList
                               //join ord in con.Order on packList.Id equals ord.PackingListId
                               let CustomerNameLet = con.Order.Where(q => q.PackingListNumber == packList.PackingListNumber).FirstOrDefault()
                               orderby packList.PackingListNumber descending
                               select new
                               {
                                   listId = packList.PackingListNumber,
                                   CustomerName = 
                                     CustomerNameLet.Customer.SmallCustomer != null &&
                                        CustomerNameLet.Customer.SmallCustomer.Person != null ?
                                        CustomerNameLet.Customer.SmallCustomer.Person.FirstName + " " +
                                        CustomerNameLet.Customer.SmallCustomer.Person.Name : CustomerNameLet.Customer.Name, 
                                   Order = CustomerNameLet,
                                   listNumber = packList.PackingListNumber,
                                   isPrinted = packList.IsPrinted == true ? "Ja" : "Nein",
                                   PostBackUrl = packList.Document.FileName == null ? "" : "<a href=" + '\u0022' + packList.Document.FileName + '\u0022' + " target=" + '\u0022' + "_blank" + '\u0022' + "> Lieferschein " + packList.PackingListNumber + " öffnen</a>",
                                    DispatchOrderNumber = (packList != null)? packList.DispatchOrderNumber: "",
                                    IsSelf = (packList != null && packList.IsSelfDispatch.HasValue) ? packList.IsSelfDispatch.Value : false,
                               };
            versandQuery = versandQuery.Where(q => q.Order.OrderType.Id == (int)OrderTypes.Admission && q.Order.Status == (int)OrderStatusTypes.Closed);
            e.Result = versandQuery;
        }      
        protected void PruefenButton_Clicked(object sender, GridCommandEventArgs e)
        {            
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;          
                item.Selected = true;
                itemIndexHiddenField.Value = item.ItemIndex.ToString();
                var myListId = Int32.Parse(item["listId"].Text);
                DataClasses1DataContext dbContext = new DataClasses1DataContext();
                var myVerbringung = dbContext.PackingList.SingleOrDefault(q => q.PackingListNumber == myListId);
                if (myVerbringung.IsSelfDispatch == true)
                {
                    myDispathHiddenField.Value = "true";
                }
                else
                {
                    myDispathHiddenField.Value = myVerbringung.DispatchOrderNumber;
                }
            }
        }
        protected void OrdersDetailedTabel_DetailTable(object source, GridDetailTableDataBindEventArgs e)
        {
            var dbContext = new DataClasses1DataContext();
            var _item = (GridDataItem)e.DetailTableView.ParentItem;
            var listId = Int32.Parse(_item["listId"].Text);
            var orderQuery = from ord in dbContext.Order
                             where ord.PackingListNumber == listId && ord.Status == (int)OrderStatusTypes.Closed && 
                             ord.HasError.GetValueOrDefault(false) != true
                             select new
                             {
                                 OrderNumber = ord.OrderNumber,
                                 CustomerName = 
                                  ord.Customer.SmallCustomer != null &&
                                        ord.Customer.SmallCustomer.Person != null ?
                                        ord.Customer.SmallCustomer.Person.FirstName + " " +
                                        ord.Customer.SmallCustomer.Person.Name : ord.Customer.Name, 
                                 OrderLocation = ord.Location.Name,
                                 Status = ord.OrderStatus.Name,
                                 OrderType = ord.OrderType.Name,
                                 OrderError = ord.HasError == true ? "Ja" : "Nein"
                             };
            var item = (GridDataItem)e.DetailTableView.ParentItem;
            var nestedItem = (GridNestedViewItem)item.ChildItem;
            var radGrdEnquiriesVarients = (RadGrid)nestedItem.FindControl("RadGridVersandDetails");
            radGrdEnquiriesVarients.DataSource = orderQuery;
            radGrdEnquiriesVarients.DataBind();
        }
        protected void OrdersDetailedTabel_DetailTableDataBind(object source, GridNeedDataSourceEventArgs e)
        {
            var dbContext = new DataClasses1DataContext();
            var sender = source as RadGrid;
            var item = sender.Parent as Panel;
            // GridDataItem item = (GridDataItem)e.DetailTableView.ParentItem;
            var mylistId = item.FindControl("listIdBox") as TextBox;
            if (!String.IsNullOrEmpty(mylistId.Text))
            {
                var listId = Int32.Parse(mylistId.Text);
                var orderQuery = from ord in dbContext.Order
                                 where ord.PackingListNumber == listId && ord.Status == (int)OrderStatusTypes.Closed && ord.HasError.GetValueOrDefault(false) != true
                                 select new
                                 {
                                     OrderNumber = ord.OrderNumber,
                                     CustomerName = 
                                      ord.Customer.SmallCustomer != null &&
                                        ord.Customer.SmallCustomer.Person != null ?
                                        ord.Customer.SmallCustomer.Person.FirstName + " " +
                                        ord.Customer.SmallCustomer.Person.Name : ord.Customer.Name, 
                                     OrderLocation = ord.Location.Name,
                                     Status = ord.OrderStatus.Name,
                                     OrderType = ord.OrderType.Name,
                                     OrderError = ord.HasError == true ? "Ja" : "Nein"
                                 };
                sender.DataSource = orderQuery;
            }
        }
        protected void RadGridVersand_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {           
           if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editForm = e.Item as GridEditFormItem;                
            }         
        }        
        protected void DrueckenButton_Clicked(object sender, EventArgs e)
        {
            Button editButton = sender as Button;                    
                var ms = new MemoryStream();
                var item = editButton.Parent as Panel;
                (((GridNestedViewItem)(((GridTableCell)(item.Parent.Parent)).Parent))).ParentItem.Selected = true;
                var isSelfDispathCheckBox = item.FindControl("isSelfDispathCheckBox") as CheckBox;
                var DispatchOrderNumberBox = item.FindControl("DispatchOrderNumberBox") as RadTextBox;
                var ErrorVersandGedrucktLabel = item.FindControl("ErrorVersandGedrucktLabel") as Label;
                var _listId = item.FindControl("listIdBox") as TextBox;
                var listId = Int32.Parse(_listId.Text);
                var dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                var packList = dbContext.PackingList.SingleOrDefault(q => q.PackingListNumber == listId);
                if (packList.IsPrinted == true)
                {
                    ErrorVersandGedrucktLabel.Visible = true;
                }
                else
                {
                    packList.LogDBContext = dbContext;
                    try
                    {
                         if (isSelfDispathCheckBox.Checked == true && String.IsNullOrEmpty(DispatchOrderNumberBox.Text))
                        {
                            packList.IsSelfDispatch = true;
                        }
                        else if (isSelfDispathCheckBox.Checked == false && !String.IsNullOrEmpty(DispatchOrderNumberBox.Text))
                        {
                            packList.DispatchOrderNumber = DispatchOrderNumberBox.Text;
                            packList.IsSelfDispatch = false;
                        }
                        else
                        {
                            packList.DispatchOrderNumber = DispatchOrderNumberBox.Text;
                            packList.IsSelfDispatch = false;
                        }
                        dbContext.SubmitChanges();
                    }
                    catch(Exception ex)
                    {
                        ErrorVersandLabel.Visible = true;
                        ErrorVersandLabel.Text = "Fehler: " + ex.Message;
                    }
                    try
                    {
                        string myPackListFileName = EmptyStringIfNull.CheckIfFolderExistsAndReturnPathForPdf(Session["CurrentUserId"].ToString());
                        string myPathToSave = myPackListFileName;                        
                        packList.Print(ms, string.Empty, dbContext, "/UserData/" + Session["CurrentUserId"].ToString() + "/" + Path.GetFileName(myPackListFileName), false);
                        File.WriteAllBytes(myPathToSave, ms.ToArray());
                        string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                        string host = ConfigurationManager.AppSettings["smtpHost"];
                        dbContext.SubmitChanges();
                        packList.SendByEmail(ms, fromEmail, host);
                        RadGridVersand.DataBind();                     
                    }
                    catch (Exception ex)
                    {
                        ErrorVersandLabel.Visible = true;
                        ErrorVersandLabel.Text = "Fehler: " + ex.Message;
                    }           
            }        
        }
        protected void DispatchOrderNumberBoxText_Changed(object sender, EventArgs e)
        {
            RadTextBox dispachBox = sender as RadTextBox;
            CheckBox isSelf = dispachBox.Parent.FindControl("isSelfDispathCheckBox") as CheckBox;
            Button drucke = dispachBox.Parent.FindControl("LieferscheinDruckenButton") as Button;
            if (!String.IsNullOrEmpty(dispachBox.Text))
            {
                isSelf.Checked = false;
                isSelf.Enabled = false;
                dispachBox.Enabled = true;
            }
            else
            {
                isSelf.Checked = true;
                isSelf.Enabled = true;
                dispachBox.Enabled = false;
            }
        }
        protected void RadGridVersand_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {         
        }
        protected void isSelfDispathCheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox isSelf = sender as CheckBox;
            RadTextBox dispachBox = isSelf.Parent.FindControl("DispatchOrderNumberBox") as RadTextBox;
            Button drucke = dispachBox.Parent.FindControl("LieferscheinDruckenButton") as Button;
            script.RegisterPostBackControl(drucke);
            if (isSelf.Checked == true)
            {
                dispachBox.Text = "";
                dispachBox.Enabled = false;
            }
            else
            {
                dispachBox.Text = "";
                dispachBox.Enabled = true;
            }
        }
        protected void ReloadPanelButton_Clicked(object sender, EventArgs e)
        {
            RadGridVersand.Rebind();
        }
        protected void btnRemovePackingList_Click(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                try
                {
                    Button btnsender = sender as Button;
                    Label lblPackingListId = null;
                    if (btnsender != null)
                    {
                        lblPackingListId = btnsender.Parent.FindControl("lbllistId") as Label;
                    }
                    if (lblPackingListId != null && !String.IsNullOrEmpty(lblPackingListId.Text))
                    {
                        Order.TryToRemovePackingListIdAndSetStateToRegistration(dbContext, Int32.Parse(lblPackingListId.Text));
                        ts.Complete();
                    }
                    else
                    {
                        throw new Exception("Achtung, die Auftragsnummer konnte nicht identifiziert werden. Bitte aktualisieren Sie die Seite oder wenden Sie sich an den Administrator");
                    }
                }
                catch (Exception ex)
                {
                    if (ts != null)
                        ts.Dispose();

                    ErrorVersandLabel.Text = ex.Message;
                    ErrorVersandLabel.Visible = true;
                    try
                    {
                        dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                        dbContext.WriteLogItem("btnRemovePackingList_Click Error " + ex.Message, LogTypes.ERROR, "Order");
                        dbContext.SubmitChanges();
                    }
                    catch { }

                }
            }
            RadGridVersand.Rebind();

        }
    }
}