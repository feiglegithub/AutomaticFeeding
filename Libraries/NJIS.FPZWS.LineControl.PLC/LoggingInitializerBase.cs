#region

using System;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.PLC.Config;
using NJIS.FPZWS.Log;

#endregion

namespace NJIS.FPZWS.LineControl.PLC
{
    /// <summary>
    ///     Log initialization base class
    /// </summary>
    public class LoggingInitializerBase : IInitializer
    {
        public virtual void Initializer(IConfig dgConfig)
        {
            var config = PlcConfig.Current.Logging.AdapterConfig;
            if (!config.Enabled)
            {
                return;
            }
            var loggerAdapter = Activator.CreateInstance(Type.GetType(config.Type));
            var adapter = loggerAdapter as ILoggerAdapter;

            if (adapter == null)
            {
                return;
            }
            LogManager.AddLoggerAdapter(adapter);
        }

        public InitializeLevel Level
        {
            get { return InitializeLevel.High; }
        }
    }
}