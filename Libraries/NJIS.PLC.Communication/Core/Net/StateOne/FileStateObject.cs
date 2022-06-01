//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：FileStateObject.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.IO;

namespace NJIS.PLC.Communication.Core.Net.StateOne
{
    /// <summary>
    ///     文件传送的异步对象
    /// </summary>
    internal class FileStateObject : StateOneBase
    {
        /// <summary>
        ///     操作的流
        /// </summary>
        public Stream Stream { get; set; }
    }
}