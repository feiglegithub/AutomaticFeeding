//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：Program.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NJIS.ConfigurationCenter.Client;

namespace NJIS.FPZWS.Config.CC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"============当天配置中心版本===============");
            Console.WriteLine($"============Path:{FpzCcSetting.Current.EmqttClientType}===============");
            Console.WriteLine($"============Path:{FpzCcSetting.Current.Env}===============");
            //ConfigurationCenterStarter.SetLog(new ConsoleLog());
            //ConfigurationCenterStarter.Start();
            //var b = FpzCcSetting.Current;
            var cc = ConfigEntry.Get<TaskCenterSetting>("湖北未来工厂项目/prod/TaskCenterSetting", 5000, true, true);


            Console.WriteLine("输入任意键退出...");
            Console.Read();
            //ConfigurationCenterStarter.Stop();
        }

        /// <summary>
        ///     查找所有项
        /// </summary>
        /// <returns></returns>
        public static Type[] FindConfigs()
        {
            var types = new List<Type>();
            foreach (var type in typeof(Program).Assembly.GetTypes())
                if (!type.IsAbstract && HasImplementedRawGeneric(type, typeof(ConfigWapper<>)))
                    types.Add(type);
            return types.ToArray();
        }

        /// <summary>
        ///     判断指定的类型 <paramref name="type" /> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
        /// </summary>
        /// <param name="type">需要测试的类型。</param>
        /// <param name="generic">
        ///     泛型接口类型，传入 typeof(IXxx<>)
        /// </param>
        /// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
        public static bool HasImplementedRawGeneric(Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // 测试接口。
            var isTheRawGenericType = IsTheRawGenericType(type, generic);
            if (isTheRawGenericType) return true;

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type, generic);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

        }

        /// <summary>
        /// 测试某个类型是否是指定的原始接口
        /// </summary>
        /// <param name="test"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsTheRawGenericType(Type test, Type generic)
        {
            bool flag = false;
            var ifs = test.GetInterfaces();
            foreach (var item in ifs)
            {
                if (item.IsGenericType && item == generic)
                {
                    flag = true;
                }
            }

            return flag;
        }

    }

    public class ConsoleLog : ILog
    {
        public void Error(Exception e)
        {
            Console.WriteLine(e);
        }

        public void Error(string errorText)
        {
            Console.WriteLine(errorText);
        }

        public void Info(string InfoText)
        {
            Console.WriteLine(InfoText);
        }
    }
}