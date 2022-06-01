//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：EdgebandingCommandBase.cs
//   创建时间：2018-12-13 16:06
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Commands
{
    public class EdgebandingCommandBase<TInput, TOutput> : CommandBase<TInput, TOutput>
        where TInput : EntityBase, new()
        where TOutput : EntityBase, new()
    {
        public EdgebandingCommandBase(string commandName) : base(commandName)
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

        protected override EntityBase Execute(IPlcConnector plc)
        {
            return Output;
        }

        private void DrillingCommandBase_CommandExecuted(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
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
            MqttManager.Current.Publish(EmqttSetting.Current.PcsCommandStart, new CommandArgs
            {
                CommandCode = CommandCode,
                Input = Input,
                Output = Output,
                Type = CommandType.S
            });
        }
    }
}