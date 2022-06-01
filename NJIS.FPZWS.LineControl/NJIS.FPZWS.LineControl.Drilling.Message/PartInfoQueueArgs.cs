//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Message
//   文 件 名：PartInfoQueueArgs.cs
//   创建时间：2018-11-29 11:37
//   作    者：
//   说    明：
//   修改时间：2018-11-29 11:37
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.LineControl.Drilling.Message
{
    [Serializable]
    public class PartInfoQueueArgs : MqttMessageArgsBase
    {
        public PartInfoQueueArgs()
        {
            CreatedTime = DateTime.Now;
        }

        public string PartId { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 下一个位置
        /// </summary>
        public string NextPlace { get; set; }
        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public string DrillingRouting { get; set; }
        public int Plc { get; set; }
        public int Pcs { get; set; }
        public int Status { get; set; }

        public string PcsMessage { get; set; }

        public int IsNg { get; set; }

    }
}