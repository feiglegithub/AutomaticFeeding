//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Simulator
//   文 件 名：RadForm1.cs
//   创建时间：2019-03-26 15:43
//   作    者：
//   说    明：
//   修改时间：2019-03-26 15:43
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Service;
using NJIS.FPZWS.UI.Common;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.Drilling.Simulator
{
    public partial class RadForm1 : BaseForm
    {
        private readonly SiemensS7Net _siemensTcpNet;


        private bool pos_11_12 = true;

        private List<PartPath> PPS = new List<PartPath>();

        public Dictionary<int, TriggerPoint> TPS = new Dictionary<int, TriggerPoint>();

        public RadForm1(SiemensPLCS siemensPlcs)
        {
            InitializeComponent();
            _siemensTcpNet = new SiemensS7Net(siemensPlcs);

            this[10] = new TriggerPoint("DB450.22", "DB450.76", "DB450.0.20") {TextBoxCtl = txtPartId10};
            this[11] = new TriggerPoint("DB450.222", "DB450.276", "DB450.198.20") {TextBoxCtl = txtPartId11};
            this[20] = new TriggerPoint("DB450.1022", "DB450.1026", "DB450.998.20") {TextBoxCtl = txtPartId20};
            this[21] = new TriggerPoint("DB450.1122", "DB450.1126", "DB450.1098.20") {TextBoxCtl = txtPartId21};
            this[22] = new TriggerPoint("DB450.1222", "DB450.1226", "DB450.1198.20") {TextBoxCtl = txtPartId22};
            this[23] = new TriggerPoint("DB450.1322", "DB450.1326", "DB450.1298.20") {TextBoxCtl = txtPartId23};
            this[24] = new TriggerPoint("DB450.1422", "DB450.1426", "DB450.1398.20") {TextBoxCtl = txtPartId24};
            this[25] = new TriggerPoint("DB450.1522", "DB450.1526", "DB450.1498.20") {TextBoxCtl = txtPartId25};
            this[26] = new TriggerPoint("DB450.1622", "DB450.1626", "DB450.1598.20") {TextBoxCtl = txtPartId26};

            this[30] = new TriggerPoint("DB450.1822", "DB450.1826", "DB450.1798.20") {TextBoxCtl = txtPartId30};
            this[31] = new TriggerPoint("DB450.1922", "DB450.1926", "DB450.1898.20") {TextBoxCtl = txtPartId31};
            //this[32] = new TriggerPoint("DB450.2792", "DB450.2796", "DB450.380") { TextBoxCtl = txtPartId32 };
            //this[33] = new TriggerPoint("DB450.2800", "DB450.2804", "DB450.400") { TextBoxCtl = txtPartId33 };
            //this[34] = new TriggerPoint("DB450.2808", "DB450.2812", "DB450.420") { TextBoxCtl = txtPartId34 };
            //this[35] = new TriggerPoint("DB450.2816", "DB450.2820", "DB450.440") { TextBoxCtl = txtPartId35 };
            //this[36] = new TriggerPoint("DB450.2824", "DB450.2828", "DB450.460") { TextBoxCtl = txtPartId36 };

            this[100] = new TriggerPoint("DB450.2022", "DB450.2026", "DB450.1998.20") {TextBoxCtl = txtPartId100};
            this[101] = new TriggerPoint("DB450.2122", "DB450.2126", "DB450.2098.20") {TextBoxCtl = txtPartId101};
            this[102] = new TriggerPoint("DB450.2222", "DB450.2226", "DB450.2198.20") {TextBoxCtl = txtPartId102};
            this[103] = new TriggerPoint("DB450.2322", "DB450.2326", "DB450.2298.20") {TextBoxCtl = txtPartId103};
            this[104] = new TriggerPoint("DB450.2422", "DB450.2426", "DB450.2398.20") {TextBoxCtl = txtPartId104};
            this[105] = new TriggerPoint("DB450.2522", "DB450.2526", "DB450.2498.20") {TextBoxCtl = txtPartId105};
            this[106] = new TriggerPoint("DB450.2622", "DB450.2626", "DB450.2598.20") {TextBoxCtl = txtPartId106};
            this[107] = new TriggerPoint("DB450.2722", "DB450.2726", "DB450.2698.20") {TextBoxCtl = txtPartId106};


            this[200] = new TriggerPoint("DB450.3022", "DB450.3026", "DB450.3000") {TextBoxCtl = txtPartId106};

            //this.rbtnAutoConnect.DataBindings.Add("Enabled", this, "IsNotAuto");
            //this.rbtnAutoConnectDis.DataBindings.Add("Enabled", this, "IsAuto");
        }

        public bool IsNotAuto
        {
            get => !IsAuto;
            set => IsAuto = !value;
        }

        public bool IsAuto { get; set; }

        private bool IsConnect { get; set; }

        public TriggerPoint this[int pos]
        {
            get
            {
                if (!TPS.ContainsKey(pos))
                {
                    throw new KeyNotFoundException(nameof(pos));
                }

                return TPS[pos];
            }
            set
            {
                if (!TPS.ContainsKey(pos))
                {
                    TPS.Add(pos, value);
                }
                else
                {
                    TPS[pos] = value;
                }
            }
        }

        private void WriteMsg(string msg)
        {
            txtMsg.InvokeExecute(() => { txtMsg.Text += $"{DateTime.Now.ToLongTimeString()}：{msg}\r\n"; });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress address;
            if (!IPAddress.TryParse(txtIpAddress.Text, out address))
            {
                WriteMsg("Ip地址输入不正确！");
                return;
            }

            _siemensTcpNet.IpAddress = txtIpAddress.Text;

            try
            {
                var connect = _siemensTcpNet.ConnectServer();
                if (connect.IsSuccess)
                {
                    IsConnect = true;
                    WriteMsg("连接成功！");
                    btnDisConnect.Enabled = true;
                    btnConnect.Enabled = false;
                }
                else
                {
                    WriteMsg("连接失败！");
                }
            }
            catch (Exception ex)
            {
                WriteMsg(ex.Message);
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            _siemensTcpNet.ConnectClose();
            IsConnect = false;
            btnDisConnect.Enabled = false;
            btnConnect.Enabled = true;
        }

        private void btnInPart10_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[10], txtPartId10.Text);
        }

        private void btnInPart11_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[11], txtPartId11.Text);
        }

        private void btnInOutPosition20_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[20], txtPartId20.Text);
        }

        private void btnInOutPosition21_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[21], txtPartId21.Text);
        }

        private void btnInOutPosition22_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[22], txtPartId22.Text);
        }

        private void btnInOutPosition23_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[23], txtPartId23.Text);
        }

        private void btnInOutPosition24_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[24], txtPartId24.Text);
        }

        private void btnInOutPosition25_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[25], txtPartId25.Text);
        }

        private void btnInOutPosition26_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[26], txtPartId26.Text);
        }

        private void RequestTrigger(TriggerPoint tp, string rtb)
        {
            if (rtb == null) return;
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }

            var gid = Guid.NewGuid().ToString();
            WriteMsg("++++++++++++++++++++++++++++++++++++++++++++++++++");
            var s = _siemensTcpNet.ReadInt32(tp.TriggerInAddress);
            if (s.IsSuccess)
            {
                WriteMsg($"读取变量[{tp.TriggerInAddress}]=>[{s.Content}],[{s.IsSuccess}]");

                var s1 = _siemensTcpNet.Write(tp.DataAddress, rtb.Trim());
                WriteMsg($"写入变量[{tp.DataAddress}]=>[{rtb.Trim()}],[{s1.IsSuccess}]");


                var s3 = _siemensTcpNet.Write(tp.TriggerOutAddress, s.Content);
                WriteMsg($"写入变量[{tp.TriggerOutAddress}]=>[{s.Content}],[{s3.IsSuccess}]");

                var v = s.Content + 1;
                var s2 = _siemensTcpNet.Write(tp.TriggerInAddress, v);
                WriteMsg($"写入变量[{tp.TriggerInAddress}]=>[{v}],[{s2.IsSuccess}]");

                tp.Value = rtb;
            }

            WriteMsg("++++++++++++++++++++++++++++++++++++++++++++++++++");
        }

        private void btnInOutPosition30_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[30], txtPartId30.Text);
        }

        private void btnInOutPosition31_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[31], txtPartId31.Text);
        }


        private void btnCompleted100_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[100], txtPartId100.Text);
        }

        private void btnCompleted101_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[101], txtPartId101.Text);
        }

        private void btnCompleted102_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[102], txtPartId102.Text);
        }

        private void btnCompleted103_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[103], txtPartId103.Text);
        }

        private void btnCompleted104_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[104], txtPartId104.Text);
        }

        private void btnCompleted105_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[105], txtPartId105.Text);
        }

        private void btnCompleted106_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[106], txtPartId106.Text);
        }

        private void ReadData(string dataAddress, RichTextBox rtx)
        {
            rtx.Text = "";
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }

            ushort maxBytesCount = 600;
            var i = 0;
            var bytes = _siemensTcpNet.ReadBytes(dataAddress, maxBytesCount);
            if (bytes.IsSuccess)
            {
                while (i < maxBytesCount)
                {
                    var str = Encoding.ASCII.GetString(bytes.Content, i, 20).Trim('\0');
                    i = i + 20;
                    if (string.IsNullOrEmpty(str)) continue;
                    rtx.Text = rtx.Text + str + "\n";
                }

                WriteMsg("读取数据成功！");
            }
            else
            {
                WriteMsg("读取数据失败！");
            }
        }

        private void WriteData(string dataAddress, RichTextBox rtx)
        {
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }

            if (string.IsNullOrEmpty(rtx.Text.Trim()))
            {
                WriteMsg("不能为空！");
                return;
            }

            ushort maxBytesCount = 600; // 总长度
            ushort sBytesCount = 20; // 每个的长度
            var datas = rtx.Text.Trim().Split('\n');

            if (datas.Length * sBytesCount > maxBytesCount)
            {
                WriteMsg($"写入的值过多，不能大于{maxBytesCount / sBytesCount}个");
                return;
            }

            if (datas.Any(m => Encoding.ASCII.GetBytes(m).Length > sBytesCount))
            {
                WriteMsg($"存在超长的数据,数据最大长度{sBytesCount}");
                return;
            }

            var bytes = new byte[maxBytesCount];
            var i = 0;
            foreach (var data in datas)
            {
                if (i > maxBytesCount)
                {
                    continue;
                }

                var sr = Encoding.ASCII.GetBytes(data);

                var l = 20;
                if (sr.Length < l)
                {
                    l = sr.Length;
                }

                Array.Copy(sr, 0, bytes, i, l);

                i = i + 20;
            }

            var s = _siemensTcpNet.Write(dataAddress, bytes);
            if (!s.IsSuccess)
            {
                WriteMsg("写入数据失败！");
            }
            else
            {
                WriteMsg("写入数据成功！");
            }
        }

        private void btnRead51_Click(object sender, EventArgs e)
        {
            ReadData("DB450.3000", txtPartId51);
        }

        private void btnWrite51_Click(object sender, EventArgs e)
        {
            WriteData("DB450.3000", txtPartId51);
        }

        private void btnRead52_Click(object sender, EventArgs e)
        {
            ReadData("DB450.3600", txtPartId52);
        }

        private void btnWrite52_Click(object sender, EventArgs e)
        {
            WriteData("DB450.3600", txtPartId52);
        }

        private void btnRead53_Click(object sender, EventArgs e)
        {
            ReadData("DB450.4200", txtPartId53);
        }

        private void btnWrite53_Click(object sender, EventArgs e)
        {
            WriteData("DB450.4200", txtPartId53);
        }

        private void btnRead54_Click(object sender, EventArgs e)
        {
            ReadData("DB450.4800", txtPartId54);
        }

        private void btnWrite54_Click(object sender, EventArgs e)
        {
            WriteData("DB450.4800", txtPartId54);
        }

        private void btnRead55_Click(object sender, EventArgs e)
        {
            ReadData("DB450.5200", txtPartId55);
        }

        private void btnWrite55_Click(object sender, EventArgs e)
        {
            WriteData("DB450.5200", txtPartId55);
        }

        private void btnRead56_Click(object sender, EventArgs e)
        {
            ReadData("DB450.6000", txtPartId56);
        }

        private void btnWrite56_Click(object sender, EventArgs e)
        {
            WriteData("DB450.6000", txtPartId56);
        }

        private void rbtnAutoConnect_Click(object sender, EventArgs e)
        {
            // 获取数据
            IsAuto = true;

            WriteMsg("开始启动自动连接");
            // 加载板件
            var parts = LoadPart();
            // 计算路径
            PPS = Calculation(parts);

            // 启动触发
            IsAuto = true;
        }

        public List<string> LoadPart()
        {
            WriteMsg("开始加载板件");
            var cc = new DrillingService();
            var pts = cc.GetTestPart();
            WriteMsg($"共加载板件数量{pts.Count()}");
            return pts;
        }


        private void rbtnAutoConnectDis_Click(object sender, EventArgs e)
        {
            IsAuto = false;
            WriteMsg("停止自动连接");
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            foreach (var item in TPS)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(item.Value.Interval);
                        if (!IsAuto || PPS.Count == 0) continue;

                        var tps = new List<PartPath>();
                        var poss = new List<int>();

                        foreach (var pp in PPS)
                        {
                            // 板件第一个未处理的位置
                            var p = pp.Poss.FirstOrDefault(m => m.Status == 0);
                            // 如果第一个未处理的位置与当前位置相等
                            if (p != null && p.Pos == item.Key)
                            {
                                // 触发周期+最近一次触发时间>当前时间，才触发
                                if (item.Value.TriggerTime.AddMilliseconds(item.Value.Interval) < DateTime.Now)
                                {
                                    WriteMsg($"板件{pp.PartId}-位置{p.Pos}-触发请求");
                                    item.Value.Value = pp.PartId;
                                    RequestTrigger(this[p.Pos], pp.PartId);
                                    p.Status = 1;
                                    item.Value.TriggerTime = DateTime.Now;
                                    poss.Add(p.Pos);
                                    break;
                                }
                            }
                        }
                    }
                });
            }
        }

        public List<PartPath> Calculation(List<string> parts)
        {
            WriteMsg("计算路径");
            var pps = new List<PartPath>();
            var i = 0;
            // 区分10,11
            foreach (var item in parts)
            {
                var pp = new PartPath {PartId = item, Idx = i};
                pp.Poss.Add(new PathPos {Pos = i % 2 == 0 ? 10 : 11, Status = 0});
                pps.Add(pp);
                // =10 位置的一定会去20，=11的一定回去22
                pp.Poss.Add(new PathPos {Pos = i % 2 == 0 ? 20 : 22, Status = 0});
                i++;
            }


            // 分配22位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 22)))
            {
                item.Poss.Add(new PathPos {Pos = i % 2 == 0 ? 100 : 21, Status = 0});
                i++;
            }

            // 分配21位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 30)))
            {
                item.Poss.Add(new PathPos {Pos = i % 4 == 0 ? 22 : 101, Status = 0});
                i++;
            }

            // 分配22位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 30)))
            {
                item.Poss.Add(new PathPos {Pos = i % 7 == 0 ? 21 : 23, Status = 0});
                i++;
            }

            // 分配23位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 23)))
            {
                item.Poss.Add(new PathPos {Pos = i % 3 == 0 ? 30 : 24, Status = 0});
                i++;
            }


            // 分配24位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 24)))
            {
                item.Poss.Add(new PathPos {Pos = i % 4 == 0 ? 104 : 25, Status = 0});
                i++;
            }


            // 分配25位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 25)))
            {
                item.Poss.Add(new PathPos {Pos = i % 3 == 0 ? 105 : 26, Status = 0});
                i++;
            }

            // 分配26位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 25)))
            {
                item.Poss.Add(new PathPos {Pos = 31, Status = 0});
                i++;
            }

            // 分配30位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 30)))
            {
                item.Poss.Add(new PathPos {Pos = i % 2 == 0 ? 103 : 102, Status = 0});
                i++;
            }

            // 分配31位置
            i = 0;
            foreach (var item in pps.Where(m => m.Poss.Any(n => n.Pos == 31)))
            {
                item.Poss.Add(new PathPos {Pos = i % 2 == 0 ? 107 : 106, Status = 0});
                i++;
            }

            return pps;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            RequestTrigger(this[200], null);
        }

        public class TriggerPoint
        {
            private string _value;

            public TriggerPoint(string ia, string oa, string da)
            {
                TriggerInAddress = ia;
                TriggerOutAddress = oa;
                DataAddress = da;
            }

            public string TriggerInAddress { get; set; }
            public string TriggerOutAddress { get; set; }
            public string DataAddress { get; set; }

            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    if (TextBoxCtl != null)
                    {
                        TextBoxCtl.InvokeExecute(() => { TextBoxCtl.Text = value; });
                    }
                }
            }

            public int Interval { get; set; } = 5400;

            public DateTime TriggerTime { get; set; }
            public Control TextBoxCtl { get; set; }
        }
    }

    public class PartPath
    {
        public PartPath()
        {
            Poss = new List<PathPos>();
        }

        public int Idx { get; set; }
        public string PartId { get; set; }

        public List<PathPos> Poss { get; set; }
    }

    public class PathPos
    {
        public int Pos { get; set; }

        /// <summary>
        ///     0：未触发，1：已触发
        /// </summary>
        public int Status { get; set; }
    }

    public class PlcCommandQueue
    {
        public PlcCommandQueue(DateTime dt, string inAddress, string outAddress, string dataAddress, string data)
        {
            ExecTime = dt;
            InAddress = inAddress;
            OutAddress = outAddress;
            DataAddress = dataAddress;
            Data = data;
        }

        public DateTime ExecTime { get; set; }

        public string InAddress { get; set; }
        public string OutAddress { get; set; }
        public string DataAddress { get; set; }
        public string Data { get; set; }
    }

    public class TriggerPosManager
    {
        private static object CommandLock = new object();

        public Dictionary<int, TriggerPos> Poss = new Dictionary<int, TriggerPos>();

        public TriggerPos this[int index]
        {
            get => Poss[index];
            set
            {
                if (Poss.ContainsKey(index))
                {
                    Poss[index] = value;
                }
                else
                {
                    Poss.Add(index, value);
                }
            }
        }

        public List<PlcCommandQueue> Commands { get; set; } = new List<PlcCommandQueue>();
    }

    public class TriggerPos
    {
        private readonly Func<int> NexPost;
        private readonly Queue<PlcCommandQueue> Queues = new Queue<PlcCommandQueue>();

        public TriggerPos(SiemensS7Net sie, string ia, string oa, string da, Func<int> next, int si = 2000,
            int ei = 4000)
        {
            _siemensTcpNet = sie;
            NexPost = next;
        }

        private SiemensS7Net _siemensTcpNet { get; }

        public int Pos { get; set; }

        public bool IsStart { get; set; }

        public int StarInt { get; set; } = 3000;

        public int EndInt { get; set; } = 5000;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(TriggerPos other)
        {
            return Equals(Pos, other.Pos);
        }

        public override int GetHashCode()
        {
            return Pos != null ? Pos.GetHashCode() : 0;
        }

        public void AddQueue(PlcCommandQueue data)
        {
            lock (Queues)
            {
                Queues.Enqueue(data);
            }
        }

        public int GetNext()
        {
            return NexPost.Invoke();
        }

        private void RequestTrigger(string triggerInAddress, string triggerOutAddress, string dataAddress, string rtb)
        {
            var gid = Guid.NewGuid().ToString();
            var s = _siemensTcpNet.ReadInt32(triggerInAddress);
            if (s.IsSuccess)
            {
                var s1 = _siemensTcpNet.Write(dataAddress, rtb.Trim());
                var s3 = _siemensTcpNet.Write(triggerOutAddress, s.Content);
                var v = s.Content + 1;
                var s2 = _siemensTcpNet.Write(triggerInAddress, v);
            }
        }

        public void CreateTrigger()
        {
            if (Queues.Count == 0) return;
            PlcCommandQueue pp = null;
            lock (Queues)
            {
                pp = Queues.Dequeue();
            }

            RequestTrigger(pp.InAddress, pp.OutAddress, pp.DataAddress, pp.Data);
        }
    }
}