//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：StackListSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class StackListSetting : ConfigWapper<StackListSetting>
    {
        /// <summary>
        /// StackList 算法
        /// </summary>
        public string StackListAlgorithm { get; set; }

        /// <summary>
        /// 开料锯数量
        /// </summary>
        public int CuttingMachineNumber { get; set; } = 5;

        /// <summary>
        /// 最大花色数量
        /// </summary>
        public int MaxColorNumber { get; set; } = 3;

        public string Department { get; set; }

        /// <summary>
        /// 每剁最大板件数量
        /// </summary>
        public int MaxPanelNumber { get; set; } = 40;

        /// <summary>
        /// 0代表未混色 修改成0以外的数字表示混色
        /// </summary>
        public int Mixture { get; set; }
        public override string Path { get; protected set; } = "生产数据/StackListSetting";
    }
}