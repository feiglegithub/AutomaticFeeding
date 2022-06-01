using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Service;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class PartReenterControl : UserControl, IStop
    {
        private PartInfoService service = null;
        private PartInfoService Service => service ?? (service = new PartInfoService());
        private ushort BatchWriteLength = SimulatorSettings.Current.BatchWriteLength;
        private ushort UpiLength = SimulatorSettings.Current.UpiWriteLength;
        private PlcOperator Plc => plcOperator ?? (plcOperator = PlcOperator.GetInstance());

        private PlcOperator plcOperator = null;

        private Thread th = null;
        //private Thread Th => th??(th=new Thread())

        public PartReenterControl()
        {
            InitializeComponent();
        }

        //private string InPartAddr => GetAddr(txtDB, txtOffset);
        private string TriggerInAddr => GetAddr(txtDBTriggerIn, txtTriggerInOffset);
        private string TriggerOutAddr => GetAddr(txtDbTriggerOut, txtTriggerOutOffset);
        private string OutPartAddr => GetAddr(txtDBOutPart, txtOutPartOffset);
        private string ResultAddr => GetAddr(txtDBResult, txtResultOffset);
        private string BatchAddr => GetAddr(txtDBBatch, txtBatchOffset);

        //public string InPartDB { get => txtDB.Text; set => txtDB.Text = value; }
        public string TriggerInDB { get => txtDBTriggerIn.Text; set => txtDBTriggerIn.Text = value; }
        public string TriggerOutDB { get => txtDbTriggerOut.Text; set => txtDbTriggerOut.Text = value; }
        public string OutPartDB { get => txtDBOutPart.Text; set => txtDBOutPart.Text = value; }
        public string ResultDB { get => txtDBResult.Text; set => txtDBResult.Text = value; }
        public string BatchDB { get => txtDBBatch.Text; set => txtDBBatch.Text = value; }
        //public string PartTypeDB { get => txtDBPartType.Text; set => txtDBPartType.Text = value; }

        //public string OffsetInPart { get => txtOffset.Text; set => txtOffset.Text = value; }
        public string OffsetInTrigger { get => txtTriggerInOffset.Text; set => txtTriggerInOffset.Text = value; }
        public string OffsetOutTrigger { get => txtTriggerOutOffset.Text; set => txtTriggerOutOffset.Text = value; }
        public string OffsetOutPart { get => txtOutPartOffset.Text; set => txtOutPartOffset.Text = value; }
        public string OffsetOutResult { get => txtResultOffset.Text; set => txtResultOffset.Text = value; }
        public string OffsetOutBatch { get => txtBatchOffset.Text; set => txtBatchOffset.Text = value; }
        //public string PartTypeOffset { get => txtPartTypeOffset.Text; set => txtPartTypeOffset.Text = value; }

        private string GetContent(RadTextBox txtBox)
        {
            string text = "";
            txtBox.Invoke((Action)(() => text = txtBox.Text));
            return text;
        }

        private void SetContent(RadTextBox txtBox, string text)
        {
            txtBox.Invoke((Action)(() => txtBox.Text = text));
        }

        private void SetContent(RadTextBox txtBox, int text)
        {
            txtBox.Invoke((Action)(() => txtBox.Text = text.ToString()));
        }

        //private string InPartContent { get => GetContent(txtInPartContent); set => SetContent(txtInPartContent, value); }
        private string OutPartContent { get => GetContent(txtOutPartContent); set => SetContent(txtOutPartContent, value); }
        private int TriggerInContent { get => Convert.ToInt32(GetContent(txtTriggerInContent)); set => SetContent(txtTriggerInContent, value); }
        private int TriggerOutContent { get => Convert.ToInt32(GetContent(txtTriggerOutContent)); set => SetContent(txtTriggerOutContent, value); }
        private string BatchContent { get => GetContent(txtBatchContent); set => SetContent(txtBatchContent, value); }
        private int ResultContent { get => Convert.ToInt32(GetContent(txtResultContent)); set => SetContent(txtResultContent, value); }


        private string GetAddr(RadTextBox txtDb, RadTextBox txtOff)
        {
            string addr = "";
            txtDb.Invoke((Action)(() => addr = "DB" + txtDb.Text + "." + txtOff.Text));
            return addr;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string upi = InPutUip;
            if (string.IsNullOrWhiteSpace(upi))
            {
                Tips("请输入板件号");
            }
            var partInfos = Service.GetInfos(upi);
            if (partInfos.Count>0)
            {
                string batch = partInfos[0].BatchCode;
                if (string.IsNullOrWhiteSpace(batch))
                {
                    batch = "HB_GS12430941224";
                }

                BatchContent = batch;
                OutPartContent = partInfos[0].PartId;
                ResultContent = 10;

            }

        }

        private bool CanReenter(string upi)
        {
            return true;
        }

        private bool IsNg(string upi)
        {
            return false;
        }

        private void btnTriggerIn_Click(object sender, EventArgs e)
        {
            txtTriggerInContent.Text = Plc.ReadLong(TriggerInAddr).ToString();
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            Plc.Write(BatchAddr, txtBatchContent.Text.Trim(), BatchWriteLength);
            txtBatchContent.Text = "";
            txtBatchContent.Text = Plc.ReadString(BatchAddr, BatchWriteLength);
        }

        private void btnOutPart_Click(object sender, EventArgs e)
        {
            Plc.Write(OutPartAddr, txtOutPartContent.Text.Trim(), UpiLength);
            txtOutPartContent.Text = "";
            txtOutPartContent.Text = Plc.ReadString(OutPartAddr, UpiLength);
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            Plc.Write(ResultAddr, Convert.ToInt32(txtResultContent.Text.Trim()));
            txtResultContent.Text = "";
            txtResultContent.Text = Plc.ReadLong(ResultAddr).ToString();
        }

        private void btnTriggerOut_Click(object sender, EventArgs e)
        {
            Plc.Write(TriggerOutAddr, Convert.ToInt32(txtTriggerOutContent.Text.Trim()));
            txtTriggerOutContent.Text = Plc.ReadLong(TriggerOutAddr).ToString();
        }

        private string InPutUip
        {
            get=> GetContent(txtUpi);
            set => SetContent(txtUpi, value);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnSearch.Enabled = false;
            btnOutPart.Enabled = btnBatch.Enabled = btnResult.Enabled = btnTriggerOut.Enabled = false;
            th = new Thread(() =>
            {
                while (true)
                {
                    int triggerIn = Plc.ReadLong(TriggerInAddr);
                    int triggerOut = Plc.ReadLong(TriggerOutAddr);
                    TriggerInContent = triggerIn;
                    TriggerOutContent = triggerOut;
                    string inPart = InPutUip;
                    //InPartContent = inPart;

                    if (triggerIn > triggerOut)
                    {
                        if (string.IsNullOrWhiteSpace(inPart))
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        var partInfos = Service.GetInfos(inPart);
                        string batch = "";
                        if (partInfos.Count > 0)
                        {
                            batch = partInfos[0].BatchCode;
                            if (string.IsNullOrWhiteSpace(batch))
                            {
                                batch = "Default";
                            }
                            //_log.Info($"查询批次结果:{batch}，需要写入地址:{BatchAddr}");
                            BatchContent = batch;
                            ResultContent = 10;
                            var ret = Plc.Write(OutPartAddr, OutPartContent, UpiLength);
                            var outPart = Plc.ReadString(OutPartAddr, UpiLength);

                            var ret1 = Plc.Write(BatchAddr, BatchContent, BatchWriteLength);
                            var batchName = Plc.ReadString(BatchAddr, BatchWriteLength);
                            var ret2 = Plc.Write(ResultAddr, ResultContent);
                            var result = Plc.ReadLong(ResultAddr);
                            if (outPart!=null&&batchName!=null&&result!=-1&&outPart== OutPartContent&&batchName== BatchContent&&result== ResultContent)
                            {
                                var ret3 = Plc.Write(TriggerOutAddr, triggerIn);
                                var trigger = Plc.ReadLong(TriggerOutAddr);
                                if (trigger != -1 && trigger == triggerIn)
                                {
                                    InPutUip = "";
                                }
                            }
                            //WirteResult(triggerIn);

                        }
                        else
                        {
                            Tips("不允许异常板或者余料板回流线体");
                        }

                        BatchContent = Plc.ReadString(BatchAddr, BatchWriteLength);
                        ResultContent = Plc.ReadLong(ResultAddr);
                        TriggerOutContent = Plc.ReadLong(TriggerOutAddr);

                    }
                    Thread.Sleep(20);
                }
            });
            btnStart.Enabled = false;
            th.IsBackground = true;
            th.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
            //btnStop.Enabled = false;
            //if (th != null)
            //{
            //    th.Abort();
            //    th = null;
            //}
            //btnStart.Enabled = true;
            //btnSearch.Enabled = true;
            //btnOutPart.Enabled = btnBatch.Enabled = btnResult.Enabled = btnTriggerOut.Enabled = true;
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(this, tips)));
        }

        public void Stop()
        {
            btnStop.Enabled = false;
            if (th != null)
            {
                th.Abort();
                th = null;
            }
            btnStart.Enabled = true;
            btnSearch.Enabled = true;
            btnOutPart.Enabled = btnBatch.Enabled = btnResult.Enabled = btnTriggerOut.Enabled = true;
        }
    }
}
