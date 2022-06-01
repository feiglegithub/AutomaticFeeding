// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：Service.cs
//  创建时间：2018-01-08 16:06
//  作    者：
//  说    明：
//  修改时间：2018-01-08 16:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class ServerAttribute : Attribute
    {
        public ServerAttribute(string address)
        {
            AelativeAddress = address;
        }
        public string AelativeAddress { get; set; }
    }
}