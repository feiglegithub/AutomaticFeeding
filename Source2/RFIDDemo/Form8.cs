using CommomLY1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDDemo
{
    public partial class Form8 : Form
    {
        public int res, k, Detect, cmd;
        public int m_hScanner = -1, m_hSocket = -1, OK = 0;
        public int nBaudRate = 0, Interval, EPC_Word;
        public string szPort;
        public int HardVersion, SoftVersion;
        public int hwnd;
        public int nidEvent, mem, ptr, len;
        public int Read_times;
        public int m_antenna_sel;
        public byte Mask;
        public byte[] AccessPassword = new byte[4];
        public byte[] mask = new byte[96];
        public byte[] IDTemp = new byte[12];
        public byte[,] TagBuffer = new byte[100, 130];
        public byte[] AccessPassWord = new byte[4];
        public byte[,] TagNumber = new byte[100, 80];
        public string readerip;
        public uint readerport;
        public string hostip;
        public uint hostport;
        public byte connect_OK = 0;
        public int count_test = 0;
        public string ReaderIP;
        int ReaderPort;
        byte[] IPrecebuffer = new byte[64];
        int AutoPort;
        byte[] Epcrecebuffer = new byte[256];
        string[] EpcStr = new string[7];

        string connstr = "Persist Security Info =true; Password=!Q@W#E$R5t6y7u8i;User ID = sa ; Initial Catalog = HGWCSB; Data Source =10.30.3.117";
        string sql1_select = "";
        string NPalletID = "";
        string NPalletID_L = "";

        //停止扫描
        private void button4_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.timer2.Enabled = false;
            this.button3.Enabled = true;
            this.button4.Enabled = false;
        }

        //刷新DV视图
        private void timer2_Tick(object sender, EventArgs e)
        {
            var dt = SqlBase.GetSqlDs(sql1_select, connstr).Tables[0];
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Columns["ScanTime"].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            this.dataGridView1.Columns[0].Width = 80;
            this.dataGridView1.Columns[1].Width = 80;
            this.dataGridView1.Columns[2].Width = 100;
        }

        //命令模式下过滤
        public int StartBit;//起始地址bit
        public int DataLenth;//数据长度bit
        public string Datastr;//过滤数据
        public Form8()
        {
            InitializeComponent();
            this.Load += Form_Load;
            //this.Deactivate += Form_Deactivate;
            //this.Activated += Form_Activated;
            sql1_select = $"select top 50 * from RFID_ScanLog where InStation={this.txtInWareName.Text} and DATEDIFF(second,ScanTime,getdate())<=5 order by RSeqID desc";
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            this.timer2.Enabled = true;

            if (connect_OK == 0)
            {
                button1_Click(sender, e);
            }
        }

        private void Form_Deactivate(object sender, EventArgs e)
        {
            this.timer2.Enabled = false;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.button2.Enabled = false;

            this.button3.Enabled = true;
            this.button4.Enabled = false;
        }

        //连接设备
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.txtInWareName.Text == "")
            {
                MessageBox.Show("请输入对应的入口！");
                return;
            }

            readerip = this.textBox1.Text;
            readerport = Convert.ToUInt16(this.textBox2.Text);
            hostip = this.textBox3.Text;
            hostport = Convert.ToUInt16(this.textBox4.Text);

            //连接三次
            var res = 0;
            for (int i = 0; i < 3; i++)
            {
                res = Reader.Net_ConnectScanner(ref m_hSocket, readerip, readerport, hostip, hostport);
                if (res == 0) { break; }
            }

            if (res == 0)
            {
                //MessageBox.Show("扫码器连接成功！");
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                connect_OK = 1;

                button3_Click(sender, e);
            }
        }

        //断开连接
        private void button2_Click(object sender, EventArgs e)
        {
            //Reader.Net_DisconnectScanner();
            Reader.DisconnectScanner(m_hSocket);

            this.button1.Enabled = true;
            this.button2.Enabled = false;
            connect_OK = 0;
        }

        //开始扫描
        private void button3_Click(object sender, EventArgs e)
        {
            int be_antenna;
            //byte[] DB = new byte[128];
            //byte[] IDBuffer = new byte[7680];
            //byte[,] chArray = new byte[100, 2600];
            //byte[] temp = new byte[10 * 260];

            Read_times++;

            be_antenna = 0;

            be_antenna = 1;

            res = Reader.Net_SetAntenna(m_hSocket, be_antenna);

            ListViewItem item = new ListViewItem();

            Array.Clear(TagBuffer, 0, TagBuffer.Length);
            count_test++;

            //设置天线
            res = Reader.Net_SetAntenna(m_hSocket, 1);

            this.timer1.Enabled = true;
            this.timer2.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = true;
        }

        //16进制转字符串
        private string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        //读取托盘号
        private void timer1_Tick(object sender, EventArgs e)
        {
            int i, j;
            string str, strtemp;
            byte[] DB = new byte[128];
            byte[] IDBuffer = new byte[7680];
            byte[,] chArray = new byte[100, 2600];
            byte[] temp = new byte[10 * 260];
            ListViewItem item = new ListViewItem();
            int nCounter = 0, ID_len = 0, ID_len_temp = 0, success;
            string str_temp;

            //读EPC标签
            res = Reader.Net_EPC1G2_ReadLabelID(m_hSocket, mem, ptr, len, mask, IDBuffer, ref nCounter);
            if (res == OK)
            {
                //int kk = 0;
                for (i = 0; i < nCounter; i++)
                {
                    ID_len_temp = IDBuffer[ID_len] * 2 + 1;

                    for (j = 0; j < ID_len_temp; j++)
                    {
                        TagBuffer[i, j] = IDBuffer[ID_len + j];
                    }
                    ID_len += ID_len_temp;
                }

                for (i = 0; i < nCounter; i++)
                {
                    str = "";
                    strtemp = "";
                    ID_len = TagBuffer[i, 0] * 2;
                    for (j = 0; j < ID_len; j++)
                    {
                        strtemp = TagBuffer[i, j + 1].ToString("X2");
                        str += strtemp;
                    }

                    for (j = 0; j < k; j++)
                    {
                        strtemp = listView1.Items[j].SubItems[2].Text;
                        ID_len_temp = Convert.ToInt32(strtemp, 16) * 2;
                        if (ID_len == ID_len_temp)
                        {
                            str_temp = listView1.Items[j].SubItems[1].Text;
                            if (str == str_temp)
                            {
                                success = Convert.ToInt32(listView1.Items[j].SubItems[3].Text) + 1;
                                listView1.Items[j].SubItems[3].Text = success.ToString();
                                break;
                            }
                        }
                    }
                    if (j == k)
                    {
                        item = listView1.Items.Add((k + 1).ToString(), k);
                        item.SubItems.Add(str);
                        item.SubItems.Add(TagBuffer[i, 0].ToString("X2"));
                        success = 1;
                        item.SubItems.Add(success.ToString());
                        item.SubItems.Add(count_test.ToString());
                        k++;
                    }
                }
            }
            for (j = 0; j < k; j++)
            {
                listView1.Items[j].SubItems[4].Text = count_test.ToString();
            }

            //////////////////////////////////////////////////////////////////////////
            //把EPC标签转换成托盘号
            comboBox3.Items.Clear();
            byte[] dada = new byte[1024];
            for (int ii = 0; ii < k; ii++)
            {
                str = listView1.Items[ii].SubItems[2].Text;
                // TagBuffer[ii, 0] = Convert.ToByte(str, 16);
                str = listView1.Items[ii].SubItems[1].Text;
                for (int jj = 0; jj < str.Length / 2; jj++)
                {
                    strtemp = str[jj * 2].ToString() + str[jj * 2 + 1].ToString();
                    // TagBuffer[ii, jj + 1] = (byte)Convert.ToInt16(strtemp, 16);
                }
                strtemp = "";
                strtemp = (ii + 1).ToString("D2") + ".";
                comboBox3.Items.Add(strtemp + str);
            }
            if (k != 0)
            {
                comboBox3.SelectedIndex = 0;
            }

            if (comboBox3.Text != "")
            {
                for (i = 0; i < 4; i++)
                {
                    AccessPassWord[i] = Convert.ToByte("00000000".Substring(i * 2, 2), 16);
                }
                EPC_Word = TagBuffer[comboBox3.SelectedIndex, 0];
                for (i = 0; i < TagBuffer[comboBox3.SelectedIndex, 0] * 2; i++)
                {
                    IDTemp[i] = TagBuffer[comboBox3.SelectedIndex, i + 1];
                }

                //通过标签读取托盘号
                res = Reader.Net_EPC1G2_ReadWordBlock(m_hSocket, 6, IDTemp, 1, 4, 6, DB, AccessPassWord);
                if (res == OK)
                {
                    str = "";
                    for (i = 0; i < 12; i++)
                    {
                        strtemp = DB[i].ToString("X2");
                        str += strtemp;
                    }

                    NPalletID = HexStringToString(str, Encoding.UTF8).Replace("\0", "");
                    //if (NPalletID_L == NPalletID) { return; }
                    if (this.txtInWareName.Text == "2015")
                    {
                        var sql1 = $"insert RFID_ScanLog(InStation,NPallet,ScanTime)values({this.txtInWareName.Text},'{NPalletID}',GETDATE())";
                        SqlBase.ExecSql(sql1, connstr);
                    }
                    //NPalletID_L = NPalletID;
                }
            }
        }

        #region ErrorInformation
        public void ReportError(ref string temp)
        {
            switch (res)
            {
                case 1:
                    temp = "Connect antenna fail!";
                    break;
                case 2:
                    temp = "No Tag!";
                    break;
                case 3:
                    temp = "Illegal Tag!";
                    break;
                case 4:
                    temp = "Power is not enough!";
                    break;
                case 5:
                    temp = "The memory has been protected!";
                    break;
                case 6:
                    temp = "Check sum error!";
                    break;
                case 7:
                    temp = "Parameter error!";
                    break;
                case 8:
                    temp = "The memory don't exist!";
                    break;
                case 9:
                    temp = "The Access Password is error!";
                    break;
                case 10:
                    temp = "The Kill Password cannot be 000000!";
                    break;
                case 14:
                    temp = "Locked Tags in the field!";
                    break;
                case 30:
                    temp = "Invalid Command!";
                    break;
                case 31:
                    temp = "Other Error!";
                    break;
                default:
                    temp = "Unbeknown Error!";
                    break;
            }
        }
        #endregion
    }
}
