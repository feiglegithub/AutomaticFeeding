// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf.Service
//  文 件 名：MonitorControlServer.cs
//  创建时间：2017-12-28 16:05
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:32
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

#endregion

#region

using NJIS.FPZWS.Wcf.Monitor;

#endregion

namespace NJIS.FPZWS.Wcf.Service.Servers
{
    [Server("/sfy/wcf/monitor")]
    public class MonitorControlServer : WcfServer<MonitorControl>
    {
    }
}