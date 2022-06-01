//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.MprImport
//   文 件 名：MprImportSetting.cs
//   创建时间：2019-07-25 9:47
//   作    者：
//   说    明：
//   修改时间：2019-07-25 9:47
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Drilling.MprImport
{
    [IniFile]
    public class MprImportSetting : SettingBase<MprImportSetting>
    {
        [Property("mprDown")] public string OneMprPath { get; set; }

        [Property("mprDown")] public int OneMprStorageDay { get; set; } = 10;

        [Property("mprDown")] public string DoubleMprPath { get; set; }

        [Property("mprDown")] public int DoubleMprStorageDay { get; set; } = 10;
        [Property("mprDown")] public bool MprDownEnable { get; set; } = false;
        [Property("homagCsv")] public string HomagCsvPath { get; set; }
        [Property("homagCsv")] public int HomagCsvCsvStoreDay { get; set; } = 10;

        [Property("homagCsv")] public bool HomagCsvCsvEnable { get; set; } = false;

        [Property("homagData")] public string HomagDataService { get; set; }
        [Property("homagData")] public string HomagDataDatabase { get; set; }
        [Property("homagData")] public string HomagDataUser { get; set; }
        [Property("homagData")] public string HomagDataPwd { get; set; }

        [Property("homagData")] public bool HomagDataServerEnable { get; set; } = false;


        [Property("general")] public string Machine { get; set; }

        [Property("general")] public int MprInterval { get; set; }
        [Property("general")] public int ImportDataForDay { get; set; } = -10;


    }
}