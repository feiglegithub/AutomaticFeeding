//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Client
//   文 件 名：EdgebandingSetting.cs
//   创建时间：2018-12-13 16:46
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:46
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Edgebanding.Client
{
    [IniFile]
    public class EdgebandingSetting : SettingBase<EdgebandingSetting>
    {
        [Property("net")] public string CheckIp { get; set; }

        [Property("net")] public int CheckTimeOut { get; set; } = 120;

        [Property("net")] public int CheckCnt { get; set; } = 20;

        [Property("ui")] public int PartInfoQueueMaxRecordCount { get; set; } = 200;

        [Property("ui")] public int MsgMaxRecordCount { get; set; } = 200;

        [Property("ui")] public int CommandMaxRecordCount { get; set; } = 200;

        [Property("ui")] public int AlarmMaxRecordCount { get; set; } = 200;

        [Property("ui")] public bool EnableAlarmForm { get; set; } = true;

        [Property("ui")] public bool EnableAnalyzeForm { get; set; } = true;

        [Property("ui")] public bool EnableMachine { get; set; } = true;

        [Property("ui")] public bool EnableCommandForm { get; set; } = true;


        [Property("ui")] public bool EnableMsgForm { get; set; } = true;

        [Property("ui")] public bool EnablePartInfoQueueForm { get; set; } = true;
    }
}