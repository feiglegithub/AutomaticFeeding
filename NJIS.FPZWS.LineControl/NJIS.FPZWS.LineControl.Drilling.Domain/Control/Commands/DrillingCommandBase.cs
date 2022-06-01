//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：DrillingCommandBase.cs
//   创建时间：2018-11-22 8:45
//   作    者：
//   说    明：
//   修改时间：2018-11-22 8:45
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Commands
{
    public class DrillingCommandBase<TInput, TOutput> : CommandBase<TInput, TOutput>
        where TInput : EntityBase, new()
        where TOutput : EntityBase, new()
    {
        public DrillingCommandBase(string commandName) : base(commandName)
        {
            CommandExecuting += DrillingCommandBase_CommandExecuting;
            CommandExecuted += DrillingCommandBase_CommandExecuted;
        }

        public override bool LoadInput(IPlcConnector plc)
        {
            if (base.LoadInput(plc))
            {
                //重置输出
                Output.TriggerOut = Input.TriggerOut;
                Output.Trigger = false;
                return true;
            }

            return false;
        }

        protected DateTime BeginTime { get; set; }
        protected DateTime EndTime { get; set; }

        private void DrillingCommandBase_CommandExecuted(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            EndTime = DateTime.Now;

            var totalTime = (EndTime - BeginTime).TotalMilliseconds;
            _logger.Debug($"Command[{this.CommandCode}]执行结束,共耗费:{totalTime}");
            MqttManager.Current.Publish(EmqttSetting.Current.PcsCommandEnd, new CommandArgs
            {
                CommandCode = CommandCode,
                Input = Input,
                Output = Output,
                Type = CommandType.E
            });
        }

        private void DrillingCommandBase_CommandExecuting(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            BeginTime = DateTime.Now;

            _logger.Debug($"Command[{this.CommandCode}]执行开始");
            MqttManager.Current.Publish(EmqttSetting.Current.PcsCommandStart, new CommandArgs
            {
                CommandCode = CommandCode,
                Input = Input,
                Output = Output,
                Type = CommandType.S
            });
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            return Output;
        }
    }
}