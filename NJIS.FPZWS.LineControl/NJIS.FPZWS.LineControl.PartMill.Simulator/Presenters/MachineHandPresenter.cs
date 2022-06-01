using NJIS.FPZWS.LineControl.PartMill.Simulator.CommunicationBase.Plc;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Communications;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Interfaces;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using NJIS.FPZWS.UI.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters
{
    public class MachineHandPresenter:PresenterBase
    {
        public const string Connect = nameof(Connect);
        public const string ConnectResult = nameof(ConnectResult);
        public const string Close = nameof(Close);
        public const string CloseResult = nameof(CloseResult);
        public const string BindingData = nameof(BindingData);

        //public const string AutoRun = nameof(AutoRun);

        public const string BeginListen = nameof(BeginListen);
        public const string StopListen = nameof(StopListen);

        public const string Write = nameof(Write);

        public const string ClearLastTask = nameof(ClearLastTask);
        public const string FinishedTask = nameof(FinishedTask);


        public const string Stop = nameof(Stop);

        public const string Auto = nameof(Auto);

        public const string ExecuteMsg = nameof(ExecuteMsg);

        private string _DefaultWriteAddr = "DB100.0";
        private string _DefaultReadAddr = "DB102.0";
        private string _DefaultStatusAddr = "DB102.2";

        private PlcOperator Plc = null;

        private Thread th = null;

        private Thread AutoThread = null;

        private bool IsConnect { get; set; } = false;

        private bool IsListen { get; set; } = false;

        public MachineHandPresenter()
        {
            Register();
        }
        private void Register()
        {
            Register<string>(Connect, (sender,ip) =>
            {
                try
                {
                    if (Plc == null)
                    {
                        Plc = MhCommunication.Plc;
                    }
                    var ret = Plc.Connect();
                    IsConnect = ret;
                    Send(ConnectResult,sender,ret);
                }
                catch (Exception e)
                {
                    Send(ConnectResult, sender, false);
                    SendTipsMessage(e.Message,sender);
                }

            });

            Register<string>(Close, (sender, ip) =>
            {
                try
                {
                    if (Plc == null)
                    {
                        Plc = new PlcOperator(ip);
                    }

                    var ret = Plc.Close();
                    IsConnect = false;
                    Send(CloseResult, sender, ret.IsSuccess);
                }
                catch (Exception e)
                {
                    Send(CloseResult, sender, false);
                    SendTipsMessage(e.Message, sender);
                }

            });

            Register<string>(BeginListen, (sender, arg) =>
            {
                if(IsListen) return;
                if (th == null)
                {
                    th = new Thread(Listen);
                }

                th.IsBackground = true;
                th.Start();
                IsListen = true;
            });

            Register<string>(StopListen, (sender, arg) =>
            {
                if (!IsListen) return;
                if (th == null)
                {
                    return;
                }
                th.Abort();
                IsListen = false;
            });

            Register<EMachineHandCommand>(Write, (sender, command) =>
            {
                try
                {
                    if (Status != 100)
                    {
                        SendTipsMessage("机械手未准备好");
                        return;
                    }

                    //if (FinishedValue != 0)
                    //{
                    //    SendTipsMessage("任务未结算，无法写入任务");
                    //    return;
                    //}

                    if (WriteValue != 0)
                    {
                        SendTipsMessage("上一个任务值未清0，无法写入任务");
                        return;
                    }

                    var ret = Plc.Write(_DefaultWriteAddr, Convert.ToInt16(command));
                    if (ret.IsSuccess)
                    {
                        SendTipsMessage("写入成功");
                    }
                    else
                    {
                        SendTipsMessage(ret.Message);
                    }

                }
                catch (Exception e)
                {
                    SendTipsMessage(e.Message,sender);
                }
                
                
                
            });

            Register<List<Command>>(Auto, ExecuteAuto);

            Register<string>(Stop, arg =>
            {
                if (AutoThread != null && AutoThread.IsAlive)
                {
                    AutoThread.Abort();
                }
            });

            Register<short>(FinishedTask, (sender, value) =>
            {
                try
                {
                    while (FinishedValue != value)
                    {
                        FinishedValue = value;
                        Thread.Sleep(500);
                    }
                }
                catch (Exception e)
                {
                    SendTipsMessage(e.Message, sender);
                }
            });

            Register<short>(ClearLastTask, (sender, value) =>
            {
                try
                {
                    while (MhCommunication.FinishedTaskFeedBack(100))
                    {
                        Thread.Sleep(500);
                    }
                }
                catch (Exception e)
                {
                    SendTipsMessage(e.Message, sender);
                }
            });
        }

        private bool IsFreeRequest => Status == 100;
        private bool IsClearBaseBoardRequest => FinishedValue == 17;


        private short Status => Plc.ReadShort(_DefaultStatusAddr);

        private short FinishedValue
        {
            get => Plc.ReadShort(_DefaultReadAddr);
            set => Plc.Write(_DefaultReadAddr, value);
        }

        private short WriteValue
        {
            get => Plc.ReadShort(_DefaultWriteAddr);
            set => Plc.Write(_DefaultWriteAddr, value);
        }

        

        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (IsConnect)
                    {
                        var status = Plc.ReadShort(_DefaultStatusAddr);
                        var writeValue = Plc.ReadShort(_DefaultWriteAddr);
                        var readValue = Plc.ReadShort(_DefaultReadAddr);
                        Send(BindingData, new Tuple<short, short, short>(status, writeValue, readValue));
                    }
                }
                catch (Exception e)
                {
                    SendTipsMessage(e.Message);
                }
                
                Thread.Sleep(500);
            }
        }

        private List<Command> Commands { get; set; }=new List<Command>();

        private void ExecuteAuto(object sender, List<Command> commands)
        {
            Commands = commands;
            AutoThread = new Thread(AutoExecuteCommands){IsBackground = true};
            AutoThread.Start();
        }

        private void AutoExecuteCommands()
        {
            RespondPlc();
            //for (int i = 0; i < 10; i++)
            //{
            //    foreach (var command in Commands)
            //    {
            //        while (!IsFreeRequest)
            //        {
            //            Thread.Sleep(30);
            //        }
            //        Send(ExecuteMsg,i+":收到空闲请求");
            //        if (WriteValue != 0)
            //        {
            //            WriteValue = 0;
            //        }
            //        while (WriteValue != command.CommandValue)
            //        {
            //            WriteValue = command.CommandValue;
            //            Thread.Sleep(30);
            //        }
            //        Send(ExecuteMsg, i + ":写入任务" + command.Name+"成功");
            //        //等待完成
            //        while (true)
            //        {
            //            if (FinishedValue == command.CommandValue)
            //            {
            //                Send(ExecuteMsg, i + ":任务" + command.Name + "执行完成");
            //                command.Status = i+1;
            //                break;
            //            }
            //            Thread.Sleep(30);
            //        }

            //        while (WriteValue != 0)
            //        {
            //            WriteValue = 0;
            //            Thread.Sleep(30);
            //        }
            //        Send(ExecuteMsg, i + ":任务" + command.Name + "清除成功");
            //        //if (!command.Status)
            //        //{

            //        //}
            //    }
            //}

        }

        private IMachineHand MhCommunication { get; set; } = new MachineHandCommunication();

        private void RespondPlc()
        {
            while (true)
            {
                if (Commands.Count == 0)
                {
                    Thread.Sleep(300);
                    continue;
                }

                if (MhCommunication.IsFinished)
                {
                    var finishedTask = MhCommunication.FinishedValue;
                    Send(ExecuteMsg,$"收到完成请求,完成任务号为：{finishedTask}");
                    var command = Commands.FirstOrDefault(item => item.IsRun && item.CommandValue==finishedTask);
                    if (command != null)
                    {
                        command.IsRun = false;
                    }

                    MhCommunication.FinishedTaskFeedBack(finishedTask);
                    //if (MhCommunication.WriteValue != 0)
                    //{
                    //    if (!MhCommunication.ClearLastTask())
                    //    {
                    //        Thread.Sleep(20);
                    //        MhCommunication.ClearLastTask();
                    //    }
                    //}
                    
                }

                //if (MhCommunication.IsFullBaseBoard)
                //{
                //    MhCommunication.ClearBoseBoard();
                //}

                if (MhCommunication.IsRequest)
                {
                    var commands = from cmd in Commands
                        orderby cmd.IsRun descending, cmd.ExecuteCount ,cmd.Index
                        select cmd;
                    var command = commands.ToArray()[0];
                    MhCommunication.WriteValue = 0;
                    MhCommunication.WriteValue = command.CommandValue;
                    if (command.IsRun)
                    {
                        //再次下发
                        continue;
                    }
                    command.IsRun = true;
                    command.ExecuteCount += 1;
                }

                Thread.Sleep(30);
            }
        }

    }

    public class Command
    {
        public string Name { get; set; }
        public short CommandValue { get; set; }
        public int ExecuteCount { get; set; } = 0;
        public bool IsRun { get; set; } = false;
        public int Index { get; set; }
    }
}
