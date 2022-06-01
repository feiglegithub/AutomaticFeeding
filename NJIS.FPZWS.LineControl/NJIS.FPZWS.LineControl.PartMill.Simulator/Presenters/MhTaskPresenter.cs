using NJIS.FPZWS.LineControl.PartMill.Contract;
using NJIS.FPZWS.LineControl.PartMill.Model;
using NJIS.FPZWS.LineControl.PartMill.Service;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Communications;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Interfaces;
using NJIS.FPZWS.UI.Common.Message;
using System;
using System.Threading;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters
{
    public class MhTaskPresenter:PresenterBase
    {
        public const string GetData = nameof(GetData);
        public const string Execute = nameof(Execute);
        public const string BindingData = nameof(BindingData);
        public const string FinishedResult = nameof(FinishedResult);

        public const string Auto = nameof(Auto);
        public const string StopAuto = nameof(StopAuto);

        private IContract contract = new PartMillService();

        private IMachineHand _machineHand = null;

        private IMachineHand MachineHand =>
            _machineHand ?? (_machineHand = new MachineHandCommunication());

        private bool IsAuto { get; set; } = false;

        public MhTaskPresenter()
        {
            Register<string>(GetData, (sender, arg) =>
            {
                var data = contract.GetMhTasks();
                Send(BindingData, sender, data);

            });

            Register<mh_task>(Execute, (sender, arg) =>
            {
                try
                {
                    var task = arg;
                    var tuple = contract.BeginMhTask(task);
                    if (!tuple.Item1)
                    {
                        SendTipsMessage("写入失败:" + tuple.Item2);
                        return;
                    }

                    if (!MachineHand.Plc.CheckConnect())
                    {
                        var isConnect = MachineHand.Plc.Connect();
                        if (!isConnect)
                        {
                            SendTipsMessage("连接失败");
                            Send(FinishedResult, sender, false);
                            return;
                        }
                    }

                    var result = MachineHand.Plc.Write(task.commad_db_address, task.command_value);

                    SendTipsMessage(result.IsSuccess ? "写入成功" : "写入失败");
                    Send(BindingData, sender, "等待完成");
                    while (true)
                    {
                        if (MachineHand.IsFinished)
                        {
                            var tuple1 = contract.FinishedMhTask();
                            Send(BindingData, sender, tuple1.Result ? "完成成功！" : "完成失败：" + tuple1.Msg);
                            MachineHand.Plc.Write(tuple1.Address, tuple1.ReturnValue);
                            Send(FinishedResult, sender, tuple1.Result);
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                

            });

            Register<string>(Auto, (sender, arg) =>
            {
                ExecuteBase(this,sender,arg, ExecuteAuto);
            });

            Register<string>(StopAuto, (sender, arg) => { IsAuto = false; });
        }

        private void ExecuteAuto(object sender, string arg)
        {
            if (IsAuto) return;
            IsAuto = true;
            while (true)
            {
                if(!IsAuto) return;
                if (MachineHand.IsRequest)
                {
                    var tasks = contract.GetMhTasks();
                    if (tasks.Count > 0)
                    {
                        var task = tasks[0];
                        var tuple = contract.BeginMhTask(task);
                        if (!tuple.Item1)
                        {
                            Send(BindingData, sender, "任务开始失败:" + tuple.Item2);
                            continue;
                        }
                        Send(BindingData, sender, $"任务[{task.id}]开始写入Plc:{task.commad_db_address}->{task.command_value}");
                        //先清零数据
                        MachineHand.Plc.Write(task.commad_db_address, (short)0);
                        var ret = MachineHand.Plc.Write(task.commad_db_address, task.command_value);

                        if (!ret.IsSuccess)
                        {
                            var msg = $"任务[{task.id}]写入Plc失败:{task.commad_db_address}->{task.command_value}";
                            Send(BindingData, sender, msg);
                            //先清零数据
                            MachineHand.Plc.Write(task.commad_db_address, (short)0);
                            Send(BindingData, sender, $"任务[{task.id}]第二次写入Plc:{task.commad_db_address}->{task.command_value}");
                            ret = MachineHand.Plc.Write(task.commad_db_address, task.command_value);
                            msg = ret.IsSuccess ? $"任务[{task.id}]第二次写入Plc失败:{task.commad_db_address}->{task.command_value}" : $"任务[{task.id}]写入Plc成功:{task.commad_db_address}->{task.command_value}";
                            Send(BindingData, sender, msg);
                        }
                        else
                        {
                            Send(BindingData, sender, $"任务[{task.id}]写入Plc成功:{task.commad_db_address}->{task.command_value}");
                        }
                    }
                }
                else if (MachineHand.IsFinished)
                {
                    var result = contract.FinishedMhTask();
                    if (!result.Result)
                    {
                        Send(BindingData, sender, "任务完成失败:" + result.Msg);
                    }
                    else
                    {
                        Send(BindingData, sender, "任务完成成功:" + result.Msg);
                        Send(BindingData, sender, "开始清除机械手完成信号,地址:"+ result.Address);
                        var ret = MachineHand.Plc.Write(result.Address, result.ReturnValue);
                        Send(BindingData, sender, ret.IsSuccess ? "清除完成信号成功！" : "清除完成信号失败:" + ret.Message);
                        if (!ret.IsSuccess)
                        {
                            Thread.Sleep(20);
                            Send(BindingData, sender, "开始再次清除Plc完成信号,地址:" + result.Address);
                            ret = MachineHand.Plc.Write(result.Address, result.ReturnValue);
                            Send(BindingData, sender, ret.IsSuccess ? "清除完成信号成功！" : "再次清除完成信号失败:" + ret.Message);
                        }
                    }
                }
                else if(MachineHand.IsFullBaseBoard)
                {
                    Send(BindingData, sender, "收到底板满垛信号！");
                    Send(BindingData, sender, "开始创建底板垛退料任务！");
                    var ret = contract.CreatedBackPilerTask();
                    if (ret.Result)
                    {
                        Send(BindingData, sender, "创建底板垛退料任务成功！");
                        Send(BindingData, sender, "开始清除底板垛信号！");
                        var r=MachineHand.Plc.Write(ret.Address, ret.ReturnValue);
                        if (!r.IsSuccess)
                        {
                            Send(BindingData, sender, "清除底板垛信号失败:"+ r.Message);
                            Send(BindingData, sender, "开始第二次清除底板垛信号！");
                            r = MachineHand.Plc.Write(ret.Address, ret.ReturnValue);
                            Send(BindingData, sender,r.IsSuccess? "清除底板垛信号成功！":
                                "第二次清除底板信号失败：" + r.Message);
                        }
                    }
                }


                Thread.Sleep(500);
            }

        }

    }
}
