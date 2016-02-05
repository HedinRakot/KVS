﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZulassungLaufkundeControl.ascx.cs" Inherits="KVSWebApplication.Auftragseingang.ZulassungLaufkundeControl" %>
<%@ Register TagPrefix="smc" TagName="Registration" Src="../Auftragsbearbeitung_Neuzulassung/Zulassungsstelle.ascx" %>
<script type="text/javascript">
    function MyValueChanging(sender, args) {
        args.set_newValue(args.get_newValue().toUpperCase());
    }
    $(document).keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            return false;
        }
    });
    function MyValueChanging23(sender, args) {
        args.set_newValue(args.get_newValue().toUpperCase());
        var textLength = args.get_newValue().length;
        if (textLength != 8 && textLength != 17) {
            sender.focus();
            alert("Bitte FIN 17 oder 8 stellig eingeben! Jetzt: " + textLength);
        }
    }
    function openFile(path) {
        window.open(path, "_blank", "left=0,top=0,scrollbars=yes,menubar=yes,toolbar=yes,resizable=yes");
    }
    function MyFirstValueChanging(sender, args) {
        args.set_newValue(args.get_newValue().charAt(0).toUpperCase() + args.get_newValue().slice(1));
    }
    function isnumber(val) {
        var replaced = val.value.replace(',', '.');
        if (!$.isNumeric(replaced)) {
            val.value = '';
        }
    }
    var itemIndex = 0;
    function getMessage(message) {
        alert(message);
    }
    function checkFields() {
        var ZulassungsstelleComboBox = $find('<%=ZulassungsstelleComboBox.ClientID %>');
        var RegistrationOrderDropDownList = $find('<%=RegistrationOrderDropDownList.ClientID %>');
        var CustomerDropDownList = $find('<%=CustomerDropDownList.ClientID %>');
        var treeView = $find('<%= DienstleistungTreeView.ClientID %>');
        var LicenceBox1 = $find('<%=LicenceBox1.ClientID %>');
        var LicenceBox2 = $find('<%=LicenceBox2.ClientID %>');
        var LicenceBox3 = $find('<%=LicenceBox3.ClientID %>');
        var createButton = $find('<%=AuftragZulassenButton.ClientID %>');
        var PreviousLicenceBox1 = $find('<%=PreviousLicenceBox1.ClientID %>');
        var PreviousLicenceBox2 = $find('<%=PreviousLicenceBox2.ClientID %>');
        var PreviousLicenceBox3 = $find('<%=PreviousLicenceBox3.ClientID %>');
        var kennzeichen = LicenceBox1.get_value() + LicenceBox2.get_value() + LicenceBox3.get_value();
        var firstname = $find('<%=txbSmallCustomerVorname.ClientID %>');
        var lastName = $find('<%=txbSmallCustomerNachname.ClientID %>');
        var VINBox = $find('<%=VINBox.ClientID %>');
        var allNodes = treeView.get_allNodes();
        if (allNodes.length < 1) {
            alert("Sie haben keine Dienstleistungen hinzugefügt!");
            return false;
        }
        if (VINBox.get_value() == '') {
            //getMessage('Bitte geben Sie die FIN ein');
            //return false;
        }
        if (ZulassungsstelleComboBox.get_selectedIndex() == null) {
            getMessage('Bitte wählen Sie die Zulassungsstelle aus');
            return false;
        }
        if (RegistrationOrderDropDownList.get_selectedIndex() == null) {
            getMessage('Bitte wählen Sie die Zulassungsart aus');
            return false;
        }
        if (CustomerDropDownList.get_selectedIndex() == null && (firstname.get_value() == '' && lastName.get_value() == '')) {
            getMessage('Bitte wählen Sie den Kunden aus');
            return false;
        }
        if (RegistrationOrderDropDownList.get_text() == 'Umkennzeichnung' && kennzeichen == '') {
            getMessage('Für die Umkennzeichnung ist  mind. das neue Kennzeichen erforderlich!');
            return false;
        }
        __doPostBack('createButton', "CreateOrder");
    }
    function addNodeZul() {
        var treeView = $find('<%= DienstleistungTreeView.ClientID %>');
        var prodDropDown = $find('<%=ProductDropDownList.ClientID %>');
        var selectedIndex = prodDropDown.get_selectedIndex();
        var myItemPrice = $('#MainContentPlaceHolder_SmallCustomerZulassung_ProductDropDownList_i' + selectedIndex + '_myPrice');
        var myItemCharge = $('#MainContentPlaceHolder_SmallCustomerZulassung_ProductDropDownList_i' + selectedIndex + '_myAuthCharge');
        var acText = myItemCharge.text();
        var nodeProd = prodDropDown.get_value();
        var nodeProdText = prodDropDown.get_text();
        if (!nodeProd) {
            alert("Bitte wählen Sie eine Dienstleistung aus!");
            return false;
        }
        treeView.trackChanges();
        node = new Telerik.Web.UI.RadTreeNode();
        node.set_text(nodeProdText);
        node.set_value(nodeProd + ";");
        var amtGebEnabled = (acText != '') ? 'visibility:visible;' : 'visibility:hidden;';
        node.set_clientTemplate("<div style='width:240px'>#= Text #</div> Dienstleistungspreis: <input type='text' onkeyup='isnumber(this)' value='" + myItemPrice.text() + "'  style='width:60px;' id='txtItemPrice_" + nodeProd + "_" +
        itemIndex + "' name='txtItemPrice_" + nodeProd + "_" + itemIndex + "'/> Amtl. Gebühr:<input type='text' onkeyup='isnumber(this)'  value='" +
        acText + "' style='width:60px; " + amtGebEnabled + "'  id='txtAuthPrice_" +
        nodeProd + "_" + itemIndex + "' name='txtAuthPrice_" + nodeProd + "_" + itemIndex + "'/>");
        treeView.get_nodes().add(node);
        node.bindTemplate();
        itemIndex++;
        //Add the new node as the child of the selected node or the treeview if no node is selected
        var parent = treeView;
        parent.get_nodes().add(node);
        //Expand the parent if it is not the treeview
        if (parent != treeView && !parent.get_expanded())
            parent.set_expanded(true);
        treeView.commitChanges();
        return false;
    }
    function deleteNodeZul() {
        var treeView = $find('<%= DienstleistungTreeView.ClientID %>');
        var allNodes = treeView.get_allNodes();
        if (allNodes.length < 1) {
            alert("Sie haben keine Dienstleistung zu löschen!");
            return false;
        }
        var selectedNode = treeView.get_selectedNode();
        if (!selectedNode) {
            alert("Sie haben keine Dienstleistung ausgewählt!");
            return false;
        }
        if (allNodes.length == 1) {
            if (!confirm("Das ist die einzige Dienstleistung - wollen Sie sie löschen?"))
                return false;
        }
        treeView.trackChanges();
        var parent = selectedNode.get_parent();
        parent.get_nodes().remove(selectedNode);
        treeView.commitChanges();
        return false;
    }
    function openRadWindowZulPos() {
        $find("<%=RadWindowZul_Product.ClientID %>").show();
    }
    function PositionenLeeren() {
        var ProdAndCostLabel = document.getElementById("<%=ProdAndCostLabel.ClientID%>");
        var ProdField = document.getElementById("<%=ProduktHiddenField.ClientID%>");
        var AnzahlHiddenField = document.getElementById("<%=AnzahlVonDienstHiddenField.ClientID%>");
        AnzahlHiddenField.value = "";
        var NewPosButton = document.getElementById("<%=NewPositionZulButton.ClientID%>");
        NewPosButton.value = "Neue Dienstleistungen hinzufügen";
        ProdAndCostLabel.innerHTML = "";
        ProdField.value = "";
        itemIndex = 0;
    }
    function keyPress(sender, args) {
        var text = sender.get_value() + args.get_keyCharacter();
        if (!text.match('^[0-9]+$'))
            args.set_cancel(true);
    }
    function SavePositionClick() {
        var ProdAndCostLabel = document.getElementById("<%=ProdAndCostLabel.ClientID%>");
        var ProdField = document.getElementById("<%=ProduktHiddenField.ClientID%>");
        var prodDD = document.getElementById("<%=NewProductDropDownList.ClientID%>");
        var prodDDValue = prodDD.options[prodDD.selectedIndex].value;
        var prodDDText = prodDD.options[prodDD.selectedIndex].text;
        ProdAndCostLabel.innerHTML = ProdAndCostLabel.innerHTML + "Produkt:" + prodDDText + " <br/>";
        ProdField.value = ProdField.value + prodDDValue + ";";
        var NewPosButton = document.getElementById("<%=NewPositionZulButton.ClientID%>");
        var anzahl = prodDDValue.value.split(";");
        anzahl = anzahl.length - 1;
        NewPosButton.value = "Neue Dienstleistungen hinzufügen " + " (" + anzahl + ")";
    }
    function OnClientClose() {
        window.location.reload();
    }
    function EingabeFelderLeerenConfirm(sender, args) {
        if (window.confirm("Eingabefelder leeren?")) {
            itemIndex = 0;
            args.set_cancel(false);
        }
    }
    function GoToToday() {
        var datepicker = $find("<%=FirstRegistrationDateBox.ClientID%>");
        var dt = new Date();
        datepicker.set_selectedDate(dt);
        datepicker.hidePopup();
    }
    function GoToToday2() {
        var datepicker = $find("<%=ZulassungsdatumPicker.ClientID%>");
        var dt = new Date();
        datepicker.set_selectedDate(dt);
        datepicker.hidePopup();
    }


    function custVornameValueChanging(sender, eventArgs) {
        document.getElementById('<%=CarOwner_FirstnameBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custNachnameValueChanging(sender, eventArgs) {
        document.getElementById('<%=CarOwner_NameBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custStreetNrValueChanging(sender, eventArgs) {
        document.getElementById('<%=Adress_StreetNumberBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custStreetValueChanging(sender, eventArgs) {
        document.getElementById('<%=Adress_StreetBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custZipValueChanging(sender, eventArgs) {
        document.getElementById('<%=Adress_ZipcodeBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custCityValueChanging(sender, eventArgs) {
        document.getElementById('<%=Adress_CityBox.ClientID%>').value = eventArgs.get_newValue();
    }

    function custCountryValueChanging(sender, eventArgs) {
        document.getElementById('<%=Adress_CountryBox.ClientID%>').value = eventArgs.get_newValue();
    }
</script>
<asp:Panel runat="server" ID="ZulassungPanel" Enabled="false" Width="1000px">
    <div id="main_info">
        <table border="0">
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Rechnungserstellung: " ID="RadTextBox6" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:CheckBox ID="invoiceNow" runat="server" Text="Sofortrechnung" Checked="true" />
                </td>
                <td></td>
                <td>
                    <div style="float: right; z-index: 100; position: absolute; border: 1px solid;">
                        <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black"
                            BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Dienstleistungen: " ID="RadTextBox4">
                        </telerik:RadTextBox>
                        <br />
                        <asp:HiddenField runat="server" ID="SessionID" />
                        <telerik:RadPersistenceManager runat="server" ID="RadPersistenceManager1">
                            <PersistenceSettings>
                                <telerik:PersistenceSetting ControlID="DienstleistungTreeView" />
                            </PersistenceSettings>
                        </telerik:RadPersistenceManager>
                        <telerik:RadTreeView Skin="Office2010Blue" CollapseAnimation-Type="InOutCubic" ID="DienstleistungTreeView"
                            runat="server" Height="250px" Width="400px">
                        </telerik:RadTreeView>
                        <div style="float: right; z-index: 110; position: absolute; border: 1px solid;">
                            <asp:Label runat="server" ID="SmallCustomerHistorie" Visible="false" Width="400px"></asp:Label>
                        </div>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Bitte wählen Sie einen Kunden aus: " ID="RadCustomerTextBox" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadComboBox Height="300px" Width="250px" Enabled="true"
                        DataSourceID="CustomerDataSource" AutoPostBack="true" Filter="Contains" runat="server"
                        DropDownWidth="515px" EmptyMessage="Wählen Sie einen Kunden aus..." HighlightTemplatedItems="true"
                        DataTextField="Name" DataValueField="Value" ID="CustomerDropDownList"
                        OnSelectedIndexChanged="CustomerIndex_Changed">
                        <HeaderTemplate>
                            <table style="width: 515px" cellspacing="0" cellpadding="0">
                                <tr align="center">
                                    <td style="width: 90px;">Kundennummer
                                    </td>
                                    <td style="width: 175px;">Matchcode
                                    </td>
                                    <td style="width: 250px">Kundenname
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 515px;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 110px;">
                                        <%# DataBinder.Eval(Container, "DataItem.Kundennummer")%>
                                    </td>
                                    <td style="width: 175px;">
                                        <%# DataBinder.Eval(Container, "DataItem.Matchcode") %>
                                    </td>
                                    <td style="width: 250px;">
                                        <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                    <asp:Button runat="server" ID="btnClearSelection" Text="Neu" OnClick="btnClearSelection_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Zulassungsart: " ID="RegistrationTextBox" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadComboBox EmptyMessage="Zulassungstyp..." Filter="Contains" Width="250px" OnSelectedIndexChanged="RegistrationTyp_Changed" Enabled="True" DataTextField="Name" DataValueField="Value" DataSourceID="RegistrationOrderDataSource" AutoPostBack="true" runat="server" ID="RegistrationOrderDropDownList"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Produkt: " ID="ProductTextBox" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadComboBox Height="300px" Width="250px" Enabled="true"
                        DataSourceID="ProductDataSource" AutoPostBack="false" Filter="Contains" runat="server" DropDownWidth="515px" EmptyMessage="Produkt..." HighlightTemplatedItems="true"
                        DataTextField="Name" DataValueField="Value" ID="ProductDropDownList">
                        <HeaderTemplate>
                            <table style="width: 515px" cellspacing="0" cellpadding="0">
                                <tr align="center">
                                    <td style="width: 90px;">Produktnummer
                                    </td>
                                    <td style="width: 175px;">Produktname
                                    </td>

                                    <td style="width: 250px">Warengruppe
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 515px;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 110px;">
                                        <%# DataBinder.Eval(Container, "DataItem.ItemNumber")%>
                                    </td>
                                    <td style="width: 175px;">
                                        <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                        <%--   DataBinder.Eval(Container, "DataItem.AccountNumber")--%>
                                    </td>
                                    <td style="width: 250px;">
                                        <%# DataBinder.Eval(Container, "DataItem.Category")%>
                                    </td>
                                    <td style="visibility: hidden; width: 0px !important; height: 0px !important;">
                                        <asp:Label ID="myPrice" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Price")%>' Style="visibility: hidden;"></asp:Label>
                                        <asp:Label ID="myAuthCharge" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AuthCharge")%>' Style="visibility: hidden;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                    <asp:Button runat="server" ID="NewPositionZulButton" Text="Hinzufügen" OnClientClick="return addNodeZul()" />
                    <asp:Button runat="server" ID="DeleteNewPosButton" Text="Löschen" OnClientClick="return deleteNodeZul()" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Zulassungsstelle: " ID="ZulassungsstelleTextBox" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadComboBox Filter="Contains" DataTextField="Name" DataValueField="Value" DataSourceID="ZulassungsstelleDataSource" ID="ZulassungsstelleComboBox" DropDownWidth="515px" EmptyMessage="Zulassungsstelle..." HighlightTemplatedItems="true" runat="server" Width="250px" AutoPostBack="false">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Zulassungsdatum: " ID="RadTextBox5" Width="240px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadDatePicker runat="server" ID="ZulassungsdatumPicker" Width="250px" MinDate="1/1/1900">
                        <Calendar ID="Calendar" runat="server">
                            <FooterTemplate>
                                <div style="width: 100%; text-align: center; background-color: Gray;">
                                    <input id="Button1" type="button" value="Heute" onclick="GoToToday2()" />
                                </div>
                            </FooterTemplate>
                        </Calendar>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table border="0" cellspacing="0" frame="void" cellpadding="0">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label7" runat="server"
                                    AssociatedControlID="ProductDropDownList" Font-Bold="true" Text="Sofortkundendaten"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 245px;">
                                <asp:Label ID="lblSmallCustomerVorname" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Vorname:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerVorname" runat="server"
                                    Width="240px">
                                    <ClientEvents OnValueChanging="custVornameValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerNachname" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Nachname:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerNachname" runat="server"
                                    Width="240px">
                                    <ClientEvents OnValueChanging="custNachnameValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerTitle" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Titel:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerTitle" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblGender" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Geschlecht:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbSmallCustomerGender" runat="server"
                                    AllowCustomText="True" Culture="de-DE" MarkFirstMatch="True"
                                    Width="60px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Owner="cmbSmallCustomerGender"
                                            Selected="true" Text="M" Value="M" />
                                        <telerik:RadComboBoxItem runat="server" Owner="cmbSmallCustomerGender" Text="W"
                                            Value="W" />
                                    </Items>
                                </telerik:RadComboBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerStreetNr" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Strasse /Nr:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerStreet" runat="server"
                                    Width="200px">
                                    <ClientEvents OnValueChanging="custStreetValueChanging" />
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txbSmallCustomerNr" runat="server"
                                    Width="35px">
                                    <ClientEvents OnValueChanging="custStreetNrValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerZipCode" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="PLZ:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerZipCode" runat="server"
                                    Width="240px">
                                    <ClientEvents OnValueChanging="custZipValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerCity" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Stadt:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="cmbSmallCustomerCity" runat="server"
                                    Width="240px">
                                    <ClientEvents OnValueChanging="custCityValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerCountry" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Land:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerCountry" runat="server" Text="Deutschland"
                                    Width="240px">
                                    <ClientEvents OnValueChanging="custCountryValueChanging" />
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerNumber" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Kundennummer:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerNumber" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerPhone" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Telefon:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerPhone" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr class="FormContainer">
                            <td>
                                <asp:Label ID="lblSmallCustomerFax" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Fax:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerFax" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerMobil" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Mobil:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerMobil" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerEmail" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="Email:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerEmail" runat="server"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerZahlungsziel" runat="server"
                                    AssociatedControlID="txbSmallCustomerZahlungsziel" Text="Zahlungsziel:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="txbSmallCustomerZahlungsziel" runat="server"
                                    Width="240px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Sofort" Value="0" />
                                        <telerik:RadComboBoxItem Text="5" Value="5" />
                                        <telerik:RadComboBoxItem Text="10" Value="10" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSmallCustomerVat" runat="server"
                                    AssociatedControlID="ProductDropDownList" Text="MwSt:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txbSmallCustomerVat" runat="server" Text="19"
                                    Width="240px">
                                </telerik:RadTextBox>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%--HauptInfo End--%>
    <br />
    <asp:HiddenField runat="server" ID="LicenceNumberCacheField" />
    <asp:HiddenField runat="server" ID="VehicleIdField" />
    <asp:HiddenField runat="server" ID="RegistrationIdField" />
    <telerik:RadFormDecorator runat="server" ID="AuftragRadDecorator" />
    <asp:Panel runat="server" Visible="true" ID="Halter">
        <asp:Label Text="Halter" ForeColor="Blue" ID="HalterLabel" runat="server" Visible="false" />
        <br />
        <asp:Panel runat="server" Visible="false" ID="CarOwner_Firstname">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Vorname: " ID="OwnerFirstNameLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="CarOwner_FirstnameBox">
                <ClientEvents OnValueChanging="MyFirstValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="CarOwner_Name">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Name: " ID="OwnerNameLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="CarOwner_NameBox">
                <ClientEvents OnValueChanging="MyFirstValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Adress_Street">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Straße: " ID="OwnerStreetLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Adress_StreetBox">
                <ClientEvents OnValueChanging="MyFirstValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Adress_StreetNumber">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Nummer: " ID="OwnerStreetNubmerLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" MaxLength="10" runat="server" ID="Adress_StreetNumberBox"></telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Adress_Zipcode">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="PLZ: " ID="OwnerZipCodeLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" MaxLength="10" runat="server" ID="Adress_ZipcodeBox"></telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Adress_City">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Stadt: " ID="OwnerCityLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Adress_CityBox">
                <ClientEvents OnValueChanging="MyFirstValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Adress_Country">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Land: " ID="OwnerCountryLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Adress_CountryBox" Text="Deutschland">
                <ClientEvents OnValueChanging="MyFirstValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="FahrzeugPanel">
        <asp:Label Text="Kfz-Daten" Visible="false" ForeColor="Blue" ID="FahrzeugLabel" runat="server" />
        <asp:Panel runat="server" Visible="false" ID="Registration_RegistrationDocumentNumber">
            <telerik:RadTextBox ID="RegDocNumbLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Briefnummer: " Visible="True" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" ID="RegDocNumBox" MaxLength="8" runat="server">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_HSN">
            <telerik:RadTextBox ID="HSNLabel" runat="server" BorderColor="Transparent"
                DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black"
                Text="HSN: " Enabled="false" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" MaxLength="4" AutoPostBack="false" runat="server" ID="HSNBox">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <asp:Label runat="server" Enabled="true" Visible="false" Text="" ID="HSNSearchLabel"></asp:Label>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_TSN">
            <telerik:RadTextBox ID="TSNLabel" runat="server" BorderColor="Transparent"
                DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black"
                Enabled="false" Text="TSN: " Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" MaxLength="4" runat="server" ID="TSNBox">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <asp:RegularExpressionValidator ID="RegExVal" runat="server"
                ControlToValidate="TSNBox"
                ValidationExpression=".{3}.*"
                Display="Static"
                ForeColor="Red"
                ErrorMessage="TSN ist weniger als 3 Zeichen">
            </asp:RegularExpressionValidator>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_VIN">
            <telerik:RadTextBox ID="VINLabel" runat="server" BorderColor="Transparent"
                DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="FIN: "
                Visible="true" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="226px" MaxLength="17" AutoPostBack="false" ID="VINBox" runat="server">
                <ClientEvents OnValueChanging="MyValueChanging23" />
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="PruefzifferBox" MaxLength="1" runat="server" Width="20">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_Color">
            <telerik:RadTextBox Width="240px" ID="ColorLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Farbe: " Visible="True">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" ID="Vehicle_ColorBox" MaxLength="1" runat="server">
                <ClientEvents OnKeyPress="keyPress" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Registration_EmissionCode">
            <telerik:RadTextBox Width="240px" ID="EmissionsCodeLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Emission: " Visible="True">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" ID="EmissionsCodeBox" MaxLength="5" runat="server">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Registration_eVBNumber">
            <telerik:RadTextBox ID="InsuranceLabel" runat="server"
                BorderColor="Transparent" DisabledStyle-BackColor="Transparent"
                DisabledStyle-ForeColor="Black" Enabled="false" Text="eVB-Nummer: " Visible="True"
                Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox MaxLength="7" Width="250px" runat="server" ID="Registration_eVBNumberBox">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_Variant">
            <telerik:RadTextBox ID="VariantLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Variant: " Visible="True" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" ID="Vehicle_VariantBox" runat="server">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Registration_Licencenumber">
            <telerik:RadTextBox ID="LicenceNumLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Kennzeichen: " Visible="True" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="LicenceBox1" MaxLength="3" runat="server" Width="70">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="LicenceBox2" MaxLength="2" runat="server" Width="86">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="LicenceBox3" MaxLength="4" runat="server" Width="86">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <asp:Button runat="server" ID="KennzeichenTauschButton" OnClick="KennzeichenTauschButton_Clicked" Text="Umtauschen" Visible="false"></asp:Button>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="RegistrationOrder_PreviousLicencenumber">
            <telerik:RadTextBox ID="PreviousLicNumLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Vorherige Kennzeichen: " Visible="True" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="PreviousLicenceBox1" MaxLength="3" runat="server" Width="70">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="PreviousLicenceBox2" MaxLength="2" runat="server" Width="86">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
            <telerik:RadTextBox ID="PreviousLicenceBox3" MaxLength="4" runat="server" Width="86">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Registration_GeneralInspectionDate">
            <table style="padding: 0px; text-align: left; margin: 0px; border-spacing: 0px;" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 240px; text-align: left;">
                        <telerik:RadTextBox ID="InspectionLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Inspektionsdatum: " Visible="True" Width="240px">
                        </telerik:RadTextBox></td>
                    <td style="width: 200px; text-align: left; padding-left: 4px;">
                        <telerik:RadMonthYearPicker Width="200px" MinDate="1/1/1900" Visible="true" ID="Registration_GeneralInspectionDateBox" runat="server">
                        </telerik:RadMonthYearPicker>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Vehicle_FirstRegistrationDate">
            <table style="padding: 0px; text-align: left; margin: 0px; border-spacing: 0px;" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 240px; text-align: left;">
                        <telerik:RadTextBox ID="FirstRegistrationDateLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Erstzulassungsdatum: " Visible="True" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 200px; text-align: left; padding-left: 4px;">
                        <telerik:RadDatePicker Width="200px" ID="FirstRegistrationDateBox" MinDate="1/1/1900" runat="server">
                            <Calendar ID="Calendar2" runat="server">
                                <FooterTemplate>
                                    <div style="width: 100%; text-align: center; background-color: Gray;">
                                        <input id="Button1" type="button" value="Heute" onclick="GoToToday()" />
                                    </div>
                                </FooterTemplate>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Order_Freitext">
            <telerik:RadTextBox ID="FreiTextLabel" runat="server" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" DisabledStyle-ForeColor="Black" Enabled="false" Text="Freitext: " Visible="True" Width="240px">
            </telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" ID="FreiTextBox" runat="server">
                <ClientEvents OnValueChanging="MyValueChanging" />
            </telerik:RadTextBox>
        </asp:Panel>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel runat="server" Visible="true" ID="Halterdaten">
        <asp:Label Text="Bankdaten" ForeColor="Blue" ID="HalterdatenLabel" runat="server" Visible="false" />
        <br />
        <telerik:RadAjaxPanel runat="server" ID="loadIban">
            <asp:Panel runat="server" Visible="false" ID="BankAccount_BankName">
                <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Bankname: " ID="BankNameLabel" Width="240px"></telerik:RadTextBox>
                <telerik:RadTextBox Width="250px" runat="server" ID="BankAccount_BankNameBox">
                    <ClientEvents OnValueChanging="MyFirstValueChanging" />
                </telerik:RadTextBox>
            </asp:Panel>
            <asp:Panel runat="server" Visible="false" ID="BankAccount_Accountnumber">
                <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Kontonummer: " ID="AccountNumberLabel" Width="240px"></telerik:RadTextBox>
                <telerik:RadTextBox Width="250px" runat="server" ID="BankAccount_AccountnumberBox">
                    <ClientEvents OnValueChanging="MyValueChanging" />
                </telerik:RadTextBox>
            </asp:Panel>
            <asp:Panel runat="server" Visible="false" ID="BankAccount_BankCode">
                <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="BLZ: " ID="BankCodeLabel" Width="240px"></telerik:RadTextBox>
                <telerik:RadTextBox Width="250px" runat="server" ID="BankAccount_BankCodeBox">
                    <ClientEvents OnValueChanging="MyValueChanging" />
                </telerik:RadTextBox>
            </asp:Panel>
            <asp:Panel runat="server" Visible="false" ID="IBANPanel">
                <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black"
                    BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="IBAN: " ID="txbIBANInfo" Width="240px">
                </telerik:RadTextBox>
                <telerik:RadTextBox Width="250px" runat="server" ID="txbBancAccountIban">
                </telerik:RadTextBox>
                <br />
                <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black"
                    BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="BIC: " ID="txbBICInfo" Width="240px">
                </telerik:RadTextBox>
                <telerik:RadTextBox Width="250px" runat="server" ID="txbBankAccount_Bic">
                </telerik:RadTextBox>
                <telerik:RadButton ID="btnGenerateIBAN" runat="server" Text="IBAN/BIC" ToolTip="IBAN Nummer generieren" OnClick="genIban_Click"></telerik:RadButton>
            </asp:Panel>
        </telerik:RadAjaxPanel>
    </asp:Panel>
    <br />
    <asp:Panel Visible="true" runat="server" ID="Kontaktdaten">
        <asp:Label Text="Kontaktdaten" ForeColor="Blue" ID="KontaktdatenLabel" runat="server" Visible="false" />
        <br />
        <asp:Panel runat="server" Visible="false" ID="Contact_Phone">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Telefonnummer: " ID="OwnerPhoneLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Contact_PhoneBox"></telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Contact_Fax">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Fax: " ID="OwnerFaxLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Contact_FaxBox"></telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Contact_MobilePhone">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Mobil: " ID="OwnerMobilePhoneLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Contact_MobilePhoneBox"></telerik:RadTextBox>
        </asp:Panel>
        <asp:Panel runat="server" Visible="false" ID="Contact_Email">
            <telerik:RadTextBox runat="server" Enabled="false" Visible="True" DisabledStyle-ForeColor="Black" BorderColor="Transparent" DisabledStyle-BackColor="Transparent" Text="Email: " ID="OwnerEmailLabel" Width="240px"></telerik:RadTextBox>
            <telerik:RadTextBox Width="250px" runat="server" ID="Contact_EmailBox"></telerik:RadTextBox>
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:Button ID="AuftragZulassenButton" runat="server" Text="Zulassungsauftrag erstellen" OnClientClick="return checkFields();" OnClick="AuftragZulassenButton_Clicked" />
    <telerik:RadButton runat="server" ID="NaechtenAuftragButton" OnClick="NaechtenAuftragButton_Clicked" OnClientClicking="EingabeFelderLeerenConfirm" Text="Eingabefelder leeren"></telerik:RadButton>
    <asp:Label runat="server" Visible="false" ForeColor="Green" ID="ZulassungOkLabel" Text="Neuer Auftrag wurde erfolgreich erstellt!"></asp:Label>
    <asp:Label runat="server" Visible="false" ForeColor="Red" ID="SubmitChangesErrorLabel" Text="Bitte überprüfen Sie Ihre Eingaben und versuchen Sie es erneut. Wenn das Problem weiterhin auftritt, wenden Sie sich an den Systemadministrator."></asp:Label>
    <asp:Label runat="server" Visible="false" ForeColor="Red" ID="ErrorLeereTextBoxenLabel" Text="Bitte überprüfen Sie die Pflichtfelder!"></asp:Label>
    <asp:LinqDataSource ID="CustomerDataSource" runat="server" OnSelecting="CustomerLinq_Selected" ContextTypeName="KVSCommon.Database.DataClasses1DataContext">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="ZulassungsstelleDataSource" runat="server" OnSelecting="ZulassungsstelleDataSourceLinq_Selected" ContextTypeName="KVSCommon.Database.DataClasses1DataContext">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="RegistrationOrderDataSource" runat="server" OnSelecting="RegistrationOrderDataSourceLinq_Selected" ContextTypeName="KVSCommon.Database.DataClasses1DataContext">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="ProductDataSource" runat="server" OnSelecting="ProductDataSourceLinq_Selected" ContextTypeName="KVSCommon.Database.DataClasses1DataContext">
    </asp:LinqDataSource>
</asp:Panel>
<asp:HiddenField runat="server" ID="smallCustomerOrderHiddenField" />
<telerik:RadWindowManager runat="server" ID="WindowManager1" EnableViewState="false" DestroyOnClose="true" VisibleOnPageLoad="false" ReloadOnShow="true"></telerik:RadWindowManager>
<telerik:RadWindow ShowContentDuringLoad="false" runat="server" Height="500" Width="450" ID="AddAdressRadWindow" Modal="true" DestroyOnClose="true" ReloadOnShow="true">
    <ContentTemplate>
        <asp:Label runat="server" ID="LocationLabelWindow"></asp:Label>
        <div class="contButton">
            <asp:Label runat="server" ID="StreetLabel" Text="Straße*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="StreetTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label1" Text="Nummer*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="StreetNumberTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label2" Text="PLZ*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="ZipcodeTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label3" Text="Stadt*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="CityTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label4" Text="Land*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="CountryTextBox" runat="server" Text="Deutschland"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label5" Text="Rechnungsempfänger*: " Width="140"></asp:Label>
            <asp:TextBox Width="220" ID="InvoiceRecipient" runat="server"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="Label6" Text="Rabatt in %: " Width="140"></asp:Label>
            <telerik:RadNumericTextBox Width="220" ID="txbDiscount" runat="server" NumberFormat-DecimalDigits="0" Value="0" MinValue="0" MaxValue="100"></telerik:RadNumericTextBox>
            <br />
        </div>
        <br />
        <br />
        <div class="contButton">
            <telerik:RadButton ID="AddAdressButton" Text="Speichern und neue Rechnungsauftrag erstellen" runat="server" OnClick="OnAddAdressButton_Clicked">
            </telerik:RadButton>
        </div>
        <asp:Label ID="AllesIstOkeyLabel" runat="server" Text=""></asp:Label>
        <asp:Label ID="ZusatzlicheInfoLabel" Visible="false" runat="server" Text="*Die Rechnung wird sofort erstellt!"></asp:Label>
    </ContentTemplate>
</telerik:RadWindow>
<telerik:RadFormDecorator runat="server" ID="OffenNeuzulassungFormDekorator" DecoratedControls="all" />
<telerik:RadWindow Title="Neue Position hinzufügen" runat="server" ID="RadWindowZul_Product" Modal="true" Width="600px" Height="600px">
    <ContentTemplate>
        <p class="contText">
            Alle neue Positionen:
        </p>
        <p>
            <asp:Label runat="server" ID="ProdAndCostLabel"></asp:Label>
        </p>
        <p class="contText">
            Wählen Sie bitte neues Produkt aus
        </p>
        <asp:DropDownList DataTextField="Name" DataValueField="Value" DataSourceID="ProductDataSource" runat="server" ID="NewProductDropDownList"></asp:DropDownList>
        <p class="contText">
            Und bestätigen:
        </p>
        <div class="contButton">
            <asp:Button ID="NewPositionButtonHinzuguegen" Text="Neue Position hinzufügen" runat="server" OnClientClick="SavePositionClick(); return false;"></asp:Button>
            <asp:Button ID="PositionenLeerButton" Text="Positionen leeren" runat="server" OnClientClick="PositionenLeeren(); return false;"></asp:Button>
        </div>
    </ContentTemplate>
</telerik:RadWindow>
<asp:HiddenField runat="server" ID="CostCenterHiddenField" />
<asp:HiddenField runat="server" ID="ProduktHiddenField" />
<asp:HiddenField runat="server" ID="AnzahlVonDienstHiddenField" />
<asp:HiddenField runat="server" ID="LabelProdCostHiddenField" />
<link href="../Styles/auftragseingang.css" rel="stylesheet" type="text/css" />
