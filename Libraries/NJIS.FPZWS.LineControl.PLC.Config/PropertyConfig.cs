//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：PropertyConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    public class PropertyConfig
    {
        public PropertyConfig()
        {
        }

        public PropertyConfig(string name, string map)
        {
            Name = name;
            Map = map;
        }

        /// <summary>
        ///     属性
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     PLC 标签
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        ///     读取的数据长度
        ///     对String,Binary类型有效
        /// </summary>
        public int Length { get; set; } = 20;

        /// <summary>
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///     是否映射PLC值
        /// </summary>
        public bool IsMap { get; set; }

        /// <summary>
        /// </summary>
        public string ValueType { get; set; }

        public bool IsCheck { get; set; } = false;

        public int WriteIndex { get; set; } = -1;
    }
}