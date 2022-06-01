//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Trial.Produce.ERP
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：PackingFeedbackForSortingDbSetting.cs
//   创建时间：2018-10-24 10:46
//   作    者：
//   说    明：
//   修改时间：2018-10-24 10:46
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class PackingFeedbackForSortingDbSetting : ConfigWapper<PackingFeedbackForSortingDbSetting>
    {
        public string SortingDbConnects { get; set; }

        public override string Path { get; protected set; } = "Feedback/PackingFeedbackForSortingDbSetting";
    }
}