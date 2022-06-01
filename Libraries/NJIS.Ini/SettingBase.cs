// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：SettingBase.cs
//  创建时间：2017-08-31 16:23
//  作    者：
//  说    明：
//  修改时间：2017-10-08 10:31
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#endregion

namespace NJIS.Ini
{
    [IniFile]
    public abstract class SettingBase<T> where T : class
    {
        protected SettingBase()
        {
            Load();
        }

        internal void Load()
        {
            var ifa = GetType().GetCustomAttribute<IniFileAttribute>();
            if (ifa == null)
            {
                ifa = new IniFileAttribute(GetType().Name, System.Text.Encoding.Default);
            }
            var file = ifa.Name + ".ini";
            if (string.IsNullOrEmpty(ifa.Name))
            {
                file = GetType().Name + ".ini";
            }
            var properties = GetType().GetProperties((BindingFlags.Instance
                                                      | BindingFlags.Public | BindingFlags.SetProperty));

            ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            bool isExistsFile = File.Exists(ConfigFilePath);
            IniFile = new IniFile(ConfigFilePath, new IniLoadSettings { Encoding = ifa.Encoding, AutoCreate = true });
            foreach (var propertyInfo in properties)
            {
                var pa = propertyInfo.GetCustomAttribute<PropertyAttribute>();
                if (pa == null)
                {
                    pa = new PropertyAttribute("general", propertyInfo.Name);
                }

                var section = pa.Section;
                var property = pa.Name;
                if (string.IsNullOrEmpty(property))
                {
                    property = propertyInfo.Name;
                }
                if (!IniFile.Contains(section))
                {
                    IniFile.Add(new IniFile.Section(section));
                }
                if (!IniFile[section].Contains(new IniFile.Property(property, "")))
                {
                    var val1 = propertyInfo.GetValue(this);
                    IniFile[section].Add(new IniFile.Property(property, val1 != null ? val1.ToString() : ""));
                }
                object val = IniFile[section][property];
                if (string.IsNullOrEmpty(IniFile[section][property]))
                {
                    val = pa.DefaultValue;
                    if (string.IsNullOrEmpty(pa.DefaultValue))
                    {
                        val = propertyInfo.PropertyType.IsValueType
                            ? Activator.CreateInstance(propertyInfo.PropertyType)
                            : "";
                    }
                }
                if (val != null)
                {
                    propertyInfo.SetValue(this, Convert.ChangeType(val, propertyInfo.PropertyType));
                }

            }
            if (!isExistsFile)
            {
                Save();
            }
        }

        protected string ConfigFilePath { get; set; }

        /// <summary>
        ///     Ini File
        /// </summary>
        protected IniFile IniFile { get; set; }

        protected List<string> this[string section]
        {
            get { return GetPropertyValues(section); }
        }

        public bool Save()
        {
            var properties = GetType().GetProperties((BindingFlags.Instance
                                                      | BindingFlags.Public | BindingFlags.SetProperty));
            foreach (var propertyInfo in properties)
            {
                var pa = propertyInfo.GetCustomAttribute<PropertyAttribute>();
                if (pa != null)
                {
                    var section = pa.Section;
                    var property = propertyInfo.Name;
                    var val = propertyInfo.GetValue(this);
                    IniFile[section][property] = val.ToString();
                }
            }
            IniFile.SaveTo(ConfigFilePath);
            return true;
        }

        private List<string> GetPropertyValues(string section)
        {
            var strs = new List<string>();
            var properties = GetType().GetProperties();
            foreach (var pro in properties)
            {
                var pa = pro.GetCustomAttribute<PropertyAttribute>();
                if (pa != null && pa.Section == section)
                {
                    strs.Add(pro.GetValue(this).ToString());
                }
            }
            return strs;
        }

        //public static readonly T Instance = new T();

        #region Singleton

        public static T Current
        {
            get { return Nested.Instance; }
        }

        private sealed class Nested
        {
            internal static readonly T Instance = (T)Activator.CreateInstance(typeof(T), true);

            static Nested()
            {
            }
        }

        #endregion
    }
}