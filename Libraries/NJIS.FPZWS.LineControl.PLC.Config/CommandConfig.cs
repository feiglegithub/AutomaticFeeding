//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：CommandConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    public class CommandConfig
    {
        public CommandConfig()
        {
            InputConfig = new EntityConfig();
            OutputConfig = new EntityConfig();
            CommandType = 0;
            IsClearData = true;
        }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public EntityConfig InputConfig { get; set; }

        public EntityConfig OutputConfig { get; set; }

        /// <summary>
        ///     命令类型
        ///     0:触发式命令（default)-> 通过trigger触发
        ///     1:普通命令
        /// </summary>
        public int CommandType { get; set; }

        /// <summary>
        ///     是否清除数据
        ///     读取WinCC数据后，清除数据
        /// </summary>
        public bool IsClearData { get; set; }

        /// <summary>
        ///     是否为同步命令
        ///     异步会单独开线程
        /// </summary>
        public bool IsSync { get; set; }

        public int CommandExecutInterval { get; set; }

        /// <summary>
        ///     Type 是否相等
        /// </summary>
        /// <returns></returns>
        public bool IsType(Type type)
        {
            if (Type == null) return false;
            var t = System.Type.GetType(Type);
            if (t == null) return false;
            return type.Name == t.Name && type.Namespace == t.Namespace;
        }
    }
}