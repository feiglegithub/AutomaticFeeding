//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Service
//   文 件 名：DrillingDbSetting.cs
//   创建时间：2018-11-26 9:56
//   作    者：
//   说    明：
//   修改时间：2018-11-26 9:56
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Drilling.Service
{
    [IniFile]
    internal class DrillingDbSetting : SettingBase<DrillingDbSetting>
    {
        internal string DbConnect => $"Password={Pwd};Persist Security Info=True;" +
                                     $"User ID={UserName};" +
                                     $"Initial Catalog={Database};" +
                                     $"Data Source={Server}";


        [Property("common")] public string Server { get; set; }

        [Property("common")] public string Database { get; set; }

        [Property("common")] public string UserName { get; set; }

        [Property("common")] public string Pwd { get; set; }
    }
}