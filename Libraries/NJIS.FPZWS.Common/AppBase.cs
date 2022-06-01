// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：AppBase.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.Common.Initialize;

#endregion

namespace NJIS.FPZWS.Common
{
    public class AppBase<T> where T : class, new()
    {
        public AppBase()
        {
            StarterInstances = new List<IModularStarter>();
        }

        public bool IsStart { get; set; }

        protected virtual IConfig Config { get; set; }

        protected List<IModularStarter> StarterInstances { get; set; }

        public virtual void Initializer()
        {
            // 加载所有初始化器
            var finder = new DirectoryReflectionFinder();
            var initializerTypes = finder.GetTypeFromAssignable<IInitializer>();
            var initializers = new List<IInitializer>();
            foreach (var initial in initializerTypes)
            {
                var instance = (IInitializer)Activator.CreateInstance(initial);
                if (instance != null)
                {
                    initializers.Add(instance);
                }
            }

            foreach (var initializer in initializers.OrderByDescending(m => m.Level))
            {
                initializer.Initializer(Config);
            }
        }

        public virtual bool Start()
        {
            if (IsStart)
            {
                return true;
            }

            IsStart = true;
            Initializer();

            // 加载所有模块启动器
            var finder = new DirectoryReflectionFinder();
            var starters = finder.GetTypeFromAssignable<IModularStarter>();

            foreach (var starter in starters)
            {
                var instance = (IModularStarter)Activator.CreateInstance(starter);
                if (instance != null)
                {
                    StarterInstances.Add(instance);
                }
            }

            var lst = new List<string>();
            foreach (var starter in StarterInstances.OrderByDescending(m => m.Level))
            {
                if (!lst.Contains(starter.GetType().FullName))
                {
                    StartModular(starter);
                    lst.Add(starter.GetType().FullName);
                }
            }
            
            return true;
        }

        public virtual void StartModular(IModularStarter instance)
        {
            if (instance != null)
            {
                instance.Start();
            }
        }

        public virtual bool Stop()
        {
            if (!IsStart) return false;
            if (StarterInstances != null && StarterInstances.Count > 0)
            {
                foreach (var starterInstance in StarterInstances)
                {
                    starterInstance.Stop();
                }
            }

            IsStart = false;
            return true;
        }


        #region Singleton

        public static T Current => Nested.Instance;

        private sealed class Nested
        {
            internal static readonly T Instance = new T();

            static Nested()
            {
            }
        }

        #endregion
    }
}