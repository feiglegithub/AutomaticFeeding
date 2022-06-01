using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCSToWMSDemo
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            var dt = DemoSql.GetDdjStatus();

            foreach(DataRow dr in dt.Rows)
            {
                var no = dr["No"].ToString();
                var status = dr["Status"].ToString();
                switch (no)
                {
                    case "1":
                        this.btn1.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "2":
                        this.btn2.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "3":
                        this.btn3.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "4":
                        this.btn4.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "5":
                        this.btn5.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "6":
                        this.btn6.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "7":
                        this.btn7.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "8":
                        this.btn8.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "9":
                        this.btn9.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "10":
                        this.btn10.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    case "11":
                        this.btn11.BackColor = status == "1" ? Color.LimeGreen : Color.Red;
                        break;
                    default:
                        break;
                }
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var no = btn.Tag.ToString();
            var msg = "";

            if (btn.BackColor == Color.LimeGreen)
            {
                msg = DemoSql.UpdateScStatus(btn.Text, 0, no);
                if (msg.Length == 0)
                {
                    btn.BackColor = Color.Red;
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
            else
            {
                msg = DemoSql.UpdateScStatus(btn.Text, 1, no);
                if (msg.Length == 0)
                {
                    btn.BackColor = Color.LimeGreen;
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }
    }
}
