//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：PackingSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.Config.CC
{
    public class PackingSetting : ConfigWapper<PackingSetting>
    {
        private const string WorkSection = "work";

        public PackingSetting()
        {
            PackingAlgorithmTimeout = 60000;
            if (string.IsNullOrEmpty(PackingFilePath))
                PackingFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks");
        }


        public int TaskMonitorInterval { get; set; }
        public int WorkInterval { get; set; }

        /// <summary>
        ///     包装规划文件路径
        /// </summary>
        public string PackingFilePath { get; set; }

        /// <summary>
        ///     包装算法执行程序
        /// </summary>
        public string PackingAlgorithmExecuteFile { get; set; }

        /// <summary>
        ///     包装算法执行程序超时时间
        /// </summary>
        public int PackingAlgorithmTimeout { get; set; }

        public int TaskFileStorageDay { get; set; } = 1;

        /// <summary>
        ///     纸皮类别
        /// </summary>
        public string PaperType { get; set; }

        /// <summary>
        ///     护脚宽度
        /// </summary>
        public int FootProtection { get; set; }

        /// <summary>
        ///     折边宽度
        /// </summary>
        public int Hem { get; set; }

        public override string Path { get; protected set; } = "生产数据/PackingSetting";
    }
}