//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
//   文 件 名：FormSiemens.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using NJIS.PLC.Communication.BasicFramework;
using NJIS.PLC.Communication.Core.Types;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
{
    public partial class FormSiemens : BaseForm
    {
        private readonly SiemensS7Net _siemensTcpNet;

        public FormSiemens(SiemensPLCS siemensPlcs)
        {
            InitializeComponent();
            _siemensTcpNet = new SiemensS7Net(siemensPlcs);
        }

        private void FormSiemens_Load(object sender, EventArgs e)
        {
            panel2.Enabled = false;
        }

        private void FormSiemens_FormClosing(object sender, FormClosingEventArgs e)
        {
            isThreadRun = false;
        }

        /// <summary>
        ///     统一的读取结果的数据解析，显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="address"></param>
        /// <param name="textBox"></param>
        private void readResultRender<T>(OperateResult<T> result, string address, TextBox textBox)
        {
            if (result.IsSuccess)
            {
                textBox.AppendText(DateTime.Now.ToString("[HH:mm:ss] ") +
                                   $"[{address}] {result.Content}{Environment.NewLine}");
            }
            else
            {
                MessageBox.Show(DateTime.Now.ToString("[HH:mm:ss] ") +
                                $"[{address}] 读取失败{Environment.NewLine}原因：{result.ToMessageShowString()}");
            }
        }

        /// <summary>
        ///     统一的数据写入的结果显示
        /// </summary>
        /// <param name="result"></param>
        /// <param name="address"></param>
        private void writeResultRender(OperateResult result, string address)
        {
            if (result.IsSuccess)
            {
                MessageBox.Show(DateTime.Now.ToString("[HH:mm:ss] ") + $"[{address}] 写入成功");
            }
            else
            {
                MessageBox.Show(DateTime.Now.ToString("[HH:mm:ss] ") +
                                $"[{address}] 写入失败{Environment.NewLine}原因：{result.ToMessageShowString()}");
            }
        }

        #region 报文读取测试

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                var read = _siemensTcpNet.ReadFromCoreServer(SoftBasic.HexStringToBytes(textBox13.Text));
                if (read.IsSuccess)
                {
                    textBox11.Text = "结果：" + SoftBasic.ByteToHexString(read.Content);
                }
                else
                {
                    MessageBox.Show("读取失败：" + read.ToMessageShowString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取失败：" + ex.Message);
            }
        }

        #endregion


        #region Connect And DisConnect

        private void button1_Click(object sender, EventArgs e)
        {
            // 
            IPAddress address;
            if (!IPAddress.TryParse(textBox1.Text, out address))
            {
                MessageBox.Show("Ip地址输入不正确！");
                return;
            }

            _siemensTcpNet.IpAddress = textBox1.Text;

            try
            {
                var connect = _siemensTcpNet.ConnectServer();
                if (connect.IsSuccess)
                {
                    MessageBox.Show("连接成功！");
                    button2.Enabled = true;
                    button1.Enabled = false;
                    panel2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("连接失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 断开连接
            _siemensTcpNet.ConnectClose();
            button2.Enabled = false;
            button1.Enabled = true;
            panel2.Enabled = false;
        }

        #endregion

        #region 单数据读取测试

        private void button_read_bool_Click(object sender, EventArgs e)
        {
            // 读取bool变量
            readResultRender(_siemensTcpNet.ReadBool(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_byte_Click(object sender, EventArgs e)
        {
            // 读取byte变量
            readResultRender(_siemensTcpNet.ReadByte(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_short_Click(object sender, EventArgs e)
        {
            // 读取short变量
            readResultRender(_siemensTcpNet.ReadInt16(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_ushort_Click(object sender, EventArgs e)
        {
            // 读取ushort变量
            readResultRender(_siemensTcpNet.ReadUInt16(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_int_Click(object sender, EventArgs e)
        {
            // 读取int变量
            readResultRender(_siemensTcpNet.ReadInt32(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_uint_Click(object sender, EventArgs e)
        {
            // 读取uint变量
            readResultRender(_siemensTcpNet.ReadUInt32(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_long_Click(object sender, EventArgs e)
        {
            // 读取long变量
            readResultRender(_siemensTcpNet.ReadInt64(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_ulong_Click(object sender, EventArgs e)
        {
            // 读取ulong变量
            readResultRender(_siemensTcpNet.ReadUInt64(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_float_Click(object sender, EventArgs e)
        {
            // 读取float变量
            readResultRender(_siemensTcpNet.ReadFloat(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_double_Click(object sender, EventArgs e)
        {
            // 读取double变量
            readResultRender(_siemensTcpNet.ReadDouble(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_string_Click(object sender, EventArgs e)
        {
            // 读取字符串
            readResultRender(_siemensTcpNet.ReadString(textBox3.Text), textBox3.Text,
                textBox4);
        }

        #endregion

        #region 单数据写入测试

        private void button24_Click(object sender, EventArgs e)
        {
            // bool写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, bool.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            // byte写入
            try
            {
                //byte[] buffer = new byte[500];
                //for (int i = 0; i < 500; i++)
                //{
                //    buffer[i] = (byte)i;
                //}
                //writeResultRender( siemensTcpNet.Write( textBox8.Text, buffer ), textBox8.Text );
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, byte.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            // short写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, short.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            // ushort写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, ushort.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button20_Click(object sender, EventArgs e)
        {
            // int写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, int.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // uint写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, uint.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // long写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, long.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // ulong写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, ulong.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // float写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, float.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // double写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, double.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button14_Click(object sender, EventArgs e)
        {
            // string写入
            try
            {
                writeResultRender(_siemensTcpNet.Write(textBox8.Text, textBox7.Text), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 批量读取测试

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                var read = _siemensTcpNet.Read(textBox6.Text, ushort.Parse(textBox9.Text));
                if (read.IsSuccess)
                {
                    //textBox10.Text = "结果：" + HslCommunication.BasicFramework.SoftBasic.ByteToHexString( read.Content );
                }
                else
                {
                    MessageBox.Show("读取失败：" + read.ToMessageShowString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取失败：" + ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // 订货号
            var read = _siemensTcpNet.ReadOrderNumber();
            if (read.IsSuccess)
            {
                textBox10.Text = "订货号：" + read.Content;
            }
            else
            {
                MessageBox.Show("读取失败：" + read.ToMessageShowString());
            }
        }

        #endregion

        #region 定时器读取测试

        // 外加曲线显示

        private Thread thread; // 后台读取的线程
        private int timeSleep = 300; // 读取的间隔
        private bool isThreadRun; // 用来标记线程的运行状态

        private void button27_Click(object sender, EventArgs e)
        {
            // 启动后台线程，定时读取PLC中的数据，然后在曲线控件中显示

            if (!isThreadRun)
            {
                if (!int.TryParse(textBox14.Text, out timeSleep))
                {
                    MessageBox.Show("间隔时间格式输入错误！");
                    return;
                }

                button27.Text = "停止";
                isThreadRun = true;
                thread = new Thread(ThreadReadServer);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                button27.Text = "启动";
                isThreadRun = false;
            }
        }

        private void ThreadReadServer()
        {
            while (isThreadRun)
            {
                Thread.Sleep(timeSleep);

                try
                {
                    var read = _siemensTcpNet.ReadInt16(textBox12.Text);
                    if (read.IsSuccess)
                    {
                        // 显示曲线
                        if (isThreadRun) Invoke(new Action<short>(AddDataCurve), read.Content);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取失败：" + ex.Message);
                }
            }
        }


        private void AddDataCurve(short data)
        {
            if (txtInterMsg.Text.Length > 10000)
            {
                txtInterMsg.Text = $"{DateTime.Now.ToString("HH:mm:ss")}->{data}";
            }
            else
            {
                txtInterMsg.AppendText($"{DateTime.Now.ToString("HH:mm:ss")}->{data}\r\n");
            }
        }

        #endregion
    }
}