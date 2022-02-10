using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;
using Sinoma_WCS;

namespace JY_Sinoma_WCS
{
    public partial class FormRFIDStatus : Form
    {
        public ConnectPool dbConn;
        public frmMain mainfrm;
        public FormRFIDStatus( frmMain mainfrm)
        {
            this.mainfrm = mainfrm;
            this.dbConn = mainfrm.dbConn;
            InitializeComponent();
            
        }

  

        private void FormDeviceStatus_Load(object sender, EventArgs e)
        {
            foreach (var row in mainfrm.ReadBarCodeFromSPs)
            {
                switch (int.Parse(row.Value.readerName))
                {
                    case 1:
                        if (row.Value.isRead)
                            rbAvailabel1.Checked = true;
                        else
                            rbStop1.Checked = true;
                        break;
                    case 2:
                        if (row.Value.isRead)
                            rbAvailabel2.Checked = true;
                        else
                            rbStop2.Checked = true;
                        break;
                    case 3:
                        if (row.Value.isRead)
                            rbAvailabel3.Checked = true;
                        else
                            rbStop3.Checked = true;
                        break;
                    case 4:
                        if (row.Value.isRead)
                            rbAvailabel4.Checked = true;
                        else
                            rbStop4.Checked = true;
                        break;
                    case 5:
                        if (row.Value.isRead)
                            rbAvailabel5.Checked = true;
                        else
                            rbStop5.Checked = true;
                        break;
                    case 6:
                        if (row.Value.isRead)
                            rbAvailabel6.Checked = true;
                        else
                            rbStop6.Checked = true;
                        break;
                    default:
                        break;
                }
            }
        }



        private void btChange1_Click(object sender, EventArgs e)
        {
            if (rbAvailabel1.Checked)
                mainfrm.ReadBarCodeFromSPs["1"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["1"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["1"].ScanStopRead();
            }
        }

        private void btChange2_Click(object sender, EventArgs e)
        {
            if (rbAvailabel2.Checked)
                mainfrm.ReadBarCodeFromSPs["2"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["2"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["2"].ScanStopRead();
            }
        }

        private void btChange3_Click(object sender, EventArgs e)
        {
            if (rbAvailabel3.Checked)
                mainfrm.ReadBarCodeFromSPs["3"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["3"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["3"].ScanStopRead();
            }
        }

        private void btChange4_Click(object sender, EventArgs e)
        {
            if (rbAvailabel4.Checked)
                mainfrm.ReadBarCodeFromSPs["4"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["4"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["4"].ScanStopRead();
            }
        }

        private void btChange5_Click(object sender, EventArgs e)
        {
            if (rbAvailabel5.Checked)
                mainfrm.ReadBarCodeFromSPs["5"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["5"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["5"].ScanStopRead();
            }
        }

        private void btChange6_Click(object sender, EventArgs e)
        {
            if (rbAvailabel6.Checked)
                mainfrm.ReadBarCodeFromSPs["6"].isRead = true;
            else
            {
                mainfrm.ReadBarCodeFromSPs["6"].isRead = false;
                mainfrm.ReadBarCodeFromSPs["6"].ScanStopRead();
            }
        }
    }
}
