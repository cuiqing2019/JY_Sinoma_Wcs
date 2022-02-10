using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JY_Sinoma_WCS.Forms
{
    public partial class FormEmptyPalletSet : Form
    {
        public frmMain mainFrm;
        public FormEmptyPalletSet(frmMain mainFrm)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
        }

        private void FormEmptyPalletSet_Load(object sender, EventArgs e)
        {
            if (mainFrm.emptyPalletType == 1)
                lbCurrentType.Text = "当前模式：自动回库模式";
            else if(mainFrm.emptyPalletType==2)
                lbCurrentType.Text = "当前模式：人工回库模式";
            else
                lbCurrentType.Text = "当前模式：无";

        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            mainFrm.emptyPalletType = 1;
            lbCurrentType.Text = "当前模式：自动回库模式";
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            mainFrm.emptyPalletType = 2;
            lbCurrentType.Text = "当前模式：人工回库模式";
        }
    }
}
