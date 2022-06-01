// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IniFileAttribute.cs
//  创建时间：2017-07-28 14:33
//  作    者：
//  说    明：
//  修改时间：2017-10-08 10:31
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Text;

#endregion

namespace NJIS.Ini
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IniFileAttribute : Attribute
    {
        public IniFileAttribute() : this(string.Empty, Encoding.UTF8)
        {
        }

        public IniFileAttribute(string name) : this(name, Encoding.UTF8)
        {
        }

        public IniFileAttribute(string name, Encoding encoding)
        {
            Encoding = encoding;
            Name = name;
        }

        public string Name { get; set; }

        public Encoding Encoding { get; set; }
    }
}