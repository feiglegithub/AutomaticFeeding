using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Commands
{
    public class CuttingCommandBase<TInput, TOutput> : CommandBase<TInput, TOutput>
    where TInput : EntityBase, new()
    where TOutput : EntityBase, new()
    {

        public CuttingCommandBase(string commandCode) : base(commandCode)
        {
            this.CommandExecuting += CuttingCommandBase_CommandExecuting;
            this.CommandExecuted += CuttingCommandBase_CommandExecuted;
        }

        private void CuttingCommandBase_CommandExecuted(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            MqttManager.Current.Publish(EmqttSettings.Current.PcsCommandEnd,new CommandArgs()
            {
                CommandCode = this.CommandCode,
                Input = this.Input,
                Output = this.Output,
                Type = CommandType.E 
            });
        }

        private void CuttingCommandBase_CommandExecuting(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            MqttManager.Current.Publish(EmqttSettings.Current.PcsCommandStart, new CommandArgs()
            {
                CommandCode = this.CommandCode,
                Input = this.Input,
                Output = this.Output,
                Type = CommandType.S
            });
        }

        public override bool LoadInput(IPlcConnector plc)
        {
            if (base.LoadInput(plc))
            {
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
    }
}
