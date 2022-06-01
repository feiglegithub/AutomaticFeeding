// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc
//  项目名称：NJIS.FPZWS.MqttClient
//  文 件 名：SettingBase.cs
//  创建时间：2017-10-19 8:17
//  作    者：
//  说    明：
//  修改时间：2017-10-19 17:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#endregion

namespace NJIS.FPZWS.MqttClient.Setting
{
    public class SettingBase<T> : Singleton<T> where T : class
    {
        protected SettingBase(string fileName) : this(fileName, Encoding.UTF8)
        {
        }

        protected SettingBase(string fileName, Encoding encoding)
        {
            Init(fileName, Encoding.Default);
        }

        protected SettingBase()
        {
            var fileName = string.Empty;
            var ifa = GetType().GetCustomAttribute<IniFileAttribute>();
            if (ifa != null)
            {
                if (string.IsNullOrEmpty(ifa.Name))
                {
                    fileName = GetType().Name + ".ini";
                }
            }

            Init(fileName, Encoding.UTF8);
        }

        /// <summary>
        ///     Ini File
        /// </summary>
        protected IniFile IniFile { get; set; }

        public List<string> this[string section]
        {
            get { return GetPropertyValues(section); }
        }

        private void Init(string fileName, Encoding encoding)
        {
            var properties = GetType().GetProperties();

            IniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName),
                new IniLoadSettings {Encoding = encoding});
            foreach (var propertyInfo in properties)
            {
                var pa = propertyInfo.GetCustomAttribute<PropertyAttribute>();
                if (pa != null)
                {
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
                        IniFile[section].Add(new IniFile.Property(property, ""));
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

                var sa = propertyInfo.GetCustomAttribute<SectionAttribute>();
                if (sa != null)
                {
                    if (string.IsNullOrEmpty(sa.Name))
                    {
                        sa.Name = propertyInfo.Name;
                    }
                    if (IniFile.Contains(sa.Name))
                    {
                        propertyInfo.SetValue(this, IniFile[sa.Name]);
                    }
                }
            }
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
    }
}