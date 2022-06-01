// ************************************************************************************
//  解决方案：NJIS.FPZWS.Packing
//  项目名称：NJIS.FPZWS.Packing.Domain
//  文 件 名：AppConfigInitializer.cs
//  创建时间：2018-06-21 14:41
//  作    者：
//  说    明：
//  修改时间：2018-06-21 14:38
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using NJIS.ConfigurationCenter.Client;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;
using ILog = NJIS.ConfigurationCenter.Client.ILog;

namespace NJIS.FPZWS.App
{
    public class AppConfigStarter : IModularStarter
    {
        public AppConfigStarter()
        {
            Level = StarterLevel.High;
        }

        public void Start()
        {
            ConfigurationCenterStarter.SetLog(new Log());
        }

        public void Stop()
        {
            ConfigurationCenterStarter.Stop();
        }

        public StarterLevel Level { get; }
    }

    public class Log : ILog
    {
        private ILogger _logger = LogManager.GetLogger("CC");
        public void Error(string errorText)
        {
            _logger.Error(errorText);
        }
        public void Error(Exception e)
        {
            _logger.Error(e);
        }
        public void Info(string InfoText)
        {
            _logger.Info(InfoText);
        }
    }
}