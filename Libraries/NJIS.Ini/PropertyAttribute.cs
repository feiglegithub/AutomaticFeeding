// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：PropertyAttribute.cs
//  创建时间：2017-07-28 14:33
//  作    者：
//  说    明：
//  修改时间：2017-10-08 10:31
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.Ini
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute() : this("general")
        {
        }

        public PropertyAttribute(string section) : this(section, string.Empty)
        {
        }

        public PropertyAttribute(string section, string name)
        {
            Name = name;
            Section = section;
        }

        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
    }
}