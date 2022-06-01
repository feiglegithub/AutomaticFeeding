using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class NgControl : UserControl,IStop
    {
        private PartInfoService service = null;
        private PartInfoService Service => service ?? (service = new PartInfoService());
        private ILogger _log = LogManager.GetLogger(typeof(NgControl).Name);
        private ushort BatchWriteLength = SimulatorSettings.Current.BatchWriteLength;
        private ushort UpiLength = SimulatorSettings.Current.UpiWriteLength;
        public NgControl()
        {
            InitializeComponent();
            LogManager.AddLoggerAdapter(new Log.Implement.Log4Net.Log4NetLoggerAdapter());
        }

        private Thread th = null;

        private PlcOperator Plc => plcOperator ?? (plcOperator = PlcOperator.GetInstance());

        private PlcOperator plcOperator = null;

        public string InPartDB{get => txtDB.Text;set => txtDB.Text = value;}
        public string TriggerInDB { get => txtDBTriggerIn.Text; set => txtDBTriggerIn.Text = value; }
        public string TriggerOutDB { get => txtDbTriggerOut.Text; set => txtDbTriggerOut.Text = value; }
        public string OutPartDB { get => txtDBOutPart.Text; set => txtDBOutPart.Text = value; }
        public string ResultDB { get => txtDBResult.Text; set => txtDBResult.Text = value; }
        public string BatchDB { get => txtDBBatch.Text; set => txtDBBatch.Text = value; }
        public string PartTypeDB { get => txtDBPartType.Text; set => txtDBPartType.Text = value; }

        public string OffsetInPart { get => txtOffset.Text; set => txtOffset.Text = value; }
        public string OffsetInTrigger { get => txtTriggerInOffset.Text; set => txtTriggerInOffset.Text = value; }
        public string OffsetOutTrigger { get => txtTriggerOutOffset.Text; set => txtTriggerOutOffset.Text = value; }
        public string OffsetOutPart { get => txtOutPartOffset.Text; set => txtOutPartOffset.Text = value; }
        public string OffsetOutResult { get => txtResultOffset.Text; set => txtResultOffset.Text = value; }
        public string OffsetOutBatch { get => txtBatchOffset.Text; set => txtBatchOffset.Text = value; }
        public string OffsetOutPartType { get => txtPartTypeOffset.Text; set => txtPartTypeOffset.Text = value; }

        private string InPartAddr => GetAddr(txtDB, txtOffset);
        private string TriggerInAddr => GetAddr(txtDBTriggerIn, txtTriggerInOffset);
        private string TriggerOutAddr => GetAddr(txtDbTriggerOut, txtTriggerOutOffset);
        private string OutPartAddr => GetAddr(txtDBOutPart, txtOutPartOffset);
        private string ResultAddr => GetAddr(txtDBResult, txtResultOffset);
        private string BatchAddr => GetAddr(txtDBBatch, txtBatchOffset);
        private string PartTypeAddr => GetAddr(txtDBPartType, txtPartTypeOffset);

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

        private string InPartContent { get => GetContent(txtInPartContent); set => SetContent(txtInPartContent, value); }
        private string OutPartContent { get => GetContent(txtOutPartContent); set => SetContent(txtOutPartContent, value); }
        private int TriggerInContent { get => Convert.ToInt32(GetContent(txtTriggerInContent)); set => SetContent(txtTriggerInContent, value); }
        private int TriggerOutContent { get => Convert.ToInt32(GetContent(txtTriggerOutContent)); set => SetContent(txtTriggerOutContent, value); }
        private string BatchContent { get => GetContent(txtBatchContent); set => SetContent(txtBatchContent, value); }
        private int ResultContent { get => Convert.ToInt32(GetContent(txtResultContent)); set => SetContent(txtResultContent, value); }
        private int PartTypeContent { get => Convert.ToInt32(GetContent(txtPartTypeContent));set => SetContent(txtPartTypeContent, value); }


        public void Stop()
        {
            if (th != null)
            {
                th.Abort();
                th = null;
            }
        }

        /// <summary>
        /// 执行写入后
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="result">执行结果</param>
        /// <param name="content">写入内容</param>
        private void PlcWriteResultLog(string addr, PLC.Communication.Core.Types.OperateResult result, string content)
        {
            _log.Info($"写入地址:{addr},结果:{(result.IsSuccess ? "成功" : "失败")},应写入内容:{content},错误代码：{result.ErrorCode},错误描述:{result.Message}");
        }

        /// <summary>
        /// 执行写入后，再读取写入后的结果
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="content">写入后的内容</param>
        private void PlcWriteAfterLog(string addr, string content)
        {
            _log.Info($"执行写入后，地址{addr}内容:{content}");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            th = new Thread(() =>
            {
                while (true)
                {
                    int triggerIn = Plc.ReadLong(TriggerInAddr);
                    int triggerOut = Plc.ReadLong(TriggerOutAddr);
                    TriggerInContent = triggerIn;
                    TriggerOutContent = triggerOut;
                    
                    if (triggerIn > triggerOut)
                    {
                        string inPart = Plc.ReadString(InPartAddr, UpiLength);
                        if(string.IsNullOrWhiteSpace(inPart)) continue;
                        InPartContent = inPart;
                        OutPartContent = InPartContent;
                        _log.Info($"Plc请求:{triggerIn},请求板件号:{inPart}");
                        var partInfos = Service.GetInfos(inPart);
                        string batch = "";
                        if (partInfos.Count > 0)
                        {
                            batch = partInfos[0].BatchCode;
                            if (string.IsNullOrWhiteSpace(batch))
                            {
                                batch = "HB_GS1243094544312";
                            }
                            BatchContent = batch;
                            _log.Info($"查询批次结果:{batch}，需要写入地址:{BatchAddr}");
                            ResultContent = 10;
                            if (SimulatorSettings.Current.PartCount++ < 5)
                            {
                                PartTypeContent = 30;//不出板
                            }
                            else
                            {
                                SimulatorSettings.Current.PartCount = 0;
                                PartTypeContent = 20;//抽检
                                SimulatorSettings.Current.Save();
                            }
                            WirteResult(triggerIn);
                            //var ret1 =Plc.Write(BatchAddr, BatchContent, WriteLength);
                            //var ret2 =Plc.Write(ResultAddr, ResultContent);
                            //var ret3 =Plc.Write(PartTypeAddr, PartTypeContent);
                            //if (ret1.IsSuccess && ret2.IsSuccess && ret3.IsSuccess)
                            //{
                            //    Plc.Write(TriggerOutAddr, triggerIn);
                            //}
                        }
                        else
                        {
                            //var distrbutes = Service.GeTaskDistributes(inPart);
                            //if (distrbutes.Count > 0)//余料
                            if (/*inPart.Contains("X")*/Regex.IsMatch(inPart, "[a-zA-Z]"))
                            {
                                //BatchContent = distrbutes[0].BatchCode;
                                BatchContent = "HB_GS1243094";
                                ResultContent = 10;
                                PartTypeContent = 10;//余料
                                _log.Info($"查询批次结果:{BatchContent}，需要写入地址:{BatchAddr}");
                                WirteResult(triggerIn);

                                //var ret1 =Plc.Write(BatchAddr, BatchContent, WriteLength);
                                //var ret2 = Plc.Write(ResultAddr, ResultContent);
                                //var ret3 = Plc.Write(PartTypeAddr, PartTypeContent);
                                //if (ret1.IsSuccess && ret2.IsSuccess && ret3.IsSuccess)
                                //{
                                //    Plc.Write(TriggerOutAddr, triggerIn);
                                //}
                            }
                            else //找不到板件。停止
                            {
                                //batch = "HB_GS12430941224";
                                BatchContent = batch;
                                ResultContent = 20;
                                _log.Info($"查询批次结果:{batch}，需要写入地址:{BatchAddr}");
                                WirteResult(triggerIn);
                                //var ret1 = Plc.Write(BatchAddr, BatchContent, WriteLength);
                                //var ret2 = Plc.Write(ResultAddr, ResultContent);
                                //if (ret1.IsSuccess && ret2.IsSuccess)
                                //{
                                //    Plc.Write(TriggerOutAddr,triggerIn);
                                //}
                            }

                        }
                        OutPartContent = Plc.ReadString(OutPartAddr, UpiLength);
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

        private void WirteResult(int triggerIn)
        {
            var ret = Plc.Write(OutPartAddr, OutPartContent, UpiLength);
            PlcWriteResultLog(OutPartAddr, ret, OutPartContent);
            string outPart = Plc.ReadString(OutPartAddr, UpiLength);
            PlcWriteAfterLog(OutPartAddr, outPart);
            var ret1 = Plc.Write(BatchAddr, BatchContent, BatchWriteLength);
            PlcWriteResultLog(BatchAddr, ret1, BatchContent);
            var batchName = Plc.ReadString(BatchAddr, BatchWriteLength);
            PlcWriteAfterLog(BatchAddr, batchName);

            var ret2 = Plc.Write(ResultAddr, ResultContent);
            PlcWriteResultLog(ResultAddr, ret2, ResultContent.ToString());
            var result = Plc.ReadLong(ResultAddr);
            PlcWriteAfterLog(ResultAddr, result.ToString());

            var ret3 = Plc.Write(PartTypeAddr, PartTypeContent);
            PlcWriteResultLog(PartTypeAddr, ret3, PartTypeContent.ToString());
            var partType = Plc.ReadLong(PartTypeAddr);
            PlcWriteAfterLog(PartTypeAddr, PartTypeContent.ToString());

            if (ret1.IsSuccess && ret2.IsSuccess&&ret3.IsSuccess&&ret.IsSuccess)
            {
                _log.Info($"执行写入结果均成功");
                //Plc.Write(TriggerOutAddr, triggerIn);
            }

            if (batchName != BatchContent || result != ResultContent || partType!= PartTypeContent||outPart!=OutPartContent)
            {
                string tips = $"写入后数据与要写入数据不一致";
                _log.Info(tips);
                Tips(tips);
            }
            else
            {
                Plc.Write(TriggerOutAddr, triggerIn);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            if (th != null)
            {
                th.Abort();
            }
            btnStart.Enabled = true;
        }

        private string GetAddr(RadTextBox txtDb, RadTextBox txtOff)
        {
            string addr = "";
            txtDb.Invoke((Action)(() => addr = "DB" + txtDb.Text + "." + txtOff.Text));
            return addr;
        }

        private void txtOffset_TextChanged(object sender, EventArgs e)
        {
            if (sender is RadTextBox txtBox)
            {
                try
                {
                    Convert.ToUInt32(txtBox.Text.Trim());
                }
                catch (Exception exception)
                {
                    RadMessageBox.Show("输入地址偏移量格式不对");
                    txtBox.Text = "0";
                }
            }
        }

        private void txtDB_TextChanged(object sender, EventArgs e)
        {
            if (sender is RadTextBox txtBox)
            {
                try
                {
                    Convert.ToUInt32(txtBox.Text.Trim());
                }
                catch (Exception exception)
                {
                    RadMessageBox.Show("输入DB块格式不对");
                    txtBox.Text = "450";
                }
            }
        }

        private void btnReadPartId_Click(object sender, EventArgs e)
        {
            txtInPartContent.Text = Plc.ReadString(InPartAddr, UpiLength);
        }

        private void btnTriggerIn_Click(object sender, EventArgs e)
        {
            txtTriggerInContent.Text = Plc.ReadLong(TriggerInAddr).ToString();
        }

        private void btnOutPart_Click(object sender, EventArgs e)
        {
            Plc.Write(OutPartAddr, txtOutPartContent.Text.Trim(), UpiLength);
            txtOutPartContent.Text = Plc.ReadString(OutPartAddr, UpiLength);
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {

            Plc.Write(BatchAddr, txtBatchContent.Text.Trim(), BatchWriteLength);
            txtBatchContent.Text = Plc.ReadString(BatchAddr, BatchWriteLength);
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            Plc.Write(ResultAddr, Convert.ToInt32(txtResultContent.Text.Trim()));
            txtResultContent.Text = Plc.ReadLong(ResultAddr).ToString();
        }

        private void btnTriggerOut_Click(object sender, EventArgs e)
        {
            Plc.Write(TriggerOutAddr, Convert.ToInt32(txtTriggerOutContent.Text.Trim()));
            txtTriggerOutContent.Text = Plc.ReadLong(TriggerOutAddr).ToString();
        }

        private void btnPartTypeWrite_Click(object sender, EventArgs e)
        {
            Plc.Write(PartTypeAddr, Convert.ToInt32(txtPartTypeContent.Text.Trim()));
            txtPartTypeContent.Text = Plc.ReadLong(PartTypeAddr).ToString();
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(this, tips)));
        }
    }
}
