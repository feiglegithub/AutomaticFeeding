//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：ConnectorConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    public class ConnectorConfig
    {
        public ConnectorConfig()
        {
            Commands = new List<CommandConfig>();
        }

        public int ReaderInterval { get; set; } = 250;

        public string Name { get; set; }

        /// <summary>
        ///     PLC 类型
        /// </summary>
        public string PlcType { get; set; }

        public string Address { get; set; }
        public int Port { get; set; }

        /// <summary>
        ///     连接超时时间
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        ///     接收数据超时时间
        /// </summary>
        public int ReceiveTimeOut { get; set; }


        /// <summary>
        ///     是否测试
        /// </summary>
        public bool IsDebug { get; set; } = false;

        /// <summary>
        ///     命令多线程
        /// </summary>
        public bool IsCommandMultiThreading { get; set; }

        /// <summary>
        ///     是否模拟
        /// </summary>
        public bool IsSimulator { get; set; } = false;

        public string SimulatorType { get; set; }

        public string Type { get; set; }


        public List<CommandConfig> Commands { get; set; }
    }
}