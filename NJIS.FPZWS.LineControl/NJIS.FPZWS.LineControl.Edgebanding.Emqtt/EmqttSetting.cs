//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：EmqttSetting.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    [IniFile]
    public class EmqttSetting : SettingBase<EmqttSetting>
    {
        public const string EdgebandingTopic = "/sfy/rx/pcs/edgebanding/3";

        [Property("topic")] public string EdgebandingReq { get; set; } = $"{EdgebandingTopic}/req/#";

        #region 报警

        [Property("topic")] public string PcsAlarmRep { get; set; } = $"{EdgebandingTopic}/rep/alarm";

        #endregion

        #region 入板

        [Property("topic")] public string PcsInPartRep { get; set; } = $"{EdgebandingTopic}/rep/InPart";

        #endregion


        #region 消息

        [Property("topic")] public string PcsMsg { get; set; } = $"{EdgebandingTopic}/msg";

        #endregion

        #region 位置请求

        [Property("topic")] public string PcsPositionRep { get; set; } = $"{EdgebandingTopic}/rep/position";

        #endregion

        #region 初始化

        [Property("topic")] public string PcsInitReq { get; set; } = $"{EdgebandingTopic}/req/init";

        [Property("topic")] public string PcsInitRep { get; set; } = $"{EdgebandingTopic}/rep/init";

        #endregion

        #region 命令

        [Property("topic")] public string PcsCommand { get; set; } = $"{EdgebandingTopic}/command/#";

        [Property("topic")] public string PcsCommandStart { get; set; } = $"{EdgebandingTopic}/command/s";

        [Property("topic")] public string PcsCommandEnd { get; set; } = $"{EdgebandingTopic}/command/e";

        #endregion

        #region 入板队列

        [Property("topic")] public string PcsInitQueueReq { get; set; } = $"{EdgebandingTopic}/req/initQueue";

        [Property("topic")] public string PcsInitQueueRep { get; set; } = $"{EdgebandingTopic}/rep/initQueue";


        [Property("topic")] public string PcsDeleteQueueReq { get; set; } = $"{EdgebandingTopic}/req/deleteQueue";

        [Property("topic")] public string PcsDeleteQueueRep { get; set; } = $"{EdgebandingTopic}/rep/deleteQueue";

        #endregion


        #region 入板队列

        [Property("topic")]
        public string PcsPartInfoPositionReq { get; set; } = $"{EdgebandingTopic}/req/PartInfoPositionCommand";

        [Property("topic")]
        public string PcsPartInfoPositionRep { get; set; } = $"{EdgebandingTopic}/rep/PartInfoPositionCommand";

        #endregion

        #region 链式缓存

        [Property("topic")] public string PcsChainBufferRep { get; set; } = $"{EdgebandingTopic}/InitChainBuffer";


        [Property("topic")]
        public string PcsInitChainBufferReq { get; set; } = $"{EdgebandingTopic}/req/InitChainBuffer";

        [Property("topic")]
        public string PcsInitChainBufferRep { get; set; } = $"{EdgebandingTopic}/rep/InitChainBuffer";


        [Property("topic")]
        public string PcsChanageStatusChainBufferReq { get; set; } = $"{EdgebandingTopic}/req/ChanageStatusChainBuffer";

        [Property("topic")]
        public string PcsChanageStatusChainBufferRep { get; set; } = $"{EdgebandingTopic}/rep/ChanageStatusChainBuffer";


        [Property("topic")]
        public string PcsDeletePartChainBufferReq { get; set; } = $"{EdgebandingTopic}/req/DeletePartChainBuffer";

        [Property("topic")]
        public string PcsDeletePartChainBufferRep { get; set; } = $"{EdgebandingTopic}/rep/DeletePartChainBuffer";

        #endregion
    }
}