using NJIS.Framework.Core.Configs;
using NJIS.Framework.Core.Dependency;
using NJIS.Framework.Core.Initialize;

namespace NJIS.Framework.Logging.Log4Net
{
    /// <summary>
    ///     服务映射信息集合扩展操作
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     添加Log4Net日志功能相关映射信息
        /// </summary>
        public static void AddLog4NetServices(this IServiceCollection services)
        {
            if (FrameworkConfig.LoggingConfigReseter == null)
            {
                FrameworkConfig.LoggingConfigReseter = new Log4NetLoggingConfigReseter();
            }
            services.AddSingleton<IBasicLoggingInitializer, Log4NetLoggingInitializer>();
            services.AddSingleton<Log4NetLoggerAdapter>();
        }
    }
}