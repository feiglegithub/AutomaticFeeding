//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：PartInfoQueueArgs.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    [Serializable]
    public class PartInfoQueueArgs : MqttMessageArgsBase
    {
        public PartInfoQueueArgs()
        {
            CreatedTime = DateTime.Now;
        }

        public string PartId { get; set; }
        

        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Plc { get; set; }
        public int Pcs { get; set; }

        public string PcsMessage { get; set; }


        public string L1_FORMAT { get; set; }
        public string L1_EDGE { get; set; }
        public string L1_CORNER { get; set; }
        public string L1_GROOVE { get; set; }
        public string L1_EDGECODE { get; set; }
        public string L2_FORMAT { get; set; }
        public string L2_EDGE { get; set; }
        public string L2_CORNER { get; set; }
        public string L2_EDGECODE { get; set; }
        public string C1_FORMAT { get; set; }
        public string C1_EDGE { get; set; }
        public string C1_CORNER { get; set; }
        public string C1_EDGECODE { get; set; }
        public string C1_GROOVE { get; set; }
        public string C2_FORMAT { get; set; }
        public string C2_EDGE { get; set; }
        public string C2_CORNER { get; set; }
        public string C2_EDGECODE { get; set; }
    }
}