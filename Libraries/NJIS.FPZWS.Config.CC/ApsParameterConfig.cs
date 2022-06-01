//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：ApsParameterConfig.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class ApsParameterConfig : ConfigWapper<ApsParameterConfig>
    {
        /// <summary>
        ///     Ou
        /// </summary>
        public int Orgs { get; set; }

        /// <summary>
        ///     排产类型
        /// </summary>
        public int ScheduleType { get; set; }

        /// <summary>
        ///     如果为ERP排产，是否启用加工范围过滤
        /// </summary>
        public bool TechniqueEnable { get; set; }

        /// <summary>
        ///     排产内部批次最大板件数量（内部排产用）
        /// </summary>
        public int BatchNeedPartCount { get; set; }

        /// <summary>
        ///     如果同一板件类型板件数量小于此数量，则为小半件类型，优先集中分到同一批
        /// </summary>
        public int SmallerNumberTypeOfPartType { get; set; }

        public override string Path { get; protected set; } = "生产数据/ApsParameterConfig";
    }
}