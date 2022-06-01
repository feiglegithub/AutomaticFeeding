//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：DrillingSetting.cs
//   创建时间：2018-11-28 16:14
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Drilling.Domain
{
    [IniFile]
    public class DrillingSetting : SettingBase<DrillingSetting>
    {
        public string Line { get; set; }

        [Property("data")] public int CacheDayStart { get; set; } = 0;

        [Property("data")] public int CacheDayEnd { get; set; } = 3;
        [Property("data")] public int InitQueueNumber { get; set; } = 200;

        [Property("general")]
        public int MachineInfoRefreshInterval { get; set; } = 10000;



    }
}