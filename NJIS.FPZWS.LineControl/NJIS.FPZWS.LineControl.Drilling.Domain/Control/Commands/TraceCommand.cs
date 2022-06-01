//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：TraceCommand.cs
//   创建时间：2018-11-23 16:28
//   作    者：
//   说    明：
//   修改时间：2018-11-23 16:28
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Commands
{
    /// <summary>
    ///     位置跟踪
    /// </summary>
    public class TraceCommand : DrillingCommandBase<ChainBufferInputEntity, EmptyEntityBase>
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(TraceCommand).Name);

        public TraceCommand() : base("TraceCommand")
        {
        }

        public override EntityBase ExecuteCommand(IPlcConnector plc)
        {
            // todo 跟新队列中板件状态
            return base.ExecuteCommand(plc);
        }
    }
}