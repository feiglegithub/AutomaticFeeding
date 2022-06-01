//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：AlarmCommand.cs
//   创建时间：2018-12-14 11:52
//   作    者：
//   说    明：
//   修改时间：2018-12-14 11:52
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Commands
{
    public class BoolAlarmCommand : AlarmCommand<bool>
    {
        public BoolAlarmCommand() : base("BoolAlarmCommand")
        {
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            base.Execute(plc);
            if (Output.Res)
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep,
                    new PlcAlarmArgs(Input.ParamName, Input.AlarmDescribe)
                    {
                        StarTime = StartTime,
                        EndTime = EndTime,
                        AlarmId = AlarmId
                    });
            }

            return Output;
        }
    }

    public class IntAlarmCommand : AlarmCommand<int>
    {
        public IntAlarmCommand() : base("IntAlarmCommand")
        {
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            base.Execute(plc);
            if (Output.Res)
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep,
                    new PlcAlarmArgs(Input.ParamName, Input.AlarmDescribe));
            }

            return Output;
        }
    }
}