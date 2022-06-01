// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：IMonitorControl.cs
//  创建时间：2017-12-29 14:20
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Collections.Generic;
using System.ServiceModel;
using NJIS.FPZWS.Wcf.Service;

#endregion
namespace NJIS.FPZWS.Wcf.Monitor
{
    [ServiceContract]
    public interface IMonitorControl: IWcfServiceContract
    {
        /// <summary>
        ///     获取wcf监控信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, LinkModel> GetMonitorInfo(out PcData pcdata, out double memCount);
    }
}