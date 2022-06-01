//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：PositionCommand.cs
//   创建时间：2018-12-13 16:56
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:56
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Entitys;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Commands
{
    /// <summary>
    ///     位置交互
    /// </summary>
    public class PositionCommand : DbProcCommand<PositionInputEntity, PositionOutputEntity>
    {
        public PositionCommand() : this("PositionCommand")
        {
        }

        public PositionCommand(string commandName) : base(commandName)
        {
        }

        private string PreData { get; set; }

        public override bool LoadInput(IPlcConnector plc)
        {
            // 采集到的数据不能为空，并且和上一片数据不能相同
            var rst = base.LoadInput(plc);
            if (rst)
            {
                if (string.IsNullOrEmpty(Input.Data) && PreData == Input.Data)
                {
                    return false;
                }

                PreData = Input.Data;
            }

            return rst;
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            base.Execute(plc);

            // 发送Mqtt命令
            MqttManager.Current.Publish(EmqttSetting.Current.PcsPositionRep, new PositionArgs
            {
                Place = Input.Position,
                PartId = Input.Data,
                NextPlace = Output.NextPlace
            });
            return Output;
        }
    }
}