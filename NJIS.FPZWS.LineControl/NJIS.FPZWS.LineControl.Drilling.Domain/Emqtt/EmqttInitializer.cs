//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：EmqttInitializer.cs
//   创建时间：2018-11-28 16:09
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:09
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     Emqtt 初始化器
    /// </summary>
    public class EmqttInitializer : IModularStarter
    {
        public readonly static List<IEmqttCommand> Commands = new List<IEmqttCommand>();
        private readonly ILogger _logger = LogManager.GetLogger(typeof(EmqttInitializer));

        public void Start()
        {
            _logger.Info($"========Start Emqtt========");
            var finder = new DirectoryReflectionFinder();
            var initializerTypes = finder.GetTypeFromAssignable<IEmqttCommand>();

            foreach (var type in initializerTypes)
            {
                _logger.Info($"创建 Emqtt 命令[{type.FullName}]");
                var command = Activator.CreateInstance(type) as IEmqttCommand;
                if (command == null)
                {
                    throw new Exception("App initialization failed");
                }

                Commands.Add(command);
            }

            _logger.Info($"========End Emqtt========");
        }

        public void Stop()
        {
        }

        public StarterLevel Level { get; }
    }
}