using NJIS.Framework.Core.Configs;
using NJIS.Framework.Core.Initialize;
using NJIS.Framework.Core.Logging;

namespace NJIS.Framework.Logging.Log4Net
{
    /// <summary>
    ///     log4net日志初始化器，用于初始化基础日志功能
    /// </summary>
    public class Log4NetLoggingInitializer : LoggingInitializerBase, IBasicLoggingInitializer
    {
        /// <summary>
        ///     开始初始化基础日志
        /// </summary>
        /// <param name="config">日志配置信息</param>
        public void Initialize(LoggingConfig config)
        {
            LogManager.SetEntryInfo(config.EntryConfig.Enabled, config.EntryConfig.EntryLogLevel);
            foreach (var adapterConfig in config.BasicLoggingConfig.AdapterConfigs)
            {
                SetLoggingFromAdapterConfig(adapterConfig);
            }
        }
    }
}