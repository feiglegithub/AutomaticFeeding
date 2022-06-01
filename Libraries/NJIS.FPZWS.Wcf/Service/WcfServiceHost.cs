// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Service
//  文 件 名：WcfServiceHost.cs
//  创建时间：2017-12-28 15:42
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    public class WcfServiceHost<T> : ServiceHost
    {
        public WcfServiceHost(Type serviceType) : base(serviceType)
        {
        }

        public WcfServiceHost(Type serviceType, params Uri[] baseAddresses) :
            base(serviceType, baseAddresses)
        {
        }

        protected override void ApplyConfiguration()
        {
            var configName = typeof(T).Assembly.FullName.Substring(0, typeof(T).Assembly.FullName.IndexOf(","));
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CfgFiles",
                configName + ".config");
            // Check user config invalidation  
            if (!CheckConfigExist(configFilePath))
            {
                // Use default config  
                base.ApplyConfiguration();
                return;
            }
            //base.ApplyConfiguration();  
            // Use user config  
            var execfgMap = new ExeConfigurationFileMap();
            // Set user config FilePath  
            execfgMap.ExeConfigFilename = configFilePath;
            // Config info  
            var cfg = ConfigurationManager.OpenMappedExeConfiguration(execfgMap, ConfigurationUserLevel.None);
            // Gets all service model config sections  
            var servicemodelSections = ServiceModelSectionGroup.GetSectionGroup(cfg);

            // Find serivce section matched with the name "this.Description.ServiceType.FullName"   
            if (!ApplySectionInfo(Description.ServiceType.FullName, servicemodelSections))
            {
                throw new Exception(
                    "ConfigApply Error : There is no endpoint existed in your config!! Please check your config file!");
            }
            ApplyMultiBehaviors(servicemodelSections);
        }

        /// <summary>
        ///     Check config file!
        /// </summary>
        /// <param name="configpath"></param>
        /// <returns></returns>
        private bool CheckConfigExist(string configpath)
        {
            if (string.IsNullOrEmpty(configpath)) return false;
            if (!File.Exists(configpath)) return false;
            return true;
        }

        /// <summary>
        ///     Apply section info
        /// </summary>
        /// <param name="serviceFullName"></param>
        /// <param name="servicemodelSections"></param>
        /// <returns></returns>
        private bool ApplySectionInfo(string serviceFullName, ServiceModelSectionGroup servicemodelSections)
        {
            // Check config sections (!including one section at least!)
            if (servicemodelSections == null) return false;
            // Service name can't be none!
            if (string.IsNullOrEmpty(serviceFullName)) return false;
            var isElementExist = false;
            foreach (ServiceElement element in servicemodelSections.Services.Services)
            {
                if (element.Name == serviceFullName)
                {
                    // Find successfully & apply section info of config file
                    LoadConfigurationSection(element);
                    // Find service element successfully
                    isElementExist = true;
                    break;
                }
            }
            return isElementExist;
        }


        /// <summary>
        ///     Add behaviors
        /// </summary>
        /// <param name="servicemodelSections"></param>
        /// <returns></returns>
        private bool ApplyMultiBehaviors(ServiceModelSectionGroup servicemodelSections)
        {
            if (servicemodelSections == null) return false;
            foreach (ServiceBehaviorElement element in servicemodelSections.Behaviors.ServiceBehaviors)
            {
                foreach (var behavior in element)
                {
                    var behaviorEx = behavior;
                    var extention = behaviorEx.GetType().InvokeMember("CreateBehavior",
                        BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        behaviorEx,
                        null);
                    if (extention == null) continue;
                    var isb = (IServiceBehavior) extention;
                    //if (base.Description.Behaviors.Contains(isb)) break;
                    var isbehaviorExisted = false;
                    foreach (var i in Description.Behaviors)
                    {
                        if (i.GetType().Name == isb.GetType().Name)
                        {
                            isbehaviorExisted = true;
                            break;
                        }
                    }
                    if (isbehaviorExisted) break;
                    Description.Behaviors.Add((IServiceBehavior) extention);
                }
            }
            return true;
        }
    }
}