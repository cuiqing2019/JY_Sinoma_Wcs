using DataBase;
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
    public partial class FormChangeHazardArea : Form
    {
        public FormChangeHazardArea()
        {
            InitializeComponent();
            pai1.SelectedIndex = 0;
            pai2.SelectedIndex = 0;
            pai3.SelectedIndex = 0;
            lie11.SelectedIndex = 0;
            lie21.SelectedIndex = 0;
            lie31.SelectedIndex = 0;
            lie12.SelectedIndex = 0;
            lie22.SelectedIndex = 0;
            lie32.SelectedIndex = 0;
            ceng1.SelectedIndex = 0;
            ceng2.SelectedIndex = 0;
            ceng3.SelectedIndex = 0;
            area1.SelectedIndex = 0;
            area2.SelectedIndex = 0;
            area3.SelectedIndex = 0;
        }

        private void btnChannel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (area1.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择正确的分区类型！");
                    return;
                }
                int num = 0;
                if (lie11.SelectedIndex == -1 && lie12.SelectedIndex != -1)
                    num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, 0, lie12.SelectedIndex + 1, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                else if (lie11.SelectedIndex != -1 && lie12.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, 0, lie11.SelectedIndex + 1, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                else if (lie11.SelectedIndex == -1 && lie12.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, 11, 23, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                else
                {
                    if (lie11.SelectedIndex > lie12.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, lie12.SelectedIndex + 1, lie11.SelectedIndex + 1, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                    else if (lie11.SelectedIndex < lie12.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, lie11.SelectedIndex + 1, lie12.SelectedIndex + 1, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                    else
                        num = DataBaseInterface.UpdateHazardArea(pai1.SelectedIndex + 1, lie11.SelectedIndex + 1, lie12.SelectedIndex + 1, ceng1.SelectedIndex + 1, area1.SelectedItem.ToString());
                }
                MessageBox.Show("修改完成，共修改" + num + "个库位");
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void btnChannel2_Click(object sender, EventArgs e)
        {
            try
            {
                if (area2.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择正确的分区类型！");
                    return;
                }
                int num = 0;
                if (lie21.SelectedIndex == -1 && lie22.SelectedIndex != -1)
                    num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1, 0, lie22.SelectedIndex + 1, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                else if (lie21.SelectedIndex != -1 && lie22.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1, 0, lie21.SelectedIndex + 1, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                else if (lie21.SelectedIndex == -1 && lie22.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1, 0, 23, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                else
                {
                    if (lie21.SelectedIndex > lie22.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1 + 2, lie22.SelectedIndex + 1, lie21.SelectedIndex + 1, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                    else if (lie21.SelectedIndex < lie22.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1 + 2, lie21.SelectedIndex + 1, lie22.SelectedIndex + 1, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                    else
                        num = DataBaseInterface.UpdateHazardArea(pai2.SelectedIndex + 1 + 2, lie21.SelectedIndex + 1, lie22.SelectedIndex + 1, ceng2.SelectedIndex + 1, area2.SelectedItem.ToString());
                }
                MessageBox.Show("修改完成，共修改" + num + "个库位");
            }
            catch (Exception)
            {

                throw;
            }
         
        }

        private void btnChannel3_Click(object sender, EventArgs e)
        {
            try
            {
                if (area3.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择正确的分区类型！");
                    return;
                }
                int num = 0;
                if (lie31.SelectedIndex == -1 && lie32.SelectedIndex != -1)
                    num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1, 0, lie32.SelectedIndex + 1, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                else if (lie31.SelectedIndex != -1 && lie32.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1, 0, lie31.SelectedIndex + 1, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                else if (lie31.SelectedIndex == -1 && lie32.SelectedIndex == -1)
                    num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1, 0, 33, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                else
                {
                    if (lie31.SelectedIndex > lie32.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1 + 4, lie32.SelectedIndex + 1, lie31.SelectedIndex + 1, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                    else if (lie31.SelectedIndex < lie32.SelectedIndex)
                        num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1 + 4, lie31.SelectedIndex + 1, lie32.SelectedIndex + 1, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                    else
                        num = DataBaseInterface.UpdateHazardArea(pai3.SelectedIndex + 1 + 4, lie31.SelectedIndex + 1, lie32.SelectedIndex + 1, ceng3.SelectedIndex + 1, area3.SelectedItem.ToString());
                }
                MessageBox.Show("修改完成，共修改" + num + "个库位");
            }
            catch (Exception)
            {

                throw;
            }
         
        }
    }
}
