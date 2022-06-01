using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PartMill.Contract;
using NJIS.FPZWS.LineControl.PartMill.Model;
using NJIS.FPZWS.LineControl.PartMill.Service;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Communications;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using NJIS.FPZWS.UI.Common.Message;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters
{
    public class LineTaskPresenter:PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string Execute = nameof(Execute);
        public const string BindingData = nameof(BindingData);
        public const string AutoExecute = nameof(AutoExecute);
        public const string StopAuto = nameof(StopAuto);
        public const string FinishedResult = nameof(FinishedResult);

        private IContract contract = new PartMillService();

        private LineCommunication _lineCommunication = null;

        private LineCommunication Line =>
            _lineCommunication ?? (_lineCommunication = new LineCommunication(contract.GetLineConfigs()));

        private bool IsAuto { get; set; } = false;

        public LineTaskPresenter()
        {
            Register<string>(GetData, (sender,arg) =>
            {
                try
                {
                    var data = contract.GetLineTasks();
                    Send(BindingData, sender, data);
                }
                catch (Exception e)
                {
                    Send(BindingData, sender, (List<line_task>)null);
                    Console.WriteLine(e);
                    
                }
            });

            Register<line_task>(Execute, (sender, arg) =>
            {
                this.ExecuteBase(this,sender,arg, (s, a) =>
                {
                    var task = arg;
                    var tuple = contract.BeginLineTask(task);
                    if (!tuple.Item1)
                    {
                        SendTipsMessage("任务开始失败:" + tuple.Item2);
                        Send(FinishedResult, sender, false);
                        return;
                    }
                    var result = Line.WriteTarget((ELineName)task.start_position, (ELineName)task.end_position);

                    SendTipsMessage(result.Result ? "写入成功" : "写入失败");
                    if (!result.Result)
                    {
                        Send(FinishedResult, sender, false);
                        return;

                    }

                    var config = Line.LineConfigs.FirstOrDefault(item => item.position == task.end_position);
                    if (config == null)
                    {
                        Send(FinishedResult, sender, false);
                        Send(BindingData, sender, "找不到配置信息");
                        return;
                    }
                    Send(BindingData, sender, "等待完成");
                    while (true)
                    {
                        if (Line.Plc.ReadBoolean(config.is_finished_address))
                        {
                            var tuple1 = contract.FinishedLineTask(config.position);
                            Send(BindingData, sender, tuple1.Result ? "线体完成成功！" : "线体任务完成失败" + tuple1.Msg);
                            Send(FinishedResult, sender, tuple1.Result);
                            break;
                        }

                        Thread.Sleep(100);
                    }

                });
            });

            Register<string>(AutoExecute, (sender, arg) =>
            {

                this.ExecuteBase(this,sender,arg, ExecuteAuto);
            });

            Register<string>(StopAuto, (sender, arg) => { IsAuto = false; });

        }

        private void ExecuteAuto(object sender,string arg)
        {
            if (IsAuto)
            {
                return;
            }

            IsAuto = true;
            while (true)
            {
                if(!IsAuto) return;

                // 检查是否有完成任务
                var lineModels = Line.ReadAlLineModels();
                var finishedModel = lineModels.FirstOrDefault(item => item.IsFinished);
                if (finishedModel == null)
                {
                    var tasks = contract.GetLineTasks();
                    if (tasks.Count > 0)
                    {
                        var task = tasks[0];
                        var tuple = contract.BeginLineTask(task);
                        if (!tuple.Item1)
                        {
                            Send(BindingData, sender, "任务开始失败:"+tuple.Item2);
                            continue;
                        }

                        var lineModel = lineModels.FirstOrDefault(item => item.Position == task.start_position);
                        if (lineModel == null)
                        {
                            Send(BindingData, sender, $"找不到线体【{task.start_position}】");
                        }
                        else
                        {
                            if (!lineModel.HasBoard)
                            {
                                Send(BindingData, sender, $"线体【{task.start_position}】无板！");
                            }
                            else
                            {
                                Send(BindingData, sender, $"任务[{task.id}]开始写入Plc:{task.start_position}->{task.end_position}");
                                var r = Line.WriteTarget((ELineName)task.start_position, (ELineName)task.end_position);
                                Send(BindingData, sender, r.Result ? "写入成功！" : "写入失败:" + r.Msg);
                                if (r.Result)
                                {
                                    var ret = contract.LineAcceptTask();
                                    Send(BindingData, sender, ret.Msg);
                                }
                            }
                           
                        }
                        
                    }
                }
                else
                {
                    var result = contract.FinishedLineTask(finishedModel.Position);
                    if (!result.Result)
                    {
                        Send(BindingData,sender,"任务完成失败:"+result.Msg);
                    }
                    else
                    {
                        Send(BindingData, sender, "任务完成成功:" + result.Msg);
                        Send(BindingData, sender, $"开始清除Plc完成信号:{result.Address}->{result.ReturnValue}" );
                        var ret = Line.Plc.Write(result.Address, result.ReturnValue);
                        Send(BindingData, sender, ret.IsSuccess?"清除完成信号成功！": "清除完成信号失败:"+ret.Message);
                        if (!ret.IsSuccess)
                        {
                            Thread.Sleep(20);
                            Send(BindingData, sender, "开始再次清除Plc完成信号");
                            ret = Line.Plc.Write(result.Address, result.ReturnValue);
                            Send(BindingData, sender, ret.IsSuccess ? "清除完成信号成功！" : "再次清除完成信号失败:" + ret.Message);
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }

    }
}
