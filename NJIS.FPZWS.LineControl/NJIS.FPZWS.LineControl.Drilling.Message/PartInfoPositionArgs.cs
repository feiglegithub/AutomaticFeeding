// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Sorting
//  项目名称：NJIS.FPZWS.LineControl.Sorting.Emqtt.Message
//  文 件 名：InQueueArgs.cs
//  创建时间：2017-10-26 13:59
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
    public class PartInfoPositionArgs : MqttMessageArgsBase
    {
        public PartInfoPositionArgs()
        {
            CreatedTime=DateTime.Now;
        }

        /// <summary>
        /// 板件
        /// </summary>
        public string PartId { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? Time { get; set; }

        public string Msg { get; set; }
    }
    
}