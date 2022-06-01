//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
//   文 件 名：MainForm.cs
//   创建时间：2018-11-12 17:58
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:58
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.Log;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
{
    public partial class MainForm : BaseForm
    {
        private readonly ILogger _logger = LogManager.GetLogger<MainForm>();
        private readonly SiemensS7Net _siemensTcpNet;
        private bool _isR;
        private int _isRFreq = 100;
        private bool _isW;
        private int _isWFreq = 100;

        private readonly ConcurrentQueue<string> _msgQueues = new ConcurrentQueue<string>();

        private readonly List<long> _readCalculate = new List<long>();
        private readonly List<long> _writeCalculate = new List<long>();

        public MainForm()
        {
            InitializeComponent();
            _siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500) {ConnectTimeOut = 5000};
        }

        public bool ReadString { get; set; }
        public bool ReadInt { get; set; }
        public bool ReadLInt { get; set; }
        public bool Readbool { get; set; }
        public bool ReadReal { get; set; }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cb_bool.DataBindings.Add("Checked", this, "Readbool");
            cb_string.DataBindings.Add("Checked", this, "ReadString");
            cb_int.DataBindings.Add("Checked", this, "ReadInt");
            cb_lint.DataBindings.Add("Checked", this, "ReadLInt");
            cb_real.DataBindings.Add("Checked", this, "ReadReal");
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var log = "";

                    if (_msgQueues.TryDequeue(out log))
                    {
                        txtMessage.Invoke((Action) (() =>
                        {
                            if (txtMessage.Text.Length > 10000)
                            {
                                txtMessage.Text = log;
                            }
                            else
                            {
                                txtMessage.Text += log;
                            }
                        }));
                    }

                    Thread.Sleep(10);
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_isR)
                    {
                        //lock (rwLock)
                        {
                            ReadPlc();
                            //ReadPlcForAll();
                        }
                    }

                    Thread.Sleep(_isRFreq);
                }
            }, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_isW)
                    {
                        //lock (rwLock)
                        {
                            WritePlc();
                        }
                    }

                    Thread.Sleep(_isWFreq);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void ReadPlc()
        {
            var sw = new Stopwatch();
            sw.Reset();
            WriteLog("=====================开始读取PLC=====================");
            sw.Start();
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadString)
                {
                    WriteLog("======读取string=====");
                    foreach (var item in lbPlcStringVars.Text.Trim().Split('\r'))
                    {
                        var rst = _siemensTcpNet.ReadString(item, 256);
                        //siemensTcpNet.read
                        if (rst.IsSuccess)
                        {
                            WriteLog("read", item, rst.Content.TrimEnd('\0').Trim());
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadInt)
                {
                    WriteLog("======读取short=====");
                    foreach (var item in lbPlcIntVars.Text.Trim().Split('\r'))
                    {
                        var rst = _siemensTcpNet.ReadInt16(item);
                        if (rst.IsSuccess)
                        {
                            WriteLog("read", item, rst.Content + "");
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadLInt)
                {
                    WriteLog("======读取lint=====");
                    foreach (var item in lbPlcLintVars.Text.Trim().Split('\r'))
                    {
                        var rst = _siemensTcpNet.ReadInt32(item);
                        if (rst.IsSuccess)
                        {
                            WriteLog("read", item, rst.Content + "");
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (Readbool)
                {
                    WriteLog("======读取bool=====");
                    foreach (var item in lbPlcBoolVars.Text.Trim().Split('\r'))
                    {
                        var rst = _siemensTcpNet.ReadBool(item);
                        if (rst.IsSuccess)
                        {
                            WriteLog("read", item, rst.Content.ToString());
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadReal)
                {
                    WriteLog("======读取real=====");
                    foreach (var item in lbPlcRealVars.Text.Trim().Split('\r'))
                    {
                        var rst = _siemensTcpNet.ReadFloat(item);
                        if (rst.IsSuccess)
                        {
                            WriteLog("read", item, rst.Content.ToString());
                        }
                    }
                }
            }));
            Task.WaitAll(tasks.ToArray());
            _readCalculate.Add(sw.ElapsedMilliseconds);
            WriteLog($"=====================读取完成，共耗时:{sw.ElapsedMilliseconds}=====================");
        }

        private void ReadPlcForAll()
        {
            var sw = new Stopwatch();
            sw.Reset();
            WriteLog("=====================开始批量都PLC=====================");
            sw.Start();
            var rs = _siemensTcpNet.ReadBytes("DB1.0", 3146);
            //sw.Connect();
            //sw.Stop();
            //WriteLog($"=====================读取时间{sw.ElapsedMilliseconds}=====================");
            if (rs.IsSuccess)
            {
                var i = 0;
                var bts = rs.Content;
                foreach (var item in lbPlcStringVars.Text.Trim().Split('\r'))
                {
                    var str = Encoding.ASCII.GetString(bts, i, 256);
                    i = i + 256;
                    WriteLog("read", item, str.TrimEnd('\0'));
                }

                foreach (var item in lbPlcIntVars.Text.Trim().Split('\r'))
                {
                    var val = _siemensTcpNet.ByteTransform.TransInt16(bts, i);
                    i = i + 2;
                    WriteLog("read", item, val + "");
                }

                foreach (var item in lbPlcLintVars.Text.Trim().Split('\r'))
                {
                    var val = _siemensTcpNet.ByteTransform.TransInt32(bts, i);
                    i = i + 8;
                    WriteLog("read", item, val + "");
                }

                var bt = bts[i];

                var q = 0;
                var bs = _siemensTcpNet.ByteTransform.TransBool(bts, q, 1);

                var bi = 0;
                foreach (var item in lbPlcBoolVars.Text.Trim().Split('\r'))
                {
                    WriteLog("read", item, bs[bi++] + "");
                }

                i = i + 2;

                foreach (var item in lbPlcRealVars.Text.Trim().Split('\r'))
                {
                    var val = _siemensTcpNet.ByteTransform.TransSingle(bts, i);
                    i = i + 4;
                    WriteLog("read", item, val + "");
                }
            }

            sw.Stop();
            WriteLog($"=====================完成批量读取{sw.ElapsedMilliseconds}=====================");
        }

        private void WritePlc()
        {
            var sw = new Stopwatch();
            sw.Reset();
            WriteLog("=====================开始写入PLC=====================");
            sw.Start();
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadString)
                {
                    WriteLog("======写入string=====");
                    foreach (var item in lbPlcStringVars.Text.Trim().Split('\r'))
                    {
                        var val = DateTime.Now.ToString("HH:mm:ss:fff");
                        var rst = _siemensTcpNet.Write(item, val);
                        if (rst.IsSuccess)
                        {
                            WriteLog("write", item, val);
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadInt)
                {
                    WriteLog("======写入short=====");
                    foreach (var item in lbPlcIntVars.Text.Trim().Split('\r'))
                    {
                        var val = DateTime.Now.Millisecond;
                        var rst = _siemensTcpNet.Write(item, (short) val);
                        if (rst.IsSuccess)
                        {
                            WriteLog("write", item, val.ToString());
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadLInt)
                {
                    WriteLog("======写入Lint=====");
                    foreach (var item in lbPlcLintVars.Text.Trim().Split('\r'))
                    {
                        var val = DateTime.Now.Millisecond;
                        var rst = _siemensTcpNet.Write(item, val);
                        if (rst.IsSuccess)
                        {
                            WriteLog("write", item, val.ToString());
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (Readbool)
                {
                    WriteLog("======写入bool=====");
                    foreach (var item in lbPlcBoolVars.Text.Trim().Split('\r'))
                    {
                        var val = DateTime.Now.Millisecond % 2 == 1;
                        var rst = _siemensTcpNet.Write(item, val);
                        if (rst.IsSuccess)
                        {
                            WriteLog("write", item, val.ToString());
                        }
                    }
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                if (ReadReal)
                {
                    WriteLog("======写入real=====");
                    foreach (var item in lbPlcRealVars.Text.Trim().Split('\r'))
                    {
                        var val = (float) (DateTime.Now.Millisecond / 2.0);
                        var rst = _siemensTcpNet.Write(item, val);
                        if (rst.IsSuccess)
                        {
                            WriteLog("write", item, val.ToString());
                        }
                    }
                }
            }));

            Task.WaitAll(tasks.ToArray());
            _writeCalculate.Add(sw.ElapsedMilliseconds);
            WriteLog($"=====================写入完成，共耗时:{sw.ElapsedMilliseconds}=====================");
        }

        private void WriteLog(string cap, string varName, string val)
        {
            WriteLog($"{cap}:{varName}={val}");
        }

        private void WriteLog(string varName, string val)
        {
            WriteLog($"{varName}={val}");
        }

        private void WriteLog(string log)
        {
            var msg = $"{DateTime.Now.ToString("HH:mm:ss:fff")}->{log}" + "\r\n";
            _logger.Info(msg);
            _msgQueues.Enqueue(msg);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_siemensTcpNet == null) return;
            if (btnConnect.Text == @"连接")
            {
                _siemensTcpNet.IpAddress = txtIp.Text.Trim();
                _siemensTcpNet.Port = int.Parse(txtPort.Text.Trim());
                _siemensTcpNet.ConnectServer();
                WriteLog($"连接PLC：{_siemensTcpNet.IpAddress}");
                btnConnect.Text = @"断开";
            }
            else
            {
                _siemensTcpNet.ConnectClose();
                WriteLog($"断开PLC：{_siemensTcpNet.IpAddress}");
                btnConnect.Text = @"连接";
            }
        }

        private void btnReadPlcVarToString_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text != @"断开") return;
            try
            {
                _isRFreq = int.Parse(txtPlcVarStringReadInteral.Text.Trim());
            }
            catch (Exception)
            {
                MessageBox.Show(@"读取频率格式有误！");
            }

            if (btnReadPlcVarToString.Text == @"读取(>>)")
            {
                btnReadPlcVarToString.Text = @"读取(<>)";
                //ReadCalculate.Clear();
            }
            else
            {
                btnReadPlcVarToString.Text = @"读取(>>)";
            }

            _isR = btnReadPlcVarToString.Text != @"读取(>>)";
        }


        private void btnWritePlcVarToString_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text != @"断开") return;
            try
            {
                _isWFreq = int.Parse(txtPlcVarStringWriteInteral.Text.Trim());
            }
            catch (Exception)
            {
                MessageBox.Show(@"写入频率格式有误！");
            }

            if (btnWritePlcVarToString.Text == @"写入(>>)")
            {
                btnWritePlcVarToString.Text = @"写入(<>)";
                //WriteCalculate.Clear();
            }
            else
            {
                btnWritePlcVarToString.Text = @"写入(>>)";
            }

            _isW = btnWritePlcVarToString.Text != @"写入(>>)";
        }

        private void btnReadOnce_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text != @"断开") return;
            ReadPlc();
        }

        private void btnWriteOnce_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text != @"断开") return;
            WritePlc();
        }

        private void btnClearMsg_Click(object sender, EventArgs e)
        {
            while (_msgQueues.Count > 0)
            {
                var mst = "";
                _msgQueues.TryDequeue(out mst);
            }

            txtMessage.Text = "";
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            WriteLog($"************读取次数{_readCalculate.Count}*******");
            if (_readCalculate.Count > 0)
            {
                WriteLog($"最大耗时{_readCalculate.Max()}");
                WriteLog($"最小耗时{_readCalculate.Min()}");
                WriteLog($"平均耗时{_readCalculate.Average()}");
                WriteLog($"***********************************************");
            }

            if (_writeCalculate.Count > 0)
            {
                WriteLog($"*************写入次数{_writeCalculate.Count}*******");
                WriteLog($"最大耗时{_writeCalculate.Max()}");
                WriteLog($"最小耗时{_writeCalculate.Min()}");
                WriteLog($"平均耗时{_writeCalculate.Average()}");
                WriteLog($"***********************************************");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadPlcForAll();
        }

        /// <summary>
        ///     获取数据中某一位的值
        /// </summary>
        /// <param name="input">传入的数据类型,可换成其它数据类型,比如Int</param>
        /// <param name="index">要获取的第几位的序号,从0开始</param>
        /// <returns>返回值为-1表示获取值失败</returns>
        private int GetbitValue(byte input, int index)
        {
            if (index > sizeof(byte))
            {
                return -1;
            }

            //左移到最高位
            var value = input << (sizeof(byte) - 1 - index);
            //右移到最低位
            value = value >> (sizeof(byte) - 1);
            return value;
        }
    }
}