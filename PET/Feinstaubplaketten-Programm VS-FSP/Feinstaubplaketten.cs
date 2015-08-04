﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel;
using PET.WebService;
using System.ServiceModel.Description;
using KVSContracts;

namespace PET
{
    public partial class Feinstaubplaketten : Form
    {
        public Feinstaubplaketten()
        {
            Thread thread = new Thread(new ThreadStart(Splashscreen));
            thread.Start();
            Thread.Sleep(3000);
            InitializeComponent();

            thread.Abort();
            this.Focus();

            //Create a URI to serve as the base address
            //var httpUrl = new Uri("http://localhost:8090/PrintService");
            //Create ServiceHost
            var host = new ServiceHost(typeof(PetService));//, httpUrl);
            //Add a service endpoint
            //host.AddServiceEndpoint(typeof(IPrintService), new WSHttpBinding(), "");
            //Enable metadata exchange
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //host.Description.Behaviors.Add(smb);
            //Start the Service
            host.Open();
        }

        public void Splashscreen()
        {
            Application.Run(new Splashscreen());
        }
        private void Feinstaubplaketten_Load(object sender, EventArgs e)
        {
            //Fremdzündung wird von Anfang an auf True gesetzt------------
            this.radioButton1.Checked = true;
            string CSVPath = "";
            if (System.Configuration.ConfigurationManager.AppSettings["DatatablePath"] != "")
            {
                CSVPath = System.Configuration.ConfigurationManager.AppSettings["DatatablePath"];
            }
            else
            {
                CSVPath = Application.StartupPath + @"\LogDatenbank.csv";
            }
            if (!System.IO.File.Exists(CSVPath))
            {
                MessageBox.Show("Die Datenbank-Datei konnte nicht gefunden werden, es wurde eine neue angelegt. Sie finden die Datenbank-Datei unter: " + CSVPath);
                string kennzeichen = "Kennzeichen";
                string fahrzeugart = "Fahrzeugart";
                string antriebsart = "Antriebsart";
                string stufe = "Stufe";
                string emissionsschluessel = "Emissionsschluessel";
                string farbe = "Farbe";
                string benutzername = "Benutzername";
                string datum = "Datum";
                DatatableWriter(kennzeichen, fahrzeugart, antriebsart, stufe, emissionsschluessel, farbe, benutzername, datum);
            }
        }

        public void DatatableWriter(string kennzeichen, string fahrzeugart, string antriebsart, string stufe, string emissionsschluessel, string farbe, string benutzername, string datum)
        {
            string CSVPath = "";
            if (System.Configuration.ConfigurationManager.AppSettings["DatatablePath"] != "")
            {
                CSVPath = System.Configuration.ConfigurationManager.AppSettings["DatatablePath"];
            }
            else
            {
                CSVPath = Application.StartupPath + @"\LogDatenbank.csv";
            }

            using (FileStream myfs = new FileStream(CSVPath, FileMode.Append, FileAccess.Write))
            using (StreamWriter mysw = new StreamWriter(myfs))
            {
                mysw.WriteLine(kennzeichen.ToUpper() + ";" + fahrzeugart.ToUpper() + ";" + antriebsart.ToUpper() + ";" + stufe.ToUpper() + ";" + emissionsschluessel.ToUpper() + ";" + farbe.ToUpper() + ";" + benutzername.ToUpper() + ";" + datum.ToUpper());
                mysw.Close();
                myfs.Close();
            }
        }
        //Prüft ob die Tabelle LogTBL vorhanden ist, wenn nicht wird sie erstellt END
        //Form on Load END-----------------------------------------------

        //Fremdzündung Checked?------------------------------------------
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton fremdzuendend = sender as RadioButton;
            if (fremdzuendend.Checked)
            {
                this.comboBox3.Enabled = false;
                this.comboBox1.Enabled = true;
                this.comboBox1.Items.Clear();
                this.comboBox1.Items.Add("M1");
                this.comboBox1.Items.Add("M2, M3, N");
                this.comboBox1.Text = string.Empty;

                this.txtBoxEmissionsschluessel.Text = string.Empty;
                this.txtBoxEmissionsschluessel.Enabled = false;
                this.comboBox3.Items.Clear();
                this.comboBox3.Text = string.Empty;
                this.button1.Enabled = false;
            };
        }
        //Fremdzündung Checked? END--------------------------------------
        //Selbstzündung Checked?-----------------------------------------
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selbstzuendend = sender as RadioButton;
            if (selbstzuendend.Checked)
            {
                this.comboBox1.Enabled = true;
                this.comboBox1.Items.Clear();
                this.comboBox1.Items.Add("M1 (PMS)");
                this.comboBox1.Items.Add("M1");
                this.comboBox1.Items.Add("M2, M3, N");
                this.comboBox1.Items.Add("M2, M3, N (PMS)");
                this.comboBox1.Text = string.Empty;

                this.txtBoxEmissionsschluessel.Text = string.Empty;
                this.txtBoxEmissionsschluessel.Enabled = false;
                this.button1.Enabled = false;
            };
        }
        //Selbstzündung Checked? END-------------------------------------
        //Elektromotor / Neuzulassung Checked?-----------------------------------------
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Text = string.Empty;
            this.comboBox1.Enabled = false;
            this.txtBoxEmissionsschluessel.Text = string.Empty;
            this.txtBoxEmissionsschluessel.Enabled = false;
            this.comboBox3.Items.Clear();
            this.comboBox3.Text = string.Empty;
            this.comboBox3.Enabled = false;

            if (this.radioButton3.Checked && this.textBox1.Text != null && this.textBox1.Text != "")
            {
                this.button1.Enabled = true;
            }
            else
            {
                this.button2.Enabled = false;
            }
        }

        //Emissionsschlüssel in die Combobox einfügen--------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBoxEmissionsschluessel.Text = string.Empty;
            this.txtBoxEmissionsschluessel.Enabled = false;
            this.comboBox3.Items.Clear();
            this.comboBox3.Text = string.Empty;
            if (radioButton1.Checked)
            {
                this.txtBoxEmissionsschluessel.Enabled = true;
            }
            else if (radioButton2.Checked)
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "M1 (PMS)":
                        this.comboBox3.Enabled = true;
                        this.comboBox3.Items.Clear();
                        this.comboBox3.Text = string.Empty;
                        this.txtBoxEmissionsschluessel.Enabled = false;
                        this.txtBoxEmissionsschluessel.Text = string.Empty;
                        this.comboBox3.Items.Add("PM01");
                        this.comboBox3.Items.Add("PM0");
                        this.comboBox3.Items.Add("PM1");
                        this.comboBox3.Items.Add("PM2");
                        this.comboBox3.Items.Add("PM3");
                        this.comboBox3.Items.Add("PM4");
                        this.comboBox3.Items.Add("PM5");
                        this.button1.Enabled = false;
                        break;
                    case "M1":
                        this.txtBoxEmissionsschluessel.Enabled = true;
                        this.button1.Enabled = false;
                        this.comboBox3.Enabled = false;
                        this.comboBox3.Text = string.Empty;
                        break;
                    case "M2, M3, N":
                        this.txtBoxEmissionsschluessel.Enabled = true;
                        this.button1.Enabled = false;
                        this.comboBox3.Enabled = false;
                        this.comboBox3.Text = string.Empty;
                        break;
                    case "M2, M3, N (PMS)":
                        this.comboBox3.Items.Clear();
                        this.txtBoxEmissionsschluessel.Enabled = false;
                        this.txtBoxEmissionsschluessel.Text = string.Empty;
                        this.button1.Enabled = false;
                        this.comboBox3.Text = string.Empty;
                        this.comboBox3.Enabled = true;
                        this.comboBox3.Items.Add("PM01");
                        this.comboBox3.Items.Add("PM0");
                        this.comboBox3.Items.Add("PM1");
                        this.comboBox3.Items.Add("PM2");
                        this.comboBox3.Items.Add("PM3");
                        this.comboBox3.Items.Add("PM4");
                        this.comboBox3.Items.Add("PM5");
                        break;
                    default:
                        //Ein anderer Fall ist nicht möglich
                        break;
                }
            }
        }
        //Emissionsschlüssel in die Combobox einfügen END-------------------------------------
        //Verfügbare Emissionsschlüssel für PMS festlegen-------------------------------------
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.txtBoxEmissionsschluessel.Enabled = true;
        }
        //Verfügbare Emissionsschlüssel END-------------------------------------------
        //Druckenbutton wird freigeschaltet wenn ComboBox2 != "" && textBox1 != ""----
        private void txtBoxEmissionsschluessel_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBoxEmissionsschluessel.Text != "" && textBox1.Text != "")
            {
                this.button1.Enabled = true;
            }
            else
            {
                this.button1.Enabled = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked && this.textBox1.Text != null && this.textBox1.Text != "")
            {
                this.button1.Enabled = true;
            }
            else if (this.txtBoxEmissionsschluessel.Text != null && this.txtBoxEmissionsschluessel.Text != null)
            {
                if (this.txtBoxEmissionsschluessel.Text != "" && textBox1.Text != "")
                {
                    this.button1.Enabled = true;
                }
                else
                {
                    this.button1.Enabled = false;
                }
            }
        }

        /*
         * Bei Probleme - folgendes überprüfen:
         * Drucker zuerst lokal mit USB installieren und dann durch das Netzwerk benutzen.
         * */
        //Druckenbutton wird freigeschaltet wenn txtBoxEmissionsschluessel.Text != "" && textBox1 != "" END
        //Drucken---------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            string strKennzeichen = textBox1.Text;
            string strFahrzeugart;
            string csvEmissionsPath = Application.StartupPath + @"\";
            string csvEmissionsDatei = "Emissionsschluessel.csv";
            string csvConnString = @"Provider=Microsoft.ACE.OLEDB.14.0; Data Source=" + csvEmissionsPath + "; Extended Properties='text;HDR=Yes;FMT=Delimited'";
            string csvquery = "";
            {
                if (radioButton1.Checked)
                {
                    strFahrzeugart = "Fremdzündung";
                }
                else if (this.radioButton3.Checked)
                {
                    strFahrzeugart = "ElektroNeuzulassung";
                }
                else
                {
                    strFahrzeugart = "Selbstzündung";
                }
                string strAntriebsart = comboBox1.Text;
                string strStufe = comboBox3.Text;
                string strEmissionsschluesselnr = txtBoxEmissionsschluessel.Text;
                string strFarbe = "N/A";
                string strDatum = System.DateTime.Now.ToString("dd.MM.yyyy");
                string strUsername = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                //Plakettenfarbe wird festgelegt----------------------------------------------
                if (File.Exists(csvEmissionsDatei))
                {
                    strEmissionsschluesselnr = strEmissionsschluesselnr.ToUpper();
                    if (strFahrzeugart == radioButton1.Text)
                    {
                        switch (strAntriebsart)
                        {
                            case "M1":
                                //csv Abfrage
                                csvquery = @"select PKWBenzin from [" + csvEmissionsDatei + @"] where [Schluessel]='"+ strEmissionsschluesselnr + "'" ;
                                break;
                            case "M2, M3, N":
                                csvquery = @"select LKWBenzin from [" + csvEmissionsDatei + @"] where  [Schluessel]='" + strEmissionsschluesselnr + "'";
                                break;
                        }
                    }
                    else if (strFahrzeugart == "ElektroNeuzulassung")
                    {
                        csvquery = @"select PKWBenzin from [" + csvEmissionsDatei + @"] where [Schluessel]='1'";
                    }
                    else
                    {
                        switch (strAntriebsart)
                        {
                            case "M1 (PMS)":
                                csvquery = @"select PKW" + strStufe + " from [" + csvEmissionsDatei + @"] where [Schluessel]='" + strEmissionsschluesselnr + "'";
                                break;
                            case "M1":
                                csvquery = @"select PKWDiesel from [" + csvEmissionsDatei + @"] where [Schluessel]='" + strEmissionsschluesselnr + "'";
                                break;
                            case "M2, M3, N":
                                csvquery = @"select LKWDiesel from [" + csvEmissionsDatei + @"] where [Schluessel]='" + strEmissionsschluesselnr + "'";
                                break;
                            case "M2, M3, N (PMS)":
                                csvquery = @"select LKW" + strStufe + " from [" + csvEmissionsDatei + @"] where [Schluessel]='" + strEmissionsschluesselnr + "'";
                                break;
                            default:

                                break;
                        }
                    }
                    strFarbe = oledbQuery(csvquery, csvConnString);
                }
                else
                {
                    MessageBox.Show("Die Emissionsschlüssel Tabelle fehlt.", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Plakettenfarbe wird festgelegt END------------------------------------------
                //Plakette wird angezeigt und das Form wird vergrößert------------------------
                this.ClientSize = new System.Drawing.Size(376, 489);
              
                this.imgPlakette.Visible = true;

                if (strFarbe == "Gruen")
                {
                    this.imgPlakette.Visible = true;
                    this.imgPlakette.Load(Environment.CurrentDirectory + @"\gruen.png");
                }
                else if (strFarbe == "Gelb")
                {
                    this.imgPlakette.Visible = true;
                    this.imgPlakette.Load(Environment.CurrentDirectory + @"\gelb.png");
                    MessageBox.Show("Die Feinstaubplakettenfarbe ist Gelb, es wird keine Feinstaubplakette gedruckt.", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (strFarbe == "Rot")
                {
                    this.imgPlakette.Visible = true;
                    this.imgPlakette.Load(Environment.CurrentDirectory + @"\rot.png");
                    MessageBox.Show("Die Feinstaubplakettenfarbe ist Rot, es wird keine Feinstaubplakette gedruckt.", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (strFarbe == "Keine")
                {
                    this.ClientSize = new System.Drawing.Size(382, 311);
                    this.imgPlakette.Visible = false;
                    MessageBox.Show("Dieses Fahrzeug erhält keine Plakette, Ergebnis: 0.", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.ClientSize = new System.Drawing.Size(382, 311);
                    this.imgPlakette.Visible = false;
                    MessageBox.Show("Es konnte keine passende Plakettenfarbe zugeordnet werden, diese Kombination enthält kein Ergebnis.", "Achtung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //Plakette wird angezeigt und das Form wird vergrößert END--------------------
                //Beim Klick auf Drucken werden die Daten in die DB geschrieben, egal ob das Kennzeichen bereits vorhanden ist---------------
                if (strFahrzeugart == "Fremdzündung")
                {
                    strFahrzeugart = "Fremdzuendung";
                }
                else if (strFahrzeugart == "Selbstzündung")
                {
                    strFahrzeugart = "Selbstzuendung";
                }

                DatatableWriter(strKennzeichen, strFahrzeugart, strAntriebsart, strStufe, strEmissionsschluesselnr, strFarbe, strUsername, strDatum);

                if (strFarbe == "Gruen")
                {
                    // Zweiter Satz auf der Plakette
                    string additionalInfo = System.Configuration.ConfigurationManager.AppSettings["AdditionalInfo"];
                    //Etticket wird gedruckt------------------------------------------------------
                    //UTF8Encoding utf8 = new UTF8Encoding();
                    //byte[] encodedBytes = utf8.GetBytes(strKennzeichen);
                    //strKennzeichen = utf8.GetString(encodedBytes);
                    //wird Abstand für Kennz. und Zusätz. berechnet um den zweiten Satz zentriert auszudrucken
                    string printString = "m m\r\nJ\r\nH 50\r\nO T5\r\nD 0,3\r\nS l2;0,4,99,99,103\r\nT 23,65,0,5,9.85,q88;[J:c57]" + strKennzeichen.ToUpper() + "\r\nT 23,69,0,5,2.85;[J:c57]" + additionalInfo + "\r\nA 1\r\n";
                    string cabName = System.Configuration.ConfigurationManager.AppSettings["cabName"];
                    RawPrinterHelper.SendStringToPrinter(cabName, printString);
                    //Etticket wird gedruckt END -------------------------------------------------
                }
                textBox1.Text = string.Empty;
                //Beim Klick auf Drucken werden die Daten in die DB geschrieben, egal ob das Kennzeichen bereits vorhanden ist END-----------
            }
        }

        private string oledbQuery(string csvquery, string csvConnString)
        {
            string strFarbe = "";
            string strFarbeZahl;
            //OleDbConnection dbconn = new OleDbConnection();
            //dbconn.ConnectionString = csvConnString;
            //dbconn.Open();
            //OleDbCommand cmd = new OleDbCommand(csvquery, dbconn);
            //OleDbDataReader reader = cmd.ExecuteReader();
            //string strFarbeZahl = reader[0].ToString();
            using (OleDbConnection dbconn = new OleDbConnection(csvConnString))
            {
                OleDbCommand cmd = new OleDbCommand(csvquery, dbconn);
                dbconn.Open();
                Object reader = cmd.ExecuteScalar();
                if (reader == null) {
                    dbconn.Close();
                    return "Ungueltig";
                }
                strFarbeZahl = reader.ToString();
                dbconn.Close();
            }
            if (strFarbeZahl == "4")
            {
                strFarbe = "Gruen";
            }
            else if (strFarbeZahl == "3")
            {
                strFarbe = "Gelb";
            }
            else if (strFarbeZahl == "2")
            {
                strFarbe = "Rot";
            }
            else if (strFarbeZahl == "0")
            {
                strFarbe = "Keine";
            }
            else
            {
                strFarbe = "Ungueltig";
            }

            return strFarbe;
        }
        //Drucken END-----------------------------------------------------------------
        //Datenbestand ---------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.comboBox3.Items.Clear();
            this.comboBox1.Text = string.Empty;
            this.txtBoxEmissionsschluessel.Text = string.Empty;
            this.comboBox3.Text = string.Empty;
            this.txtBoxEmissionsschluessel.Enabled = false;
            this.comboBox3.Enabled = false;
            this.textBox1.Text = string.Empty;
        }
        private void dateiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feinstaubplaketten_Datenbestand datenbestand = new Feinstaubplaketten_Datenbestand();
            datenbestand.Show();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feinstaubplaketten_Programm_Info about = new Feinstaubplaketten_Programm_Info();
            about.Show();
        }

        //Datenbestand END------------------------------------------------------------
    }
    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}