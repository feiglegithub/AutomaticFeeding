using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ReaderDemo
{
    public partial class MainForm : Form
    {
        Program Program = new Program();
        public int res, k, Detect, cmd;
        public int m_hScanner = -1, m_hSocket = -1, OK = 0;
        public int nBaudRate = 0, Interval, EPC_Word;
        public string szPort;
        public int HardVersion, SoftVersion;
        public int hwnd;
        public int nidEvent, mem, ptr, len;
        public int Read_times;
        public int m_antenna_sel, RS485Address = 0;
        public byte Mask;
        public byte[] AccessPassword = new byte[4];
        public byte[] mask = new byte[96];
        public byte[] IDTemp = new byte[12];
        public byte[,] TagBuffer = new byte[100, 130];
        public byte[] AccessPassWord = new byte[4];
        public byte[,] TagNumber = new byte[100, 80];
        public int ComMode;
        public string readerip;
        public uint readerport;
        public string hostip;
        public uint hostport;
        public byte connect_OK = 0;
        public int count_test = 0;
        public Socket NetSocket = null;
        public string ReaderIP;
        public int ReaderPort;
        public byte[] IPrecebuffer = new byte[64];

        public Socket EpcSocket = null;
        public int AutoPort;
        public byte[] Epcrecebuffer = new byte[256];
        public string[] EpcStr = new string[7];

        //自动模式 标签是否只读一次
        List<string> epcOnly = new List<string>();

        //命令模式下过滤
        public int StartBit;//起始地址bit
        public int DataLenth;//数据长度bit
        public string Datastr;//过滤数据

        public delegate void NetInvoke();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;

            string strtemp = "";
            double Freq = 0, jumpFreq = 0, temp = 0;


            button27.Enabled = false;

            textBox37.Text = "192.168.1.253";

            Freq = 865.00;
            jumpFreq = 0.500;

            for (i = 0; i < 7; i++)
            {
                temp = Freq + i * jumpFreq;
                strtemp = string.Format("{0,0:D}{1,0:s}{2,7:F03}", i, ":", temp);
                
            }

            Freq = 902.00;
            jumpFreq = 0.500;

            for (i = 6; i < 59; i++)
            {
                temp = Freq + (i - 6) * jumpFreq;
                strtemp = string.Format("{0,0:D}{1,0:s}{2,7:F03}", i + 1, ":", temp);
          
            }

        }

        #region List Tag ID
        private void button3_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0)
                return;
            nidEvent = 1;
            string str, strtemp;

             mem = 1;


            //过滤记录

            StartBit = Convert.ToInt32(textBox1.Text.Trim());

            DataLenth = Convert.ToInt16(textBox2.Text.Trim());

            if (StartBit != 0)
            {
                StartBit = StartBit / 4;
            }

            if (DataLenth != 0)
            {
                DataLenth = DataLenth / 4;
            }

 

            if (StartBit == 0 && DataLenth == 0)
            {
                Datastr = "";
            }

            if (button3.Text == "List Tag ID")
            {
                button3.Text = "Stop";
                Read_times = 0;
                k = 0;
                count_test = 0;
                listView1.Items.Clear();
                timer1.Interval = 100;
                timer1.Enabled = true;
            }
            else
            {
                button3.Text = "List Tag ID";
                timer1.Enabled = false;
                comboBox3.Items.Clear();

                byte[] dada = new byte[1024];
                for (int i = 0; i < k; i++)
                {
                    str = listView1.Items[i].SubItems[2].Text;
                    TagBuffer[i, 0] = Convert.ToByte(str, 16);
                    str = listView1.Items[i].SubItems[1].Text;
                    for (int j = 0; j < str.Length / 2; j++)
                    {
                        strtemp = str[j * 2].ToString() + str[j * 2 + 1].ToString();
                        TagBuffer[i, j + 1] = (byte)Convert.ToInt16(strtemp, 16);
                    }
                    strtemp = "";
                    strtemp = (i + 1).ToString("D2") + ".";
                    comboBox3.Items.Add(strtemp + str);

                }
                if (k != 0)
                {
                    comboBox3.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region Timer
        unsafe private void timer1_Tick(object sender, EventArgs e)
        {
            int i, j, nCounter = 0, ID_len = 0, ID_len_temp = 0, be_antenna, success;
            string str, str_temp, strtemp;
            byte[] DB = new byte[128];
            byte[] IDBuffer = new byte[7680];
            byte[,] chArray = new byte[100, 2600];
            byte[] temp = new byte[10 * 260];

            Read_times++;
   
            be_antenna = 0;

            if (checkBox1.Checked == true)
            {
                be_antenna = 1;
                switch (ComMode)
                {
                    case 1:
                        res = Program.Net_SetAntenna(m_hSocket, be_antenna);
                        break;
                }
            }

            ListViewItem item = new ListViewItem();
            switch (nidEvent)
            {
                case 1:
                    if (be_antenna != 0)
                    {
                        Array.Clear(TagBuffer, 0, TagBuffer.Length);
                        count_test++;
                        switch (ComMode)
                        {
                            case 1:
                                res = Program.Net_EPC1G2_ReadLabelID(m_hSocket, mem, ptr, len, mask, IDBuffer, ref nCounter);
                                break;

                        }
                        if (res == OK)
                        {
                            int kk = 0;
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
                            listView1.Items[j].SubItems[4].Text = count_test.ToString();
                    }
                    break;
                case 2:
                    if (be_antenna != 0)
                    {
                        switch (ComMode)
                        {
                            case 1:
                                res = Program.Net_EPC1G2_ReadWordBlock(m_hSocket, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), Convert.ToByte(textBox5.Text), Convert.ToByte(textBox6.Text), DB, AccessPassWord);
                                break;
                        }
                        if (res == OK)
                        {
                            str = "";
                            for (i = 0; i < Convert.ToByte(textBox6.Text) * 2; i++)
                            {
                                strtemp = DB[i].ToString("X2");
                                str += strtemp;
                            }
                            //listBox1.Items.Add(str);
                            listBox1.Items.Add(HexStringToString(str, Encoding.UTF8));
                        }
                    }
                    break;
            }
        }
        #endregion

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

        #region Read
        private void button5_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0)
                return;
            int i;
            string str;
            nidEvent = 2;
            if (comboBox3.SelectedIndex < 0)
            {
                MessageBox.Show("Please identify a tag first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Convert.ToInt16(textBox5.Text) < 0)
            {
                MessageBox.Show("Please input start address of tag more then 0!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox5.Focus();
                textBox5.SelectAll();
                return;
            }
            if (Convert.ToInt16(textBox6.Text) < 1)
            {
                MessageBox.Show("Please input length of data more than 1!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox6.Focus();
                textBox6.SelectAll();
                return;
            }
            if (textBox7.Text.Length != 8)
            {
                MessageBox.Show("Please input correct accesspassword!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox7.Focus();
                textBox7.SelectAll();
                return;
            }
            str = textBox7.Text;
            for (i = 0; i < 4; i++)
            {
                AccessPassWord[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }

            EPC_Word = TagBuffer[comboBox3.SelectedIndex, 0];
            for (i = 0; i < TagBuffer[comboBox3.SelectedIndex, 0] * 2; i++)
            {
                IDTemp[i] = TagBuffer[comboBox3.SelectedIndex, i + 1];
            }


            mem = 1;

            if (button5.Text == "Read")
            {
                m_antenna_sel = 0;
                if (checkBox1.Checked == true)
                    m_antenna_sel += 1;

                switch (m_antenna_sel)
                {
                    case 0:
                        MessageBox.Show("Please choose one antenna at least!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        button5.Text = "Read";
                        return;
                    case 1:
                    case 2:
                    case 4:
                    case 8:
                        for (i = 0; i < 3; i++)
                        {
                            switch (ComMode)
                            {
                                case 0:
                                    res = Program.SetAntenna(m_hScanner, m_antenna_sel, RS485Address);
                                    break;
                                case 1:
                                    res = Program.Net_SetAntenna(m_hSocket, m_antenna_sel);
                                    break;
                            }
                            if (res == OK)
                                break;

                        }
                        if (res != 0)
                        {
                            MessageBox.Show("Fail to set antenna!Please try again!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            button5.Text = "Read";
                            return;
                        }
                        break;
                }
                button5.Text = "Stop";
                Read_times = 0;
                k = 0;
                listBox1.Items.Clear();
                timer1.Interval = 100;
                timer1.Enabled = true;
            }
            else
            {
                button5.Text = "Read";
                timer1.Enabled = false;
            }
        }
        #endregion

        //连接设备
        private void button26_Click(object sender, EventArgs e)
        {
            int i;
            byte HardVer;
            readerip = textBox35.Text;
            readerport = Convert.ToUInt16(textBox36.Text);
            hostip = textBox37.Text;
            hostport = Convert.ToUInt16(textBox38.Text);
            if (radioButton42.Checked == true)
                ComMode = 1;
            else
                ComMode = 0;
            for (i = 0; i < 3; i++)
            {
                switch (ComMode)
                {
                    case 1:
                        res = Program.Net_ConnectScanner(ref m_hSocket, readerip, readerport, hostip, hostport);
                        break;
                }
                if (res == OK)
                    break;
            }

            if ((res != OK) && (ComMode == 0))
            {
                MessageBox.Show("None Reader connect to the COM!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if ((res != OK) && (ComMode == 1))
            {
                MessageBox.Show("None Reader connect to the RJ45!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            for (i = 0; i < 3; i++)
            {
                switch (ComMode)
                {
                    case 1:
                        res = Program.Net_GetReaderVersion(m_hSocket, ref HardVersion, ref SoftVersion);
                        break;
                }
                if (res == OK)
                    break;
            }
            if (res != OK)
            {
                connect_OK = 0;
                MessageBox.Show("Can't get reader version!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                connect_OK = 1;
                MessageBox.Show("Connect reader success!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            HardVer = (byte)HardVersion;
            button26.Enabled = false;
            button27.Enabled = true;
            this.tabControl1.SelectedIndex = 1;
        }

        //断开连接
        private void button27_Click(object sender, EventArgs e)
        {
            Program.Net_DisconnectScanner();
            button26.Enabled = true;
            button27.Enabled = false;
        }
    }
}