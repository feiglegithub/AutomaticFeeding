// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：WcfServiceEndpoint.cs
//  创建时间：2018-01-06 9:49
//  作    者：
//  说    明：
//  修改时间：2018-01-06 9:50
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class EndPoint
    {
        public string Binding { get; set; }
        public string BindingConfiguration { get; set; }

        public string Name { get; set; }

        public string Contract { get; set; }

        public string Address { get; set; }
    }
}