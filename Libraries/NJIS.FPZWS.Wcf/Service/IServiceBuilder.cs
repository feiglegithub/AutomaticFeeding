// ************************************************************************************
//  解决方案：NJIS.FPZWS.Tools.Drilling
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：IConfigBuilder.cs
//  创建时间：2018-01-06 9:50
//  作    者：
//  说    明：
//  修改时间：2018-01-08 14:29
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.ServiceModel;
using NJIS.FPZWS.Wcf.Service;

#endregion

namespace NJIS.FPZWS.Wcf.Service
{
    public interface IServiceBuilder
    {
        ServiceHost Build<T>() where T : IWcfServiceContract, new();

    }
}