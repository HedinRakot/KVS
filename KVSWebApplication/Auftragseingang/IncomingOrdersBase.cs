﻿using KVSCommon.Database;
using KVSCommon.Enums;
using KVSCommon.Managers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace KVSWebApplication.Auftragseingang
{
    /// <summary>
    ///  Incoming Orders base class
    /// </summary>
    public abstract class IncomingOrdersBase : UserControl
    {
        #region Members

        public IncomingOrdersBase()
        {
            BicManager = (IBicManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IBicManager));
            UserManager = (IUserManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUserManager));
            OrderManager = (IOrderManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOrderManager));
            PriceManager = (IPriceManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPriceManager));
            ProductManager = (IProductManager)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IProductManager));
        }

        #region Common

        //TODO protected abstract HiddenField SessionId { get; }
        //protected abstract RadPersistenceManager RadPersistenceManager { get; }

        protected abstract PermissionTypes PagePermission { get; }

        protected List<Control> controls = new List<Control>();
        protected abstract Panel Panel { get; }
        protected abstract RadTextBox AccountNumberTextBox { get; }
        protected abstract RadTextBox BankCodeTextBox { get; }
        protected abstract RadTextBox BankNameTextBox { get;  }
        protected abstract RadTextBox IBANTextBox { get; }
        protected abstract RadTextBox BICTextBox { get; }
        protected abstract Label CustomerHistoryLabel { get; }
        protected abstract RadComboBox CustomerDropDown { get; }
        protected abstract RadComboBox LocationDropDown { get; }
        protected abstract RadTreeView ProductTree { get; }
        protected abstract RadScriptManager RadScriptManager { get; }

        #endregion

        #region Panels

        protected abstract Panel Vehicle_Variant_Panel { get; }
        protected abstract Panel Registration_GeneralInspectionDate_Panel { get; }
        protected abstract Panel CarOwner_Name_Panel { get; }
        protected abstract Panel CarOwner_Firstname_Panel { get; }
        protected abstract Panel Adress_StreetNumber_Panel { get; }
        protected abstract Panel Adress_Street_Panel { get; }
        protected abstract Panel Adress_Zipcode_Panel { get; }
        protected abstract Panel Adress_City_Panel { get; }
        protected abstract Panel Adress_Country_Panel { get; }
        protected abstract Panel Contact_Phone_Panel { get; }
        protected abstract Panel Contact_Fax_Panel { get; }
        protected abstract Panel Contact_MobilePhone_Panel { get; }
        protected abstract Panel Contact_Email_Panel { get; }
        protected abstract Panel BankAccount_BankName_Panel { get; }
        protected abstract Panel BankAccount_Accountnumber_Panel { get; }
        protected abstract Panel BankAccount_BankCode_Panel { get; }
        protected abstract Panel Registration_eVBNumber_Panel { get; }
        protected abstract Panel Vehicle_HSN_Panel { get; }
        protected abstract Panel Vehicle_TSN_Panel { get; }
        protected abstract Panel Vehicle_VIN_Panel { get; }
        protected abstract Panel Registration_Licencenumber_Panel { get; }
        protected abstract Panel RegistrationOrder_PreviousLicencenumber_Panel { get; }
        protected abstract Panel Registration_EmissionCode_Panel { get; }
        protected abstract Panel Registration_RegistrationDocumentNumber_Panel { get; }
        protected abstract Panel Vehicle_FirstRegistrationDate_Panel { get; }
        protected abstract Panel Vehicle_Color_Panel { get; }
        protected abstract Panel IBANPanel_Panel { get; }

        #endregion

        #region Managers

        public IBicManager BicManager { get; set; }
        public IUserManager UserManager { get; set; }
        public IOrderManager OrderManager { get; set; }
        public IPriceManager PriceManager { get; set; }
        public IProductManager ProductManager { get; set; }

        #endregion

        #endregion

        #region Event Handlers

        protected void genIban_Click(object sender, EventArgs e)
        {
            if (EmptyStringIfNull.IsNumber(AccountNumberTextBox.Text) &&
                !String.IsNullOrEmpty(BankNameTextBox.Text) &&
                EmptyStringIfNull.IsNumber(BankCodeTextBox.Text))
            {
                IBANTextBox.Text = "DE" + (98 - ((62 * ((1 + long.Parse(BankCodeTextBox.Text) % 97)) +
                    27 * (long.Parse(AccountNumberTextBox.Text) % 97)) % 97)).ToString("D2");
                IBANTextBox.Text += long.Parse(BankCodeTextBox.Text).ToString("00000000").Substring(0, 4);
                IBANTextBox.Text += long.Parse(BankCodeTextBox.Text).ToString("00000000").Substring(4, 4);
                IBANTextBox.Text += long.Parse(AccountNumberTextBox.Text).ToString("0000000000").Substring(0, 4);
                IBANTextBox.Text += long.Parse(AccountNumberTextBox.Text).ToString("0000000000").Substring(4, 4);
                IBANTextBox.Text += long.Parse(AccountNumberTextBox.Text).ToString("0000000000").Substring(8, 2);

                if (BICTextBox != null)
                {
                    var bicNr = BicManager.GetBicByCodeAndName(BankCodeTextBox.Text, BankNameTextBox.Text);
                    if (bicNr != null && !String.IsNullOrEmpty(bicNr.BIC))
                    {
                        BICTextBox.Text = bicNr.BIC.ToString();
                    }
                }
            }
        }

        // Suche nach Price. Falls keine gibt - stand.Price nehmen
        protected Price findPrice(string productId)
        {
            int? locationId = null;
            Price newPrice = null;

            if (LocationDropDown != null && !String.IsNullOrEmpty(LocationDropDown.SelectedValue))
            {
                locationId = Int32.Parse(LocationDropDown.SelectedValue);
                newPrice = PriceManager.GetEntities(q => q.ProductId == Int32.Parse(productId) && q.LocationId == locationId).FirstOrDefault();
            }

            if (String.IsNullOrEmpty(this.LocationDropDown.SelectedValue) || newPrice == null)
            {
                newPrice = PriceManager.GetEntities(q => q.ProductId == Int32.Parse(productId) && q.LocationId == null).FirstOrDefault();
            }
            return newPrice;
        }

        #endregion

        #region Methods
        
        protected void CheckUserPermissions()
        {
            if (UserManager.CheckPermissionsForUser(Session["UserPermissions"], PagePermission))
            {
                Panel.Enabled = true;
            }
        }
        protected void CheckUmsatzForSmallCustomer()
        {
            CustomerHistoryLabel.Visible = true;

            var orders = OrderManager.GetEntities(o => o.Status == (int)OrderStatusTypes.Payed);

            if (!String.IsNullOrEmpty(CustomerDropDown.SelectedValue))
            {
                orders = orders.Where(q => q.CustomerId == Int32.Parse(CustomerDropDown.SelectedValue));
            }

            orders = orders.ToList();

            decimal gebuehren = 0;
            decimal umsatz = 0;
            //Amtliche Gebühr
            foreach (var order in orders)
            {
                foreach (OrderItem orderItem in order.OrderItem)
                {
                    if (orderItem.IsAuthorativeCharge && orderItem.Status == (int)OrderItemStatusTypes.Payed)
                        gebuehren = gebuehren + orderItem.Amount;
                    else if (!orderItem.IsAuthorativeCharge && orderItem.Status == (int)OrderItemStatusTypes.Payed)
                        umsatz = umsatz + orderItem.Amount;
                }
            }

            CustomerHistoryLabel.Text = String.Format("Historie: <br/> Gesamt: {0} <br/> Umsatz: {1}<br/> Gebühren: {2}",
                orders.Count(), umsatz.ToString("C2"), gebuehren.ToString("C2"));
        }

        protected string CheckIfAllProduktsHavingPrice(int? locationId)
        {
            string result = "";

            foreach (RadTreeNode node in ProductTree.Nodes)
            {
                if (!String.IsNullOrEmpty(node.Value))
                {
                    string[] splited = node.Value.Split(';');
                    if (splited.Length == 2)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(splited[0]))
                            {
                                var product = ProductManager.GetById(Int32.Parse(splited[0]));
                                if (!locationId.HasValue) //small customer
                                {
                                    var price = PriceManager.GetEntities(q => q.ProductId == product.Id && q.LocationId == null).FirstOrDefault();
                                    if (price == null)
                                    {
                                        result += " " + node.Text + " ";
                                    }
                                }
                                else //large customer
                                {
                                    var newPrice = PriceManager.GetEntities(q => q.ProductId == product.Id && q.LocationId == locationId.Value).FirstOrDefault();

                                    if (newPrice == null)
                                        newPrice = PriceManager.GetEntities(q => q.ProductId == product.Id && q.LocationId == null).FirstOrDefault();

                                    if (newPrice == null)
                                    {
                                        result += " " + node.Text + " ";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return "";
                        }
                    }
                }
            }
            return result;
        }

        protected List<Control> getAllControls()
        {
            if (controls.Count == 0)
            {
                controls.Add(Vehicle_Variant_Panel);
                controls.Add(Registration_GeneralInspectionDate_Panel);
                controls.Add(CarOwner_Name_Panel);
                controls.Add(CarOwner_Firstname_Panel);
                controls.Add(Adress_StreetNumber_Panel);
                controls.Add(Adress_Street_Panel);
                controls.Add(Adress_Zipcode_Panel);
                controls.Add(Adress_City_Panel);
                controls.Add(Adress_Country_Panel);
                controls.Add(Contact_Phone_Panel);
                controls.Add(Contact_Fax_Panel);
                controls.Add(Contact_MobilePhone_Panel);
                controls.Add(Contact_Email_Panel);
                controls.Add(BankAccount_BankName_Panel);
                controls.Add(BankAccount_Accountnumber_Panel);
                controls.Add(BankAccount_BankCode_Panel);
                controls.Add(Registration_eVBNumber_Panel);
                controls.Add(Vehicle_HSN_Panel);
                controls.Add(Vehicle_TSN_Panel);
                controls.Add(Vehicle_VIN_Panel);
                controls.Add(Registration_Licencenumber_Panel);
                controls.Add(RegistrationOrder_PreviousLicencenumber_Panel);
                controls.Add(Registration_EmissionCode_Panel);
                controls.Add(Registration_RegistrationDocumentNumber_Panel);
                controls.Add(Vehicle_FirstRegistrationDate_Panel);
                controls.Add(Vehicle_Color_Panel);
                controls.Add(IBANPanel_Panel);
            }
            return controls;
        }

        #endregion

        #region Private Methods


        #endregion
    }
}