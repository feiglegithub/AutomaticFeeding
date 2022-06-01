using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Emqtt
{
    /// <summary>
    /// Mqtt模块加载器
    /// </summary>
    public class EmqttInitializer//:IModularStarter
    {
        public static List<IEmqttCommand> Commands = new List<IEmqttCommand>();
        private readonly ILogger _log = LogManager.GetLogger<EmqttInitializer>();
        public void Start()
        {
            var finder = new DirectoryReflectionFinder();
            var initalizerTypes = finder.GetTypeFromAssignable<IEmqttCommand>();
            foreach (var type in initalizerTypes)
            {
                IEmqttCommand command = Activator.CreateInstance(type) as IEmqttCommand;
                if (command == null)
                {
                    throw new Exception("App initialization failed");
                }
                Commands.Add(command);
            }
        }

        public void Stop()
        {
            //throw new NotImplementedException();
        }

        public StarterLevel Level { get; }
    }
}
