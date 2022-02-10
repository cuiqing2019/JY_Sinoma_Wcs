using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JY_Sinoma_WCS
{
    public partial class FormInternet : Form
    {
        private SystemStatus systemStatus;
        public FormInternet(SystemStatus systemStatus)
        {
            InitializeComponent();
            this.systemStatus = systemStatus;
        }

        private void FormStation_Load(object sender, EventArgs e)
        {
            if (systemStatus.internetStruct.mainDP > 0)
            {
                this.checkSource.Checked = true;
            }
               
            if (systemStatus.internetStruct.subDP1 == 1)
                this.cbSub1.Checked = true;
            if (systemStatus.internetStruct.subDP2 == 1)
                this.cbSub2.Checked = true;
            if (systemStatus.internetStruct.subDP3 == 1)
                this.cbSub3.Checked = true;
            if (systemStatus.internetStruct.subDP4 == 1)
                this.cbSub4.Checked = true;
            if (systemStatus.internetStruct.subDP == 1)
                this.cbSub5.Checked = true;
        }
       
    }
}
