using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Domain.Control.Entitys;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Commands
{
    public class PositionCommand : DbProcCommand<PositionInputEntity, PositionOutputEntity>
    {
        public PositionCommand():base(nameof(PositionCommand))
        {
        }

        public PositionCommand(string commandCode) : base(commandCode)
        {
        }

        private string PreData { get; set; }

        public override bool LoadInput(IPlcConnector plc)
        {
            var ret = base.LoadInput(plc);
            if (ret)
            {
                if (string.IsNullOrWhiteSpace(Input.Data) && PreData == Input.Data)
                {
                    return false;
                }

                PreData = Input.Data;
            }

            return ret;
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            base.Execute(plc);
            MqttManager.Current.Publish(EmqttSettings.Current.PcsPartInfoPositionRep, new PositionArgs()
            {
                Position = Input.Position,
                PartId = Input.Data
            });
            return Output;
        }
    }
}
