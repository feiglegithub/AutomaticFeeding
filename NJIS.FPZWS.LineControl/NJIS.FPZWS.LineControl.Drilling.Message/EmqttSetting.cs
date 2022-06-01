//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Message
//   文 件 名：EmqttSetting.cs
//   创建时间：2018-11-28 16:17
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:17
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Drilling.Message
{
    [IniFile]
    public class EmqttSetting : SettingBase<EmqttSetting>
    {
        public const string DrillingTopic = "/sfy/rx/pcs/drilling/3";

        [Property("topic")] public string DrillingReq { get; set; } = $"{DrillingTopic}/req/#";

        #region 报警

        [Property("topic")] public string PcsAlarmRep { get; set; } = $"{DrillingTopic}/rep/alarm";

        #endregion

        #region 入板

        [Property("topic")] public string PcsInPartRep { get; set; } = $"{DrillingTopic}/rep/InPart";

        #endregion


        #region 消息

        [Property("topic")] public string PcsMsg { get; set; } = $"{DrillingTopic}/msg";

        #endregion

        #region 位置请求

        [Property("topic")] public string PcsPositionRep { get; set; } = $"{DrillingTopic}/rep/position";

        #endregion

        #region 初始化

        [Property("topic")] public string PcsInitReq { get; set; } = $"{DrillingTopic}/req/init";

        [Property("topic")] public string PcsInitRep { get; set; } = $"{DrillingTopic}/rep/init";

        #endregion

        #region 命令

        [Property("topic")] public string PcsCommand { get; set; } = $"{DrillingTopic}/command/#";

        [Property("topic")] public string PcsCommandStart { get; set; } = $"{DrillingTopic}/command/s";

        [Property("topic")] public string PcsCommandEnd { get; set; } = $"{DrillingTopic}/command/e";

        #endregion

        #region 入板队列

        [Property("topic")] public string PcsInitQueueReq { get; set; } = $"{DrillingTopic}/req/initQueue";

        [Property("topic")] public string PcsInitQueueRep { get; set; } = $"{DrillingTopic}/rep/initQueue";


        [Property("topic")] public string PcsDeleteQueueReq { get; set; } = $"{DrillingTopic}/req/deleteQueue";

        [Property("topic")] public string PcsDeleteQueueRep { get; set; } = $"{DrillingTopic}/rep/deleteQueue";

        #endregion  

        #region 入板队列

        [Property("topic")]
        public string PcsPartInfoPositionReq { get; set; } = $"{DrillingTopic}/req/PartInfoPositionCommand";

        [Property("topic")]
        public string PcsPartInfoPositionRep { get; set; } = $"{DrillingTopic}/rep/PartInfoPositionCommand";

        #endregion

        #region 链式缓存

        [Property("topic")] public string PcsChainBufferRep { get; set; } = $"{DrillingTopic}/InitChainBuffer";


        [Property("topic")] public string PcsInitChainBufferReq { get; set; } = $"{DrillingTopic}/req/InitChainBuffer";

        [Property("topic")] public string PcsInitChainBufferRep { get; set; } = $"{DrillingTopic}/rep/InitChainBuffer";


        [Property("topic")]
        public string PcsChanageStatusChainBufferReq { get; set; } = $"{DrillingTopic}/req/ChanageStatusChainBuffer";

        [Property("topic")]
        public string PcsChanageStatusChainBufferRep { get; set; } = $"{DrillingTopic}/rep/ChanageStatusChainBuffer";


        [Property("topic")]
        public string PcsDeletePartChainBufferReq { get; set; } = $"{DrillingTopic}/req/DeletePartChainBuffer";

        [Property("topic")]
        public string PcsDeletePartChainBufferRep { get; set; } = $"{DrillingTopic}/rep/DeletePartChainBuffer";

        #endregion

        #region Ng  
        [Property("topic")]
        public string PcsNgInitReq { get; set; } = $"{DrillingTopic}/req/pcsNgInit";

        [Property("topic")]
        public string PcsNgReq { get; set; } = $"{DrillingTopic}/req/pcsNg";

        [Property("topic")]
        public string PcsNgRep { get; set; } = $"{DrillingTopic}/rep/pcsNg";

        [Property("topic")]
        public string DeletePcsNgReq { get; set; } = $"{DrillingTopic}/req/deletePcsNg";

        #endregion

        #region Machine  
        [Property("topic")]
        public string PcsMachineInitReq { get; set; } = $"{DrillingTopic}/req/machineInit";

        [Property("topic")]
        public string PcsMachineInitRep { get; set; } = $"{DrillingTopic}/rep/machineInit";

        [Property("topic")]
        public string PcsMachineUpdateReq { get; set; } = $"{DrillingTopic}/req/mcs";

        #endregion
    }
}