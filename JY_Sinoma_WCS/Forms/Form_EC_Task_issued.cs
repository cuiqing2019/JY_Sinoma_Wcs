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
    public partial class Form_EC_Task_issued : Form
    {
        public Stacker SK;
        public Form_EC_Task_issued(Stacker SK)
        {
            this.SK= SK;
            InitializeComponent();
            this.Text = SK.deviceName + "出库辊道下任务故障！"; 
        }

        private void Form_EC_Task_issued_Load(object sender, EventArgs e)
        {
            label1.Text=SK.deviceName +SK.statusStruct.taskId+ "号出库任务，出库辊道下任务故障！"; 
        }
    }
}
