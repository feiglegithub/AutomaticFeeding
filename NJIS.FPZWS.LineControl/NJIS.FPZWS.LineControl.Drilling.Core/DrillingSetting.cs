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

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    [IniFile("DrillingSetting")]
    public class DrillingSetting : SettingBase<DrillingSetting>
    {
        [Property("core")]
        public string DrillingBuilder { get; set; } = "NJIS.FPZWS.LineControl.Drilling.Core.DefaultDrillingBuilder,NJIS.FPZWS.LineControl.Drilling.Core";
    }
}