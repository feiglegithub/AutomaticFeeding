//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：PlcConnectState.cs
//   创建时间：2018-11-20 15:10
//   作    者：
//   说    明：
//   修改时间：2018-11-20 15:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    /// <summary>
    ///     PLC 连接 状态
    /// </summary>
    public enum PlcConnectState
    {
        /// <summary>
        ///     已连接
        /// </summary>
        Connected = 1,

        /// <summary>
        ///     断开，未连接
        /// </summary>
        Disconnect = 2
    }
}