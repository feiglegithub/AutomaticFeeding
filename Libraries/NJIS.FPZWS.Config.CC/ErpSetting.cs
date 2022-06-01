//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：ErpSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class ErpSetting : ConfigWapper<ErpSetting>
    {
        public string Orgs { get; set; }
        public string SyncFlag { get; set; }
        public int SyncDateDiff { get; set; }
        public int MessageEndDateDiff { get; set; }

        public int MessageBeginDateDiff { get; set; } = 0;
        public string BatchFilterForDescription { get; set; }
        public override string Path { get; protected set; } = "生产数据/ErpSetting";
    }
}