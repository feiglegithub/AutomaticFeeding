//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.LineControl.Cutting.Core
//   文 件 名：CuttingApp.cs
//   创建时间：2018-11-19 15:54
//   作    者：
//   说    明：
//   修改时间：2018-11-19 15:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.PatternCore
{
    public class PatternApp : AppBase<PatternApp>, IService
    {
        private ILogger _log = LogManager.GetLogger(typeof(PatternApp).Name);

        public override bool Start()
        {
            LogManager.AddLoggerAdapter(new Log.Implement.Log4Net.Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(PatternApp).Name);
            _log.Info("锯切图服务开始启动！");
            try
            {
                var type = Type.GetType(PatternAppSetting.Current.PatternBuilder);
                if (type == null)
                {
                    _log.Info("无法找到命令构建器");
                    return false;
                }

                _log.Info("找到命令构建器：" + PatternAppSetting.Current.PatternBuilder);
                CommandBuilder builder = Activator.CreateInstance(type) as CommandBuilder;
                if (builder == null)
                {
                    _log.Info("创建命令构建器失败");
                    return false;
                }
                _log.Info("创建命令构建器：" + PatternAppSetting.Current.PatternBuilder + "成功");
                var cc = CommandContext.Instance;
                if (!cc.Build(builder))
                {
                    _log.Info($"创建命令上下文失败");
                    return false;
                }
                _log.Info("创建命令上下文：" + PatternAppSetting.Current.PatternBuilder + "成功");

                bool ret = base.Start();
                if (!ret) return false;
                _log.Info("锯切图服务启动成功！");
            }
            catch (Exception e)
            {
                _log.Error("锯切图服务启动失败:"+e.Message);
                return false;
            }
           
            return true;
        }

        //private void WcfServersStart(List<IWcfServer> iWcfServers)
        //{
        //    foreach (var wcfServer in iWcfServers)
        //    {
        //        try
        //        {
        //            wcfServer.Start();
        //            _log.Info("服务:"+wcfServer.GetType().Name+" 启动成功");
        //        }
        //        catch (Exception e)
        //        {
        //            _log.Info("服务:" + wcfServer.GetType().Name + " 启动失败\n"+e.Message);
        //        }
        //    }
        //}

        //private void WcfServersStop(List<IWcfServer> iWcfServers)
        //{
        //    foreach (var wcfServer in iWcfServers)
        //    {
        //        wcfServer.Stop();
        //    }
        //}

        public override bool Stop()
        {
            //WcfServersStop(_iWcfServers);
            bool ret = base.Stop();
            return ret;
        }

    }
}