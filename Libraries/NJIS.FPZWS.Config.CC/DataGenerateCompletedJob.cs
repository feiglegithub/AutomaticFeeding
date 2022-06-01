//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：DataGenerateCompletedJob.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class DataGenerateCompletedJob : ConfigWapper<DataGenerateCompletedJob>
    {
        /// <summary>
        ///     1:自动下发，2:定时下发，3:手动下发
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///     数据下发时间
        /// </summary>
        public string DistributeTimes { get; set; }

        public override string Path { get; protected set; } = "生产数据/DataGenerateCompletedJob";
    }
}