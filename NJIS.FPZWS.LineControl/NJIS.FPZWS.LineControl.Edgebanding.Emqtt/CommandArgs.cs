//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：CommandArgs.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    [Serializable]
    public class CommandArgs : MqttMessageArgsBase
    {
        public string CommandCode { get; set; }
        public object Input { get; set; }
        public object Output { get; set; }

        public CommandType Type { get; set; }
    }

    public enum CommandType
    {
        /// <summary>
        ///     开始
        /// </summary>
        S = 10,

        /// <summary>
        ///     结束
        /// </summary>
        E = 11
    }
}