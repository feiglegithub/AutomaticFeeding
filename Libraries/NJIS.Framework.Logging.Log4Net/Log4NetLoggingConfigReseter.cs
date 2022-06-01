using NJIS.Framework.Core.Configs;

namespace NJIS.Framework.Logging.Log4Net
{
    /// <summary>
    ///     Log4Net日志配置信息重置类
    /// </summary>
    public class Log4NetLoggingConfigReseter : ILoggingConfigReseter
    {
        /// <summary>
        ///     日志配置信息重置
        /// </summary>
        /// <param name="config">待重置的日志配置信息</param>
        /// <returns>重置后的日志配置信息</returns>
        public LoggingConfig Reset(LoggingConfig config)
        {
            if (config.BasicLoggingConfig.AdapterConfigs.Count == 0)
            {
                config.BasicLoggingConfig.AdapterConfigs.Add(new LoggingAdapterConfig
                {
                    AdapterType = typeof (Log4NetLoggerAdapter)
                });
            }
            return config;
        }
    }
}