//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：DrillingSetting.cs
//   创建时间：2018-11-29 15:05
//   作    者：
//   说    明：
//   修改时间：2018-11-29 15:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    [IniFile]
    public class DrillingSetting : SettingBase<DrillingSetting>
    {
        [Property("net")]
        public string CheckIp { get; set; }

        [Property("net")]
        public int CheckTimeOut { get; set; } = 120;

        [Property("net")]
        public int CheckCnt { get; set; } = 20;

        [Property("ui")]
        public int PartInfoQueueMaxRecordCount { get; set; } = 200;

        [Property("ui")]
        public int MsgMaxRecordCount { get; set; } = 200;
        [Property("ui")]
        public int CommandMaxRecordCount { get; set; } = 200;
        [Property("ui")]
        public int AlarmMaxRecordCount { get; set; } = 200;

        [Property("ui")]
        public bool EnableAlarmForm { get; set; } = true;

        [Property("ui")]
        public bool EnableAnalyzeForm { get; set; } = true;

        [Property("ui")]
        public bool EnableChainBuffer { get; set; } = true;

        [Property("ui")]
        public bool EnableCommandForm { get; set; } = true;

        [Property("ui")]
        public bool EnablePartInfoTraceForm { get; set; } = true;

        [Property("ui")]
        public bool EnableMsgForm { get; set; } = true;

        [Property("ui")]
        public bool EnablePartInfoQueueForm { get; set; } = true;

        [Property("ui")]
        public bool EnableMachineForm { get; set; } = true;
    }
}