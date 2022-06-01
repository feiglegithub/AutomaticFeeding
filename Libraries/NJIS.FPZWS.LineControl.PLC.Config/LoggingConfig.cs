//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：LoggingConfig.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using NJIS.FPZWS.LineControl.PLC.Config.ConfigFile;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config
{
    /// <summary>
    ///     Logging WinCC
    /// </summary>
    public class LoggingConfig
    {
        public LoggingConfig()
        {
        }

        public LoggingConfig(LoggingElement element)
        {
            AdapterConfig = new LoggingAdapterConfig
            {
                Enabled = element.Adapter.Enabled,
                Type = element.Adapter.Type,
                Name = element.Adapter.Name
            };

            EntryConfig = new LoggingEntryConfig
            {
                Enabled = element.Entry.Enabled,
                Level = element.Entry.Level
            };
        }

        public LoggingAdapterConfig AdapterConfig { get; set; }

        public LoggingEntryConfig EntryConfig { get; set; }
    }
}