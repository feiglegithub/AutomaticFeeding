// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：WcfService.cs
//  创建时间：2018-01-06 9:49
//  作    者：
//  说    明：
//  修改时间：2018-01-06 9:50
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.Wcf.Config
{
    [Serializable]
    public class Service
    {
        public Service()
        {
            Endpoints = new List<EndPoint>();
            BaseAddresses = new List<string>();
        }

        public string Name { get; set; }

        public string BehaviorConfiguration { get; set; }

        public List<EndPoint> Endpoints { get; set; }

        public List<string> BaseAddresses { get; set; }
    }
}