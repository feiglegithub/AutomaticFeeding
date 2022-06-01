//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：MprSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class MprSetting : ConfigWapper<MprSetting>
    {
        public string MachineCodes { get; set; }

        public int Timeout { get; set; }
        public int MaxThreadCount { get; set; }
        public string UserName { get; set; }

        public string Pwd { get; set; }

        public string MprFileExt { get; set; }
        public string Url { get; set; }
        public string Domain { get; set; }
        public override string Path { get; protected set; } = "生产数据/MprSetting";
    }
}