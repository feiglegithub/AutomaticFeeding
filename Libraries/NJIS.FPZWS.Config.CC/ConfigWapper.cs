//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：ConfigWapper.cs
//   创建时间：2018-10-15 15:33
//   作    者：
//   说    明：
//   修改时间：2018-10-15 15:33
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.ComponentModel;
using System.IO;
using NJIS.ConfigurationCenter.Client;

namespace NJIS.FPZWS.Config.CC
{
    public abstract class ConfigBase
    {
        public abstract string GetPath();
        public virtual string Path { get; protected set; }
    }

    public abstract class ConfigWapper<T> : ConfigBase
        where T : ConfigBase, new()
    {
        public override string GetPath()
        {
            var depStr = "";
            if (!string.IsNullOrEmpty(FpzCcSetting.Current.Dep))
            {
                depStr = $"{FpzCcSetting.Current.Dep}";
            }
            else
            {
                depStr = "湖北未来工厂项目";
            }
            return $"{depStr}/{FpzCcSetting.Current.Env}/{Path}";
        }
        public ConfigWapper()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                try
                {

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            ConfigEntry.Changed += ConfigEntry_Changed;
        }



        private void ConfigEntry_Changed(string configAddress, string version)
        {
            if (configAddress == this.GetPath())
            {
                LoadConfig(true);
            }
        }


        public static void LoadConfig(bool isUpdate = false)
        {
            try
            {
                if (isUpdate || _current == null)
                {
                    _current = new T();
                    if (!string.IsNullOrEmpty(_current.GetPath()))
                    {
                        var t = ConfigEntry.Get<T>(_current.GetPath(), 5000, true,
                            true);
                        var propertyInfos = t.GetType().GetProperties(); //获取T对象的所有公共属性
                        foreach (var propertyInfo in propertyInfos)
                        {
                            if (propertyInfo.CanWrite)
                            {
                                //判断值是否为空，如果空赋值为null见else
                                if (propertyInfo.PropertyType.IsGenericType &&
                                    propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                                {
                                    //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                    var nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                                    //将convertsionType转换为nullable对的基础基元类型
                                    propertyInfo.SetValue(_current, Convert.ChangeType(propertyInfo.GetValue(t), nullableConverter.UnderlyingType), null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(_current, Convert.ChangeType(propertyInfo.GetValue(t), propertyInfo.PropertyType), null);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {

            }
        }

        internal static T _current;

        public static T Current
        {
            get
            {
                LoadConfig();
                return _current;
            }
        }
    }
}