// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Sorting
//  项目名称：NJIS.FPZWS.LineControl.Sorting.Emqtt.Message
//  文 件 名：CommandArgs.cs
//  创建时间：2017-10-26 16:04
//  作    者：
//  说    明：
//  修改时间：2017-10-27 16:48
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Message
{

    [Serializable]
    public class PositionArgs : MqttMessageArgsBase
    {
        /// <summary>
        /// 位置
        /// </summary>
        public int Place { get; set; }

        public int NextPlace { get; set; }

        /// <summary>
        /// 板件
        /// </summary>
        public string PartId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string PcsMessage { get; set; }

        public int IsNg { get; set; }
    }

}