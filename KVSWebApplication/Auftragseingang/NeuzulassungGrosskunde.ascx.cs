﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KVSCommon.Database;
using System.IO;
using System.Configuration;
using Telerik.Web.UI;
using KVSCommon.Enums;
namespace KVSWebApplication.Auftragseingang
{
    /// <summary>
    /// Neuzulassung Grosskunde
    /// </summary>
    public partial class NeuzulassungGrosskunde1 : System.Web.UI.UserControl
    {
        List<Control> controls = new List<Control>();
        protected void Page_Load(object sender, EventArgs e)
        {
            NeuzulassungGrosskunde auftragsEingang = Page as NeuzulassungGrosskunde;
            RadScriptManager script = auftragsEingang.getScriptManager() as RadScriptManager;
            script.RegisterPostBackControl(AddAdressButton);
            //first registration bekommt immer heutige Datum by default
            FirstRegistrationDateBox.SelectedDate = DateTime.Now;
            if (Session["CurrentUserId"] != null)
            {
                if (!String.IsNullOrEmpty(Session["CurrentUserId"].ToString()))
                {
                    CheckUserPermissions();
                }
            }
        }
        protected void CheckUserPermissions()
        {
            List<string> userPermissions = new List<string>();
            userPermissions.AddRange(KVSCommon.Database.User.GetAllPermissionsByID(Int32.Parse(Session["CurrentUserId"].ToString())));
            if (userPermissions.Count > 0)
            {
                if (userPermissions.Contains("ZULASSUNGSAUFTRAG_ANLEGEN"))
                {
                    ZulassungPanel.Enabled = true;
                }
            }
        }
        protected void CheckUmsatzForSmallCustomer()
        {
            SmallCustomerHistorie.Visible = true;
            DataClasses1DataContext con = new DataClasses1DataContext();
            var newQuery = from ord in con.Order
                           let registration = ord.RegistrationOrder != null ? ord.RegistrationOrder.Registration : ord.DeregistrationOrder.Registration
                           where ord.Status == (int)OrderStatusTypes.Payed
                           select new
                           {
                               OrderNumber = ord.OrderNumber,
                               CustomerId = ord.CustomerId,
                           };
            if (!String.IsNullOrEmpty(CustomerDropDownList.SelectedValue))
            {
                try
                {
                    newQuery = newQuery.Where(q => q.CustomerId == Int32.Parse(CustomerDropDownList.SelectedValue));
                }
                catch (Exception ex)
                {
                    SubmitChangesErrorLabel.Text = ex.Message;
                    SubmitChangesErrorLabel.Visible = true;
                }
            }
            // Allgemein
            string countAuftrag = newQuery.Count().ToString();
            decimal gebuehren = 0;
            decimal umsatz = 0;
            //Amtliche Gebühr
            foreach (var newOrder in newQuery)
            {
                var order = con.Order.SingleOrDefault(q => q.OrderNumber == newOrder.OrderNumber);
                if (order != null)
                {
                    foreach (OrderItem orderItem in order.OrderItem)
                    {
                        if (orderItem.IsAuthorativeCharge && orderItem.Status == (int)OrderItemStatusTypes.Payed)
                            gebuehren = gebuehren + orderItem.Amount;
                        else if (!orderItem.IsAuthorativeCharge && orderItem.Status == (int)OrderItemStatusTypes.Payed)
                            umsatz = umsatz + orderItem.Amount;
                    }
                }
            }
            SmallCustomerHistorie.Text = "Historie: <br/> Gesamt: " + countAuftrag + " <br/> Umsatz: " + umsatz.ToString("C2") + "<br/> Gebühren: " + gebuehren.ToString("C2");
        }
        #region Methods
        // Suche nach Price. Falls keine gibt - stand.Price nehmen
        private Price findPrice(string productId)
        {
            DataClasses1DataContext dbContext = new DataClasses1DataContext();
            int? locationId = null;
            Price newPrice = null;
            if (!String.IsNullOrEmpty(this.LocationDropDownList.SelectedValue))
            {
                locationId = Int32.Parse(LocationDropDownList.SelectedValue);
                newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == Int32.Parse(productId) && q.LocationId == locationId);
            }
            if (String.IsNullOrEmpty(this.LocationDropDownList.SelectedValue) || newPrice == null)
            {
                newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == Int32.Parse(productId) && q.LocationId == null);
            }
            return newPrice;
        }
        #endregion
        #region Index Changed
        protected void SmallLargeCustomer_Changed(object sender, EventArgs e)
        {
            CustomerDropDownList.DataBind();
            LocationDropDownList.Enabled = true;
            CostCenterDropDownList.Enabled = true;
            CustomerDropDownList.Focus();
            ProductDropDownList.DataSource = null;
            ProductDropDownList.DataBind();
        }
        protected void DeleteNewPosButton_Clicked(object sender, EventArgs e)
        {
            if (DienstleistungTreeView.SelectedNodes.Count > 0)
            {
                DienstleistungTreeView.SelectedNode.Remove();
            }
        }
        protected void NewPosButton_Clicked(object sender, EventArgs e)
        {
            IRadTreeNodeContainer target = DienstleistungTreeView;
            if (DienstleistungTreeView.SelectedNode != null)
            {
                DienstleistungTreeView.SelectedNode.Expanded = true;
                target = DienstleistungTreeView.SelectedNode;
            }
            if (!String.IsNullOrEmpty(ProductDropDownList.Text) && !String.IsNullOrEmpty(ProductDropDownList.SelectedValue))
            {
                string costCenter = "";
                if (!String.IsNullOrEmpty(CostCenterDropDownList.SelectedValue.ToString()))
                {
                    costCenter = (CostCenterDropDownList.SelectedValue.ToString());
                }
                else
                    costCenter = "";
                string value = ProductDropDownList.SelectedValue.ToString() + ";" + costCenter;
                RadTreeNode addedNode = new RadTreeNode(ProductDropDownList.Text + "(" + CostCenterDropDownList.Text + ")", value);//+ ";" + CostCenterDropDownList.SelectedValue == "SmallCustomer" ? "" : CostCenterDropDownList.SelectedValue);
                target.Nodes.Add(addedNode);
            }
        }
        protected void RegistrationTyp_Changed(object sender, EventArgs e)
        {
            KennzeichenTauschButton.Visible = false;
            ProductDropDownList.ClearSelection();
            ProductDropDownList.ClearCheckedItems();
            ProductDropDownList.SelectedValue = "";
            ProductDropDownList.Items.Clear();
            ProductDropDownList.DataBind();
            ProductDropDownList.Focus();
            setCarOwnerData();
        }
        protected void CustomerIndex_Changed(object sender, EventArgs e)
        {
            ProductDropDownList.Text = null;
            ProductDropDownList.ClearSelection();
            LocationDropDownList.Text = null;
            LocationDropDownList.ClearSelection();
            RegistrationOrderDropDownList.Text = null;
            RegistrationOrderDropDownList.ClearSelection();
            CostCenterDropDownList.Text = null;
            CostCenterDropDownList.ClearSelection();
            LocationDropDownList.DataBind();
            ProductDropDownList.DataBind();
            CostCenterDropDownList.DataBind();
            LocationDropDownList.Focus();
            ZulassungOkLabel.Visible = false;
            Adress_StreetBox.Text = string.Empty;
            Adress_StreetNumberBox.Text = string.Empty;
            Adress_ZipcodeBox.Text = string.Empty;
            Adress_CityBox.Text = string.Empty;
            Adress_CountryBox.Text = string.Empty;
            CarOwner_NameBox.Text = string.Empty;
            Registration_eVBNumberBox.Text = string.Empty;
            if (!String.IsNullOrEmpty(CustomerDropDownList.SelectedValue.ToString()))
            {
                CheckFields(getAllControls());
                SmallCustomerHistorie.Visible = false;
            }
        }
        #endregion
        #region Linq Data Source
        protected void ProductDataSourceLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var selectedCustomer = 0;
            var location = 0;
            if (!String.IsNullOrEmpty(CustomerDropDownList.SelectedValue))
                selectedCustomer = Int32.Parse(CustomerDropDownList.SelectedValue);
            if (!String.IsNullOrEmpty(LocationDropDownList.SelectedValue))
                location = Int32.Parse(LocationDropDownList.SelectedValue);
            IQueryable productQuery = null;
            if (!String.IsNullOrEmpty(RegistrationOrderDropDownList.SelectedValue))
            {
                productQuery = from p in con.Price
                               join prA in con.PriceAccount on p.Id equals prA.PriceId
                               where (p.Product.RegistrationOrderTypeId == Int32.Parse(RegistrationOrderDropDownList.SelectedValue) ||
                               p.Product.OrderType.Id == (int)OrderTypes.Common)
                               && p.Location.CustomerId == selectedCustomer
                               && p.LocationId == location
                               select new
                               {
                                   ItemNumber = p.Product.ItemNumber,
                                   Name = p.Product.Name,
                                   Value = p.Product.Id,
                                   Category = p.Product.ProductCategory.Name
                               };
            }
            else
            {
                productQuery = from p in con.Price
                               join prA in con.PriceAccount on p.Id equals prA.PriceId
                               where p.Location.CustomerId == null
                               select new
                               {
                                   ItemNumber = p.Product.ItemNumber,
                                   Name = p.Product.Name,
                                   Value = p.Product.Id,
                                   Category = p.Product.ProductCategory.Name
                               };
            }

            //werden die vordefinierte Kundenprodukte in TreeNode geladen
            if (productQuery != null && location != 0 && selectedCustomer != 0 && !String.IsNullOrEmpty(RegistrationOrderDropDownList.SelectedValue))
            {
                LoadCustomerProductsInTreeView(selectedCustomer, location);
            }

            e.Result = productQuery;
        }

        private void LoadCustomerProductsInTreeView(int selectedCustomer, int location)
        {
            using (DataClasses1DataContext con = new DataClasses1DataContext())
            {
                var productQuery = from p in con.Price
                                   join prA in con.PriceAccount on p.Id equals prA.PriceId
                                   where (p.Product.RegistrationOrderTypeId == Int32.Parse(RegistrationOrderDropDownList.SelectedValue) ||
                                   p.Product.OrderType.Id == (int)OrderTypes.Common)
                                   && p.Location.CustomerId == selectedCustomer
                                   && p.LocationId == location
                                   select new
                                   {
                                       ItemNumber = p.Product.ItemNumber,
                                       Name = p.Product.Name,
                                       Value = p.Product.Id,
                                       Category = p.Product.ProductCategory.Name
                                   };


                var custProds = con.CustomerProduct.Where(q => q.CustomerId == selectedCustomer);
                if (custProds != null)
                {
                    foreach (var product in productQuery)
                    {
                        var myProd = custProds.SingleOrDefault(q => q.ProductId == product.Value);
                        if (myProd != null)
                        {
                            IRadTreeNodeContainer target = DienstleistungTreeView;

                            string costCenter = "";
                            if (!String.IsNullOrEmpty(CostCenterDropDownList.SelectedValue.ToString()))
                            {
                                costCenter = (CostCenterDropDownList.SelectedValue.ToString());
                            }
                            else
                                costCenter = "";

                            string value = product.Value + ";" + costCenter;

                            RadTreeNode addedNode = new RadTreeNode(product.Name, value);//+ ";" + CostCenterDropDownList.SelectedValue == "SmallCustomer" ? "" : CostCenterDropDownList.SelectedValue);
                            target.Nodes.Add(addedNode);
                        }
                    }
                }
            }
        }

        // Auswahl der KundenName
        protected void CustomerLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var customerQuery = from cust in con.Customer
                                where cust.Id == cust.LargeCustomer.CustomerId
                                orderby cust.Name
                                select new
                                {
                                    Name = cust.Name,
                                    Value = cust.Id,
                                    Matchcode = cust.MatchCode,
                                    Kundennummer = cust.CustomerNumber
                                };
            e.Result = customerQuery;
        }
        protected void ZulassungsstelleDataSourceLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var zulassungsstelleQuery = from zul in con.RegistrationLocation
                                        orderby zul.RegistrationLocationName
                                        select new
                                        {
                                            Name = zul.RegistrationLocationName,
                                            Value = zul.ID
                                        };
            e.Result = zulassungsstelleQuery;
        }
        protected void LocationLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();

            if (!String.IsNullOrEmpty(CustomerDropDownList.SelectedValue.ToString()))
            {
                var locationQuery = from loc in con.Location
                                    join cust in con.Customer on loc.CustomerId equals cust.Id
                                    where loc.CustomerId == Int32.Parse(CustomerDropDownList.SelectedValue)
                                    select new
                                    {
                                        Name = loc.Name,
                                        Value = loc.Id
                                    };
                e.Result = locationQuery;
            }
            else
            {
                var locationQuery = from loc in con.Location
                                    join cust in con.Customer on loc.CustomerId equals cust.Id
                                    where loc.CustomerId == null
                                    select new
                                    {
                                        Name = loc.Name,
                                        Value = loc.Id
                                    };
                e.Result = locationQuery;
            }
        }
        protected void CostCenterLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            if (!String.IsNullOrEmpty(CustomerDropDownList.SelectedValue.ToString()))
            {
                var costCenterQuery = from cost in con.CostCenter
                                      join cust in con.Customer on cost.CustomerId equals cust.Id
                                      where cost.CustomerId == Int32.Parse(CustomerDropDownList.SelectedValue)
                                      select new
                                      {
                                          Name = cost.Name,
                                          Value = cost.Id
                                      };
                e.Result = costCenterQuery;
            }
            else
            {
                var costCenterQuery = from cost in con.CostCenter
                                      join cust in con.Customer on cost.CustomerId equals cust.Id
                                      //where cost.CustomerId == 0 //TODO
                                      select new
                                      {
                                          Name = cost.Name,
                                          Value = cost.Id
                                      };
                e.Result = costCenterQuery;
            }
        }
        protected void RegistrationOrderDataSourceLinq_Selected(object sender, LinqDataSourceSelectEventArgs e)
        {
            DataClasses1DataContext con = new DataClasses1DataContext();
            var regOrdQuery = from regord in con.RegistrationOrderType
                              select new
                              {
                                  Name = regord.Name,
                                  Value = regord.Id
                              };
            e.Result = regOrdQuery;
        }
        #endregion
        protected void CheckIfButtonShouldBeEnabled()
        {
            if (AuftragZulassenButton.Enabled == true)
            {
                //falls keine Pflichtfelder angezeigt sind - button schließen
                List<Control> allControls = getAllControls();
                foreach (Control control in allControls)
                {
                    if (control.Visible == true)
                    {
                        AuftragZulassenButton.Enabled = true;
                        break;
                    }
                    else
                        AuftragZulassenButton.Enabled = false;
                }
            }
        }
        //VIN ist eingegeben, versuch das Fahrzeug zu finden
        protected void VinBoxZulText_Changed(object sender, EventArgs e)
        {
            bool finIsOkey = false;
            FahrzeugLabel.Text = "Fahrzeug";
            FahrzeugLabel.ForeColor = System.Drawing.Color.Blue;
            if (VINBox.Text.Length == 18 && !VINBox.Text.Contains('O') && !VINBox.Text.Contains('o'))
            {
                finIsOkey = true;
                PruefzifferBox.Text = VINBox.Text.Substring(17);
                VINBox.Text = VINBox.Text.Substring(0, 17);
                PruefzifferBox.Focus();
            }
            else if (VINBox.Text.Length == 17 && !VINBox.Text.Contains('O') && !VINBox.Text.Contains('o'))
            {
                finIsOkey = true;
                PruefzifferBox.Focus();
            }
            else if (VINBox.Text.Length == 8)
            {
                finIsOkey = true;
                PruefzifferBox.Focus();
            }
            else // fin ist nicht korrekt
            {
                if (VINBox.Text.Contains('O') || VINBox.Text.Contains('o'))
                {
                    FahrzeugLabel.Text = "FIN darf nicht 'O' oder 'o' beinhalten!";
                    FahrzeugLabel.ForeColor = System.Drawing.Color.Red;
                    VINBox.Focus();
                }
                if (VINBox.Text.Length > 18 || VINBox.Text.Length < 17)
                {
                    FahrzeugLabel.Text = "FIN kann nur entweder 17 oder 8-stellig sein!";
                    FahrzeugLabel.ForeColor = System.Drawing.Color.Red;
                    VINBox.Focus();
                }
            }
            if (finIsOkey == true)
            {
                try
                {
                    VINBox.Text = VINBox.Text.ToUpper();
                    DataClasses1DataContext dbContext = new DataClasses1DataContext();
                    var autoQuery = dbContext.Vehicle.SingleOrDefault(q => q.VIN == VINBox.Text);
                    if (autoQuery != null)
                    {
                        //wird als cache field für die Kennzeichnung bei der Umkennzeichnung benutzt
                        if (autoQuery.CurrentRegistrationId.HasValue)
                        {
                            var registration = dbContext.Registration.Single(q => q.Id == autoQuery.CurrentRegistrationId.Value);
                        
                            string kennzeichen = string.Empty;
                            LicenceNumberCacheField.Value = registration.Licencenumber;
                            RegistrationIdField.Value = registration.Id.ToString();
                            kennzeichen = registration.Licencenumber;
                            string[] newKennzeichen = kennzeichen.Split('-');
                            if (newKennzeichen.Length == 3)
                            {
                                LicenceBox1.Text = newKennzeichen[0];
                                LicenceBox2.Text = newKennzeichen[1];
                                LicenceBox3.Text = newKennzeichen[2];
                            }
                            Registration_GeneralInspectionDateBox.SelectedDate = registration.GeneralInspectionDate;
                            RegDocNumBox.Text = registration.RegistrationDocumentNumber;
                            EmissionsCodeBox.Text = registration.EmissionCode;

                            CarOwner owner = registration.CarOwner;
                            if (owner != null)
                            {
                                CarOwner_NameBox.Text = owner.Name;
                                if (owner.Adress != null)
                                {
                                    Adress_StreetBox.Text = owner.Adress.Street;
                                    CarOwner_FirstnameBox.Text = owner.FirstName;
                                    Adress_StreetNumberBox.Text = owner.Adress.StreetNumber;
                                    Adress_ZipcodeBox.Text = owner.Adress.Zipcode;
                                    Adress_CityBox.Text = owner.Adress.City;
                                    Adress_CountryBox.Text = owner.Adress.Country;
                                }
                                if (owner.Contact != null)
                                {
                                    Contact_PhoneBox.Text = owner.Contact.Phone;
                                    Contact_FaxBox.Text = owner.Contact.Fax;
                                    Contact_MobilePhoneBox.Text = owner.Contact.MobilePhone;
                                    Contact_EmailBox.Text = owner.Contact.Email;
                                }
                                if (owner.BankAccount != null)
                                {
                                    BankAccount_BankNameBox.Text = owner.BankAccount.BankName;
                                    BankAccount_AccountnumberBox.Text = owner.BankAccount.Accountnumber;
                                    BankAccount_BankCodeBox.Text = owner.BankAccount.BankCode;
                                }
                                PruefzifferBox.Focus();
                            }
                        }

                        VehicleIdField.Value = autoQuery.Id.ToString();
                        Vehicle_VariantBox.Text = autoQuery.Variant;
                        HSNBox.Text = autoQuery.HSN;
                        TSNBox.Text = autoQuery.TSN;
                        Vehicle_ColorBox.Text = autoQuery.ColorCode.ToString();
                    }
                }
                // falls kein Fahrzeug gefunden
                catch (Exception ex)
                {
                    VINBox.Focus();
                }
            }
        }
        #region Button Clicked
        //Neue Auftragseingang
        protected void AuftragZulassenButton_Clicked(object sender, EventArgs e)
        {
            int? locationId = null;
            string ProduktId = "";
            string CostCenterId = "";
            ZulassungOkLabel.Visible = false;
            SubmitChangesErrorLabel.Visible = false;
            ErrorLeereTextBoxenLabel.Visible = false;
            if (!String.IsNullOrEmpty(LocationDropDownList.SelectedValue))
                locationId = Int32.Parse(LocationDropDownList.SelectedValue);

            if (CheckIfBoxenNotEmpty()) //gibt es leer boxen, die angezeigt sind.
            {
                if (DienstleistungTreeView.Nodes.Count == 0)
                {
                    ErrorLeereTextBoxenLabel.Text = "Bitte Dienstleistung hinzufügen!";
                    ErrorLeereTextBoxenLabel.Visible = true;
                }
                else if (String.IsNullOrEmpty(CostCenterDropDownList.SelectedValue))
                {
                    ErrorLeereTextBoxenLabel.Text = "Bitte tragen Sie die Kostenstelle ein!";
                    ErrorLeereTextBoxenLabel.Visible = true;
                }
                else if (String.IsNullOrEmpty(LocationDropDownList.SelectedValue))
                {
                    ErrorLeereTextBoxenLabel.Text = "Bitte wählen Sie der Standort aus!";
                    ErrorLeereTextBoxenLabel.Visible = true;
                }
                else if (String.IsNullOrEmpty(ZulassungsstelleComboBox.SelectedValue))
                {
                    ErrorLeereTextBoxenLabel.Text = "Bitte wählen Sie die Zulassungsstelle aus!";
                    ErrorLeereTextBoxenLabel.Visible = true;
                }
                else
                {
                    ErrorLeereTextBoxenLabel.Text = "Bitte Pflichtfelder überprüfen!";
                    ErrorLeereTextBoxenLabel.Visible = true;
                }
            }
            else if (!String.IsNullOrEmpty(CheckIfAllProduktsHavingPrice(locationId)))
            {
                ErrorLeereTextBoxenLabel.Text = "Für " + CheckIfAllProduktsHavingPrice(locationId) + " wurde kein Price gefunden!";
                ErrorLeereTextBoxenLabel.Visible = true;
                return;
            }
            else if (DienstleistungTreeView.Nodes.Count > 0)
            {
                RadTreeNode node = DienstleistungTreeView.Nodes[0];
                string[] splited = node.Value.Split(';');
                ProduktId = splited[0];
                CostCenterId = splited[1];
                if (CostCenterId == string.Empty)
                    CostCenterId = CostCenterDropDownList.SelectedValue;

                ErrorLeereTextBoxenLabel.Visible = false;
                try
                {
                    ZulassungOkLabel.Visible = false;
                    SubmitChangesErrorLabel.Visible = false;
                    string kennzeichen = string.Empty,
                    oldKennzeichen = string.Empty;
                    DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                    Adress newAdress = null;
                    Contact newContact = null;
                    BankAccount newBankAccount = null;
                    CarOwner newCarOwner = null;
                    Registration newRegistration = null;
                    Price price = null;
                    RegistrationOrder newKennzeichenRegOrder = null;
                    OrderItem newOrderItem1 = null;
                    OrderItem newOrderItem2 = null;
                    Vehicle newVehicle = null;
                    DateTime? FirstRegistrationDate = null;

                    int? color = null;
                    if (!String.IsNullOrEmpty(LicenceBox1.Text))
                        kennzeichen = LicenceBox1.Text + "-" + LicenceBox2.Text + "-" + LicenceBox3.Text;
                    if (!String.IsNullOrEmpty(PreviousLicenceBox1.Text))
                        oldKennzeichen = PreviousLicenceBox1.Text + "-" + PreviousLicenceBox2.Text + "-" + PreviousLicenceBox3.Text;


                    CostCenter costCenter = null;

                    if (!String.IsNullOrEmpty(CostCenterId))
                    {
                        costCenter = dbContext.CostCenter.FirstOrDefault(o => o.Id == Int32.Parse(CostCenterId));
                    }

                    if (!String.IsNullOrEmpty(FirstRegistrationDateBox.SelectedDate.ToString()))
                        FirstRegistrationDate = FirstRegistrationDateBox.SelectedDate;
                    if (!String.IsNullOrEmpty(Vehicle_ColorBox.Text))
                        color = Convert.ToInt32(Vehicle_ColorBox.Text);


                    if (RegistrationOrderDropDownList.Text.Contains("Umkennzeichnung")) // Umkennzeichnung
                    {
                        FahrzeugLabel.Text = "Fahrzeug";
                        FahrzeugLabel.ForeColor = System.Drawing.Color.Blue;
                        if (!String.IsNullOrEmpty(kennzeichen))
                        {
                            if (!String.IsNullOrEmpty(VehicleIdField.Value)) //falls auto gefunden wurde
                            {
                                newVehicle = dbContext.Vehicle.SingleOrDefault(q => q.Id == Int32.Parse(VehicleIdField.Value));
                            }
                            else // neues Auto muss angelegt werden
                            {
                                newVehicle = Vehicle.CreateVehicle(VINBox.Text, HSNBox.Text, TSNBox.Text, Vehicle_VariantBox.Text, FirstRegistrationDate, color, dbContext);
                            }

                            // another logic after new/existing Vehicle
                            newAdress = Adress.CreateAdress(Adress_StreetBox.Text, Adress_StreetNumberBox.Text, Adress_ZipcodeBox.Text, Adress_CityBox.Text, Adress_CountryBox.Text, dbContext);
                            newContact = Contact.CreateContact(Contact_PhoneBox.Text, Contact_FaxBox.Text, Contact_MobilePhoneBox.Text, Contact_EmailBox.Text, dbContext);
                            newBankAccount = BankAccount.CreateBankAccount(dbContext, BankAccount_BankNameBox.Text, BankAccount_AccountnumberBox.Text,
                                BankAccount_BankCodeBox.Text, txbBancAccountIban.Text, txbBancAccountBIC.Text);


                            newCarOwner = CarOwner.CreateCarOwner(CarOwner_NameBox.Text, CarOwner_FirstnameBox.Text, newBankAccount, newContact, newAdress, dbContext);
                            DateTime newZulassungsDatum = DateTime.Now;
                            if (ZulassungsdatumPicker.SelectedDate != null)
                            {
                                if (!string.IsNullOrEmpty(ZulassungsdatumPicker.SelectedDate.ToString()))
                                {
                                    newZulassungsDatum = (DateTime)ZulassungsdatumPicker.SelectedDate;
                                }
                            }
                            newRegistration = Registration.CreateRegistration(newCarOwner, newVehicle, kennzeichen, Registration_eVBNumberBox.Text,
                                Registration_GeneralInspectionDateBox.SelectedDate, newZulassungsDatum, RegDocNumBox.Text, EmissionsCodeBox.Text, dbContext);
                            price = findPrice(ProduktId);
                            if (price == null)
                            {
                                ErrorLeereTextBoxenLabel.Text = "Kein Price gefunden!";
                                ErrorLeereTextBoxenLabel.Visible = true;
                                return;
                            }
                            newKennzeichenRegOrder = RegistrationOrder.CreateRegistrationOrder(Int32.Parse(Session["CurrentUserId"].ToString()),
                                Int32.Parse(CustomerDropDownList.SelectedValue), kennzeichen, oldKennzeichen, Registration_eVBNumberBox.Text, newVehicle, newRegistration,
                                RegistrationOrderTypes.Renumbering, locationId, Int32.Parse(ZulassungsstelleComboBox.SelectedValue), dbContext);
                            newKennzeichenRegOrder.Order.FreeText = FreiTextBox.Text;

                            newOrderItem1 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.Amount, 1, costCenter, null, false, dbContext);
                            if (price.AuthorativeCharge.HasValue)
                            {
                                newOrderItem2 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.AuthorativeCharge.Value, 1, costCenter, newOrderItem1.Id, true, dbContext);
                            }
                            dbContext.SubmitChanges();
                            if (DienstleistungTreeView.Nodes.Count > 1)
                            {
                                bool inOrdnung = AddAnotherProducts(newKennzeichenRegOrder, locationId);
                            }
                            if (String.IsNullOrEmpty(VehicleIdField.Value)) //update CurrentRegistration Id
                            {
                                newVehicle.CurrentRegistrationId = newRegistration.Id;
                                dbContext.SubmitChanges();
                            }
                            //ZulassungOkLabel.Visible = true;
                            if (((RadButton)(sender)).ID != "rbtSameOrder")
                                MakeAllControlsEmpty();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "finishedMessage", "alert('Auftrag wurde erfolgreich angelegt.');", true);
                        }
                        else
                        {
                            FahrzeugLabel.Text = "Für die Umkennzeichnung mind. neues Kennzeichen erforderlich!";
                            FahrzeugLabel.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else if (RegistrationOrderDropDownList.Text.Contains("Wiederzulassung")) // Wiederzulassung
                    {
                        if (!String.IsNullOrEmpty(VehicleIdField.Value)) //falls auto gefunden wurde
                        {
                            newVehicle = dbContext.Vehicle.SingleOrDefault(q => q.Id == Int32.Parse(VehicleIdField.Value));
                        }
                        else // neues Auto muss angelegt werden
                        {
                            newVehicle = Vehicle.CreateVehicle(VINBox.Text, HSNBox.Text, TSNBox.Text, Vehicle_VariantBox.Text, FirstRegistrationDate, color, dbContext);
                        }
                        // another logic after new/existing Vehicle
                        newAdress = Adress.CreateAdress(Adress_StreetBox.Text, Adress_StreetNumberBox.Text, Adress_ZipcodeBox.Text, Adress_CityBox.Text, Adress_CountryBox.Text, dbContext);
                        newContact = Contact.CreateContact(Contact_PhoneBox.Text, Contact_FaxBox.Text, Contact_MobilePhoneBox.Text, Contact_EmailBox.Text, dbContext);
                        newBankAccount = BankAccount.CreateBankAccount(dbContext, BankAccount_BankNameBox.Text, BankAccount_AccountnumberBox.Text,
                            BankAccount_BankCodeBox.Text, txbBancAccountIban.Text, txbBancAccountBIC.Text);
                        newCarOwner = CarOwner.CreateCarOwner(CarOwner_NameBox.Text, CarOwner_FirstnameBox.Text, newBankAccount, newContact, newAdress, dbContext);
                        DateTime newZulassungsDatum = DateTime.Now;
                        if (ZulassungsdatumPicker.SelectedDate != null)
                        {
                            if (!string.IsNullOrEmpty(ZulassungsdatumPicker.SelectedDate.ToString()))
                            {
                                newZulassungsDatum = (DateTime)ZulassungsdatumPicker.SelectedDate;
                            }
                        }
                        newRegistration = Registration.CreateRegistration(newCarOwner, newVehicle, kennzeichen, Registration_eVBNumberBox.Text,
                            Registration_GeneralInspectionDateBox.SelectedDate, newZulassungsDatum, RegDocNumBox.Text, EmissionsCodeBox.Text, dbContext);
                        price = findPrice(ProduktId);
                        if (price == null)
                        {
                            ErrorLeereTextBoxenLabel.Text = "Keinen Preis gefunden!";
                            ErrorLeereTextBoxenLabel.Visible = true;
                            return;
                        }
                        newKennzeichenRegOrder = RegistrationOrder.CreateRegistrationOrder(Int32.Parse(Session["CurrentUserId"].ToString()),
                            Int32.Parse(CustomerDropDownList.SelectedValue), kennzeichen, oldKennzeichen, Registration_eVBNumberBox.Text, newVehicle, newRegistration,
                            RegistrationOrderTypes.Readmission, locationId, Int32.Parse(ZulassungsstelleComboBox.SelectedValue), dbContext);
                        if (!String.IsNullOrEmpty(FreiTextBox.Text))
                        {
                            newKennzeichenRegOrder.Order.FreeText = FreiTextBox.Text;
                        }
                        newOrderItem1 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.Amount, 1, costCenter, null, false, dbContext);
                        if (price.AuthorativeCharge.HasValue)
                        {
                            newOrderItem2 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.AuthorativeCharge.Value, 1, costCenter, newOrderItem1.Id, true, dbContext);
                        }
                        dbContext.SubmitChanges();
                        if (DienstleistungTreeView.Nodes.Count > 1)
                        {
                            bool inOrdnung = AddAnotherProducts(newKennzeichenRegOrder, locationId);
                        }
                        if (String.IsNullOrEmpty(VehicleIdField.Value)) //update CurrentRegistration Id
                        {
                            newVehicle.CurrentRegistrationId = newRegistration.Id;
                            dbContext.SubmitChanges();
                        }
                        // ZulassungOkLabel.Visible = true;
                        if (((RadButton)(sender)).ID != "rbtSameOrder")
                            MakeAllControlsEmpty();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "finishedMessage", "alert('Auftrag wurde erfolgreich angelegt.');", true);
                    }
                    else // Neuzulassung
                    {
                        if (!String.IsNullOrEmpty(VehicleIdField.Value)) //falls auto gefunden wurde
                        {
                            newVehicle = dbContext.Vehicle.SingleOrDefault(q => q.Id == Int32.Parse(VehicleIdField.Value));
                        }
                        else // neues Auto muss angelegt werden
                        {
                            newVehicle = Vehicle.CreateVehicle(VINBox.Text, HSNBox.Text, TSNBox.Text, Vehicle_VariantBox.Text, FirstRegistrationDate, color, dbContext);
                        }
                        newAdress = Adress.CreateAdress(Adress_StreetBox.Text, Adress_StreetNumberBox.Text, Adress_ZipcodeBox.Text, Adress_CityBox.Text, Adress_CountryBox.Text, dbContext);
                        newContact = Contact.CreateContact(Contact_PhoneBox.Text, Contact_FaxBox.Text, Contact_MobilePhoneBox.Text, Contact_EmailBox.Text, dbContext);
                        newBankAccount = BankAccount.CreateBankAccount(dbContext, BankAccount_BankNameBox.Text, BankAccount_AccountnumberBox.Text, BankAccount_BankCodeBox.Text,
                            txbBancAccountIban.Text, txbBancAccountBIC.Text);
                        newCarOwner = CarOwner.CreateCarOwner(CarOwner_NameBox.Text, CarOwner_FirstnameBox.Text, newBankAccount, newContact, newAdress, dbContext);
                        DateTime newZulassungsDatum = DateTime.Now;
                        if (ZulassungsdatumPicker.SelectedDate != null)
                        {
                            if (!string.IsNullOrEmpty(ZulassungsdatumPicker.SelectedDate.ToString()))
                            {
                                newZulassungsDatum = (DateTime)ZulassungsdatumPicker.SelectedDate;
                            }
                        }
                        newRegistration = Registration.CreateRegistration(newCarOwner, newVehicle, kennzeichen, Registration_eVBNumberBox.Text,
                            Registration_GeneralInspectionDateBox.SelectedDate, newZulassungsDatum, RegDocNumBox.Text, EmissionsCodeBox.Text, dbContext);
                        price = findPrice(ProduktId);
                        if (price == null)
                        {
                            ErrorLeereTextBoxenLabel.Text = "Kein Price gefunden!";
                            ErrorLeereTextBoxenLabel.Visible = true;
                            return;
                        }
                        newKennzeichenRegOrder = RegistrationOrder.CreateRegistrationOrder(Int32.Parse(Session["CurrentUserId"].ToString()),
                            Int32.Parse(CustomerDropDownList.SelectedValue), kennzeichen, oldKennzeichen, Registration_eVBNumberBox.Text, newVehicle, newRegistration,
                            RegistrationOrderTypes.NewAdmission, locationId, Int32.Parse(ZulassungsstelleComboBox.SelectedValue), dbContext);
                        if (!String.IsNullOrEmpty(FreiTextBox.Text))
                        {
                            newKennzeichenRegOrder.Order.FreeText = FreiTextBox.Text;
                        }
                        newOrderItem1 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.Amount, 1, costCenter, null, false, dbContext);
                        if (price.AuthorativeCharge.HasValue)
                        {
                            newOrderItem2 = newKennzeichenRegOrder.Order.AddOrderItem(Int32.Parse(ProduktId), price.AuthorativeCharge.Value, 1, costCenter, newOrderItem1.Id, true, dbContext);
                        }
                        dbContext.SubmitChanges();
                        if (DienstleistungTreeView.Nodes.Count > 1)
                        {
                            bool inOrdnung = AddAnotherProducts(newKennzeichenRegOrder, locationId);
                        }
                        newVehicle.CurrentRegistrationId = newRegistration.Id;
                        dbContext.SubmitChanges();
                        if (((RadButton)(sender)).ID != "rbtSameOrder")
                            MakeAllControlsEmpty();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "finishedMessage", "alert('Auftrag wurde erfolgreich angelegt.');", true);
                    }
                    VehicleIdField.Value = "";
                }
                catch (Exception ex)
                {
                    SubmitChangesErrorLabel.Visible = true;
                    SubmitChangesErrorLabel.Text = ex.Message;
                }
            }
        }
        protected void genIban_Click(object sender, EventArgs e)
        {
            if (BankAccount_AccountnumberBox.Text != string.Empty && BankAccount_BankCodeBox.Text != string.Empty
                && EmptyStringIfNull.IsNumber(BankAccount_AccountnumberBox.Text) && !String.IsNullOrEmpty(BankAccount_BankNameBox.Text) && EmptyStringIfNull.IsNumber(BankAccount_BankCodeBox.Text))
            {
                txbBancAccountIban.Text = "DE" + (98 - ((62 * ((1 + long.Parse(BankAccount_BankCodeBox.Text) % 97)) +
                    27 * (long.Parse(BankAccount_AccountnumberBox.Text) % 97)) % 97)).ToString("D2");
                txbBancAccountIban.Text += long.Parse(BankAccount_BankCodeBox.Text).ToString("00000000").Substring(0, 4);
                txbBancAccountIban.Text += long.Parse(BankAccount_BankCodeBox.Text).ToString("00000000").Substring(4, 4);
                txbBancAccountIban.Text += long.Parse(BankAccount_AccountnumberBox.Text).ToString("0000000000").Substring(0, 4);
                txbBancAccountIban.Text += long.Parse(BankAccount_AccountnumberBox.Text).ToString("0000000000").Substring(4, 4);
                txbBancAccountIban.Text += long.Parse(BankAccount_AccountnumberBox.Text).ToString("0000000000").Substring(8, 2);
                using (DataClasses1DataContext dataContext = new DataClasses1DataContext())
                {
                    var bicNr = dataContext.BIC_DE.FirstOrDefault(q => q.Bankleitzahl.Contains(BankAccount_BankCodeBox.Text) && (q.Bezeichnung.Contains(BankAccount_BankNameBox.Text) ||
                        q.Kurzbezeichnung.Contains(BankAccount_BankNameBox.Text)));
                    if (bicNr != null)
                    {
                        if (!String.IsNullOrEmpty(bicNr.BIC.ToString()))
                            txbBancAccountBIC.Text = bicNr.BIC.ToString();
                    }
                }
            }
        }
        #endregion
        protected string CheckIfAllProduktsHavingPrice(int? locationId)
        {
            string allesHatGutGelaufen = "";
            string ProduktId = "";
            string CostCenterId = "";

            foreach (RadTreeNode node in DienstleistungTreeView.Nodes)
            {
                if (!String.IsNullOrEmpty(node.Value))
                {
                    string[] splited = node.Value.Split(';');
                    if (splited.Length == 2)
                    {
                        try
                        {
                            DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                            Price newPrice;
                            ProduktId = splited[0];
                            CostCenterId = splited[1];
                            if (!String.IsNullOrEmpty(ProduktId))
                            {
                                var productId = Int32.Parse(ProduktId);
 
                                KVSCommon.Database.Product newProduct = dbContext.Product.SingleOrDefault(q => q.Id == productId);
                                if (locationId == null) //small
                                {
                                    newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == null);
                                    if (newPrice == null)
                                    {
                                        allesHatGutGelaufen += " " + node.Text + " ";
                                    }
                                    else
                                    {
                                    }
                                }
                                else //large
                                {
                                    var costCenterId = Int32.Parse(CostCenterId);
                                    newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == locationId);
                                    if (newPrice == null)
                                        newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == null);
                                    if (newPrice == null)
                                    {
                                        allesHatGutGelaufen += " " + node.Text + " ";
                                    }
                                    else
                                    {
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
            return allesHatGutGelaufen;
        }
        protected bool AddAnotherProducts(RegistrationOrder regOrd, int? locationId)
        {
            bool allesHatGutGelaufen = false;
            string ProduktId = "";
            string CostCenterId = "";
            int skipFirst = 0;
            foreach (RadTreeNode node in DienstleistungTreeView.Nodes)
            {
                if (skipFirst > 0)
                {
                    if (!String.IsNullOrEmpty(node.Value))
                    {
                        string[] splited = node.Value.Split(';');
                        if (splited.Length == 2)
                        {
                            try
                            {
                                DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                                var orderNumber = regOrd.OrderNumber;
                                Price newPrice;
                                OrderItem newOrderItem1 = null;
                                OrderItem newOrderItem2 = null;
                                ProduktId = splited[0];
                                CostCenterId = splited[1];
                                if (!String.IsNullOrEmpty(ProduktId))
                                {
                                    var productId = Int32.Parse(ProduktId);
                                    int? costCenterId = null;
                                    KVSCommon.Database.Product newProduct = dbContext.Product.SingleOrDefault(q => q.Id == productId);
                                    if (locationId == null) //small
                                    {
                                        newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == null);
                                    }
                                    else //large
                                    {
                                        costCenterId = Int32.Parse(CostCenterId);
                                        newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == locationId);
                                        if (newPrice == null)
                                            newPrice = dbContext.Price.SingleOrDefault(q => q.ProductId == newProduct.Id && q.LocationId == null);
                                    }
                                    var orderToUpdate = dbContext.Order.SingleOrDefault(q => q.OrderNumber == orderNumber);
                                    orderToUpdate.LogDBContext = dbContext;
                                    if (orderToUpdate != null)
                                    {
                                        CostCenter costCenter = null;
                                        if (costCenterId.HasValue)
                                        {
                                            costCenter = dbContext.CostCenter.FirstOrDefault(o => o.Id == costCenterId.Value);
                                        }

                                        newOrderItem1 = orderToUpdate.AddOrderItem(newProduct.Id, newPrice.Amount, 1, costCenter, null, false, dbContext);
                                        if (newPrice.AuthorativeCharge.HasValue)
                                        {
                                            orderToUpdate.AddOrderItem(newProduct.Id, newPrice.AuthorativeCharge.Value, 1, costCenter, newOrderItem1.Id,
                                                newPrice.AuthorativeCharge.HasValue, dbContext);
                                        }
                                        dbContext.SubmitChanges();
                                        allesHatGutGelaufen = true;
                                    }
                                }
                                if (allesHatGutGelaufen)
                                    dbContext.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    skipFirst = 1;
                }
            }
            return allesHatGutGelaufen;
        }
        protected void MakeInvoiceForSmallCustomer(int customerId, RegistrationOrder regOrder)
        {
            try
            {
                DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString()));
                var newOrder = dbContext.Order.Single(q => q.CustomerId == customerId && q.OrderNumber == regOrder.OrderNumber);
                smallCustomerOrderHiddenField.Value = regOrder.OrderNumber.ToString();
                //updating order status
                newOrder.LogDBContext = dbContext;
                newOrder.Status = (int)OrderStatusTypes.Closed;
                //updating orderitems status                          
                foreach (OrderItem ordItem in newOrder.OrderItem)
                {
                    ordItem.LogDBContext = dbContext;
                    if (ordItem.Status != (int)OrderItemStatusTypes.Cancelled)
                    {
                        ordItem.Status = (int)OrderItemStatusTypes.Closed;
                    }
                }
                dbContext.SubmitChanges();
                //updating order und items status one more time to make it abgerechnet
                newOrder.LogDBContext = dbContext;
                newOrder.ExecutionDate = DateTime.Now;
                newOrder.Status = (int)OrderStatusTypes.Payed;
                dbContext.SubmitChanges();
                //opening window for adress
                string script = "function f(){$find(\"" + AddAdressRadWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                SetValuesForAdressWindow();
            }
            catch (Exception ex)
            {
                ErrorLeereTextBoxenLabel.Text = "Error: " + ex.Message;
                ErrorLeereTextBoxenLabel.Visible = true;
            }
        }
        // getting adress from small customer
        protected void SetValuesForAdressWindow()
        {
            DataClasses1DataContext dbContext = new DataClasses1DataContext();
            var locationQuery = (from adr in dbContext.Adress
                                 join cust in dbContext.Customer on adr.Id equals cust.InvoiceAdressId
                                 where cust.Id == Int32.Parse(CustomerDropDownList.SelectedValue)
                                 select adr).SingleOrDefault();
            if (locationQuery != null)
            {
                StreetTextBox.Text = locationQuery.Street;
                StreetNumberTextBox.Text = locationQuery.StreetNumber;
                ZipcodeTextBox.Text = locationQuery.Zipcode;
                CityTextBox.Text = locationQuery.City;
                CountryTextBox.Text = locationQuery.Country;
                LocationLabelWindow.Text = "Fügen Sie bitte die Adresse für " + CustomerDropDownList.Text + " hinzu";
                ZusatzlicheInfoLabel.Visible = false;
                //InvoiceRecValidator2.Enabled = true;
                if (CustomerDropDownList.SelectedIndex == 1) // small
                {
                    ZusatzlicheInfoLabel.Visible = true;
                }
            }
        }
        // Create new Adress in der DatenBank
        protected void OnAddAdressButton_Clicked(object sender, EventArgs e)
        {
            //InvoiceRecValidator2.Enabled = false;
            //Adress Eigenschaften
            string street = "",
                streetNumber = "",
                zipcode = "",
                city = "",
                country = "",
                invoiceRecipient = "";
            // OrderItem Eigenschaften
            string ProductName = "";
            decimal Amount = 0;

            street = StreetTextBox.Text;
            streetNumber = StreetNumberTextBox.Text;
            zipcode = ZipcodeTextBox.Text;
            city = CityTextBox.Text;
            country = CountryTextBox.Text;
            invoiceRecipient = InvoiceRecipient.Text;
            int itemCount = 0;
            try
            {
                using (DataClasses1DataContext dbContext = new DataClasses1DataContext(Int32.Parse(Session["CurrentUserId"].ToString())))
                {
                    var newAdress = Adress.CreateAdress(street, streetNumber, zipcode, city, country, dbContext);
                    var newInvoice = Invoice.CreateInvoice(dbContext, Int32.Parse(Session["CurrentUserId"].ToString()), invoiceRecipient, newAdress,
                        Int32.Parse(CustomerDropDownList.SelectedValue), txbDiscount.Value, "Einzelrechnung");
                    //Submiting new Invoice and Adress
                    dbContext.SubmitChanges();
                    var orderQuery = dbContext.Order.SingleOrDefault(q => q.OrderNumber == Int32.Parse(smallCustomerOrderHiddenField.Value));
                    foreach (OrderItem ordItem in orderQuery.OrderItem)
                    {
                        ProductName = ordItem.ProductName;
                        Amount = ordItem.Amount;
                                               
                        CostCenter costCenter = null;
                        if (ordItem.CostCenterId.HasValue)
                        {
                            costCenter = dbContext.CostCenter.FirstOrDefault(o => o.Id == ordItem.CostCenterId.Value);
                        }

                        itemCount = ordItem.Count;
                        InvoiceItem newInvoiceItem = newInvoice.AddInvoiceItem(ProductName, Convert.ToDecimal(Amount), itemCount, ordItem, costCenter, dbContext);
                        ordItem.LogDBContext = dbContext;
                        ordItem.Status = (int)OrderItemStatusTypes.Payed;
                        dbContext.SubmitChanges();
                    }
                    // Submiting new InvoiceItems
                    dbContext.SubmitChanges();
                    Print(newInvoice, dbContext);
                    // Closing RadWindow
                }
            }
            catch (Exception ex)
            {
                ErrorLeereTextBoxenLabel.Text = ex.Message;
                ErrorLeereTextBoxenLabel.Visible = true;
            }
        }
        protected string CheckIfFolderExistsAndReturnPathForPdf()
        {
            string newPdfPathAndName = "";
            if (!Directory.Exists(ConfigurationManager.AppSettings["DataPath"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DataPath"]);
            }

            if (!Directory.Exists(ConfigurationManager.AppSettings["DataPath"] + "/" + Session["CurrentUserId"].ToString()))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DataPath"] + "/" + Session["CurrentUserId"].ToString());
            }
            newPdfPathAndName = ConfigurationManager.AppSettings["DataPath"] + "/" + Session["CurrentUserId"].ToString() + "/Lieferschein_" + DateTime.Today.Day + "_" +
                DateTime.Today.Month + "_" + DateTime.Today.Year + "_" + Guid.NewGuid() + ".pdf";
            return newPdfPathAndName;
        }
        private void OpenPrintfile(string myFile)
        {
            string url = ConfigurationManager.AppSettings["BaseUrl"];
            string path = url + "UserData/" + Session["CurrentUserId"].ToString() + "/" + myFile;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Invoice", "<script>openFile('" + path + "');</script>", false);
        }
        protected void Print(Invoice newInvoice, DataClasses1DataContext dbContext)
        {
            using (MemoryStream memS = new MemoryStream())
            {
                InvoiceHelper.CreateAccounts(dbContext, newInvoice);
                newInvoice.Print(dbContext, memS, "");
                dbContext.SubmitChanges();
                string fileName = "Rechnung_" + newInvoice.InvoiceNumber.Number + "_" + newInvoice.CreateDate.Day + "_" + newInvoice.CreateDate.Month + "_" + newInvoice.CreateDate.Year + ".pdf";
                string serverPath = ConfigurationManager.AppSettings["DataPath"] + "\\UserData";
                if (!Directory.Exists(serverPath)) Directory.CreateDirectory(serverPath);
                if (!Directory.Exists(serverPath + "\\" + Session["CurrentUserId"].ToString())) Directory.CreateDirectory(serverPath + "\\" + Session["CurrentUserId"].ToString());
                serverPath = serverPath + "\\" + Session["CurrentUserId"].ToString();
                File.WriteAllBytes(serverPath + "\\" + fileName, memS.ToArray());
                OpenPrintfile(fileName);
                dbContext.SubmitChanges();
            }
        }
        //Tausch die Information aus neues zu altes Kennzeichen Boxen
        protected void KennzeichenTauschButton_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PreviousLicenceBox1.Text))
            {
                PreviousLicenceBox1.Text = LicenceBox1.Text;
                PreviousLicenceBox2.Text = LicenceBox2.Text;
                PreviousLicenceBox3.Text = LicenceBox3.Text;
                LicenceBox1.Text = "";
                LicenceBox2.Text = "";
                LicenceBox3.Text = "";
            }
            else if (String.IsNullOrEmpty(LicenceBox1.Text))
            {
                LicenceBox1.Text = PreviousLicenceBox1.Text;
                LicenceBox2.Text = PreviousLicenceBox2.Text;
                LicenceBox3.Text = PreviousLicenceBox3.Text;
                PreviousLicenceBox1.Text = "";
                PreviousLicenceBox2.Text = "";
                PreviousLicenceBox3.Text = "";
            }
        }
        // findet alle angezeigte textboxen und überprüft ob die nicht leer sind
        protected bool CheckIfBoxenNotEmpty()
        {
            bool gibtsBoxenDieLeerSind = false;
            bool iFound1VisibleBox = false;
            bool carOwnerMin = false;
            int count = 0;
            List<Control> allControls = getAllControls();
            //fallse leer - soll aus der Logik rausgenommen
            if (String.IsNullOrEmpty(PruefzifferBox.Text))
                PruefzifferBox.Enabled = false;
            if (String.IsNullOrEmpty(RegistrationOrderDropDownList.SelectedValue) || String.IsNullOrEmpty(ZulassungsstelleComboBox.SelectedValue) ||
                String.IsNullOrEmpty(CostCenterDropDownList.SelectedValue) || String.IsNullOrEmpty(LocationDropDownList.SelectedValue) || DienstleistungTreeView.Nodes.Count == 0)
            {
                return true;
            }

            foreach (Control control in allControls)
            {
                if (control.Visible == true)
                {
                    iFound1VisibleBox = true;
                    foreach (Control subControl in control.Controls)
                    {
                        if (subControl is RadTextBox)
                        {
                            RadTextBox box = subControl as RadTextBox;
                            if (box.ID == "CarOwner_NameBox" || box.ID == "CarOwner_FirstnameBox")
                            {

                                if (box.Text != string.Empty)
                                {
                                    carOwnerMin = true;
                                }
                                else if (!carOwnerMin)
                                {
                                    count++;
                                }
                                else
                                {
                                    box.BorderColor = System.Drawing.Color.Black;
                                }
                                if (count > 1)
                                {
                                    box.BorderColor = System.Drawing.Color.Red;
                                    gibtsBoxenDieLeerSind = true;

                                }
                                continue;
                            }

                            else if (box.Enabled == true && String.IsNullOrEmpty(box.Text))
                            {
                                box.BorderColor = System.Drawing.Color.Red;
                                gibtsBoxenDieLeerSind = true;
                            }
                            else
                            {
                                box.BorderColor = System.Drawing.Color.Black;
                            }
                        }
                    }
                }
            }
            if (!PruefzifferBox.Enabled)
                PruefzifferBox.Enabled = true;
            if (iFound1VisibleBox == false)
                gibtsBoxenDieLeerSind = true;
            return gibtsBoxenDieLeerSind;
        }
        // findet alle textboxen und macht die leer ohne die ganze Seite neu zu laden
        protected void MakeAllControlsEmpty()
        {
            List<Control> allControls = getAllControls();
            DateTime? nullDate = null;
            Registration_GeneralInspectionDateBox.SelectedDate = nullDate;
            FirstRegistrationDateBox.SelectedDate = DateTime.Now;
            DienstleistungTreeView.Nodes.Clear();
            CustomerDropDownList.ClearSelection();
            ProductDropDownList.ClearSelection();
            LocationDropDownList.ClearSelection();
            CostCenterDropDownList.ClearSelection();
            RegistrationOrderDropDownList.ClearSelection();
            ZulassungsstelleComboBox.ClearSelection();
            foreach (Control control in allControls)
            {
                foreach (Control subControl in control.Controls)
                {
                    if (subControl is RadTextBox)
                    {
                        RadTextBox box = subControl as RadTextBox;
                        if (box.Enabled == true)
                        {
                            box.Text = "";
                        }
                    }
                }
            }
        }
        protected void NaechtenAuftragButton_Clicked(object sender, EventArgs e)
        {
            MakeAllControlsEmpty();
            ZulassungOkLabel.Visible = false;
            SubmitChangesErrorLabel.Visible = false;
        }
        protected void CheckFields(List<Control> listOfControls)
        {
            HideAllControls();
            DataClasses1DataContext con = new DataClasses1DataContext();
            var cont = from largCust in con.LargeCustomerRequiredField
                       join reqFiled in con.RequiredField on largCust.RequiredFieldId equals reqFiled.Id
                       join ordTyp in con.OrderType on reqFiled.OrderTypeId equals ordTyp.Id
                       where largCust.LargeCustomerId == Int32.Parse(CustomerDropDownList.SelectedValue) && ordTyp.Id == (int)OrderTypes.Admission
                       select reqFiled.Name;
            foreach (var nameCon in cont)
            {
                foreach (Control control in listOfControls)
                {
                    if (control.ID == nameCon)
                    {
                        control.Visible = true;
                        if (control.ID == "Vehicle_VIN" || control.ID == "Vehicle_Variant" || control.ID == "Vehicle_Color" || control.ID == "Registration_Licencenumber" ||
                            control.ID == "RegistrationOrder_PreviousLicencenumber" || control.ID == "Registration_GeneralInspectionDate" ||
                            control.ID == "Vehicle_FirstRegistrationDate" || control.ID == "Vehicle_TSN" || control.ID == "Vehicle_HSN" || control.ID == "Registration_eVBNumber" ||
                            control.ID == "Registration_RegistrationDocumentNumber" || control.ID == "Registration_EmissionCode" || control.ID == "Order_Freitext")
                            FahrzeugLabel.Visible = true;
                        if (control.ID == "CarOwner_Name" || control.ID == "CarOwner_Firstname" || control.ID == "Adress_Street" || control.ID == "Adress_StreetNumber" ||
                            control.ID == "Adress_Zipcode" || control.ID == "Adress_City" || control.ID == "Adress_Country")
                            HalterLabel.Visible = true;
                        if (control.ID == "BankAccount_BankName" || control.ID == "BankAccount_Accountnumber" || control.ID == "BankAccount_BankCode")
                        {
                            HalterdatenLabel.Visible = true;
                            IBANPanel.Visible = true;
                        }
                        if (control.ID == "Contact_Phone" || control.ID == "Contact_Fax" || control.ID == "Contact_MobilePhone" || control.ID == "Contact_Email")
                            KontaktdatenLabel.Visible = true;
                    }
                }
            }
        }
        protected void setCarOwnerData()
        {
            using (DataClasses1DataContext con = new DataClasses1DataContext())
            {
                Adress_StreetBox.Text = string.Empty;
                Adress_StreetNumberBox.Text = string.Empty;
                Adress_ZipcodeBox.Text = string.Empty;
                Adress_CityBox.Text = string.Empty;
                Adress_CountryBox.Text = string.Empty;
                CarOwner_NameBox.Text = string.Empty;
                Registration_eVBNumberBox.Text = string.Empty;
                if (!String.IsNullOrEmpty(LocationDropDownList.SelectedValue) && !String.IsNullOrEmpty(CustomerDropDownList.SelectedValue))
                {
                    var customerDetils = con.Location.FirstOrDefault(q => q.Id == Int32.Parse(LocationDropDownList.SelectedValue) && 
                        q.CustomerId == Int32.Parse(CustomerDropDownList.SelectedValue));
                    if (customerDetils != null)
                    {
                        CarOwner_NameBox.Text = customerDetils.LargeCustomer.Customer.Name;
                        if (customerDetils.LargeCustomer.Person != null)
                            CarOwner_FirstnameBox.Text = customerDetils.LargeCustomer.Person.Name + " " + customerDetils.LargeCustomer.Person.FirstName;
                        Registration_eVBNumberBox.Text = customerDetils.LargeCustomer.Customer.eVB_Number;
                        if (customerDetils.Adress != null)
                        {
                            Adress_StreetBox.Text = customerDetils.Adress.Street;
                            Adress_StreetNumberBox.Text = customerDetils.Adress.StreetNumber;
                            Adress_ZipcodeBox.Text = customerDetils.Adress.Zipcode;
                            Adress_CityBox.Text = customerDetils.Adress.City;
                            Adress_CountryBox.Text = customerDetils.Adress.Country;
                        }
                    }
                }
            }
        }
        protected void HideAllControls()
        {
            List<Control> controlsToHide = new List<Control>();
            controlsToHide = getAllControls();
            DataClasses1DataContext con = new DataClasses1DataContext();
            var cont = from largCust in con.LargeCustomerRequiredField
                       join reqFiled in con.RequiredField on largCust.RequiredFieldId equals reqFiled.Id
                       join ordTyp in con.OrderType on reqFiled.OrderTypeId equals ordTyp.Id
                       where ordTyp.Id == (int)OrderTypes.Admission
                       select reqFiled.Name;
            foreach (var nameCon in cont)
            {
                foreach (Control control in controlsToHide)
                {
                    if (control.ID == nameCon)
                    {
                        control.Visible = false;
                        FahrzeugLabel.Visible = false;
                        HalterLabel.Visible = false;
                        HalterdatenLabel.Visible = false;
                        IBANPanel.Visible = false;
                        KontaktdatenLabel.Visible = false;
                    }
                }
            }
        }
        //Gibt die Liste mit alle Controls aus der ASCX Seite zurück
        protected List<Control> getAllControls()
        {
            if (controls.Count == 0)
            {
                controls.Add(Vehicle_Variant);
                controls.Add(Registration_GeneralInspectionDate);
                controls.Add(CarOwner_Name);
                controls.Add(CarOwner_Firstname);
                controls.Add(Adress_StreetNumber);
                controls.Add(Adress_Street);
                controls.Add(Adress_Zipcode);
                controls.Add(Adress_City);
                controls.Add(Adress_Country);
                controls.Add(Contact_Phone);
                controls.Add(Contact_Fax);
                controls.Add(Contact_MobilePhone);
                controls.Add(Contact_Email);
                controls.Add(BankAccount_BankName);
                controls.Add(BankAccount_Accountnumber);
                controls.Add(BankAccount_BankCode);
                controls.Add(Registration_eVBNumber);
                controls.Add(Vehicle_HSN);
                controls.Add(Vehicle_TSN);
                controls.Add(Vehicle_VIN);
                controls.Add(Registration_Licencenumber);
                controls.Add(RegistrationOrder_PreviousLicencenumber);
                controls.Add(Registration_EmissionCode);
                controls.Add(Registration_RegistrationDocumentNumber);
                controls.Add(Vehicle_FirstRegistrationDate);
                controls.Add(Vehicle_Color);
                controls.Add(IBANPanel);
            }
            return controls;
        }
    }
}