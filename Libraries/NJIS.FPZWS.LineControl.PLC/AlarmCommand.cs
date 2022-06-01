//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：AlarmCommand.cs
//   创建时间：2018-12-14 14:39
//   作    者：
//   说    明：
//   修改时间：2018-12-14 14:39
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.LineControl.PLC.Entitys;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.PLC
{
    public class AlarmCommand<T> : CommandBase<AlarmInputEntity<T>, AlarmOutputEntity<T>>
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(AlarmCommand<T>).Name);

        public AlarmCommand(string commandName) : base(commandName)
        {
        }

        public string AlarmId { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            Output.Res = false;
            try
            {
                if (Input.AlarmTrigger())
                {
                    if (string.IsNullOrEmpty(AlarmId))
                    {
                        AlarmId = Guid.NewGuid().ToString();
                        StartTime = DateTime.Now;
                        EndTime = DateTime.Now;
                    }
                    else
                    {
                        EndTime = DateTime.Now;
                    }

                    Output.Res = true;
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }

            return Output;
        }
    }

    public class BoolAlarmCommand : AlarmCommand<bool>
    {
        public BoolAlarmCommand() : base("BoolAlarmCommand")
        {
        }
    }

    public class IntAlarmCommand : AlarmCommand<int>
    {
        public IntAlarmCommand() : base("IntAlarmCommand")
        {
        }
    }
}