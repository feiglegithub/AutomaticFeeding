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

using NJIS.FPZWS.Common;
using NJIS.FPZWS.Log;
using System;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public class CuttingApp : AppBase<CuttingApp>, IService
    {
        private ILogger _log = LogManager.GetLogger(typeof(CuttingApp).Name);

        public override bool Start()
        {
            _log = LogManager.GetLogger(typeof(CuttingApp).Name);
           
            var type = Type.GetType(CuttingSetting.Current.CuttingBuilder);
            if (type == null)
            {
                _log.Info("无法找到构建器");
                return false;
            }

            _log.Info("找到构建器："+CuttingSetting.Current.CuttingBuilder);
           CuttingBuilder builder = Activator.CreateInstance(type) as CuttingBuilder;
            if (builder == null)
            {
                _log.Info("创建构建器失败");
                return false;
            }
            _log.Info("创建构建器：" + CuttingSetting.Current.CuttingBuilder+"成功");
            var cc = CuttingContext.Instance;
            if (!cc.Build(builder))
            {
                _log.Info($"创建开料上下文失败");
                return false;
            }
            _log.Info("创建开料上下文：" + CuttingSetting.Current.CuttingBuilder + "成功");

            bool ret = base.Start();
            if (!ret) return false;
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