//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：PartInfoArgs.cs
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
    public class PartInfoArgs : MqttMessageArgsBase
    {
        public PartInfoArgs()
        {
            CreatedTime = DateTime.Now;
        }

        public string PartId { get; set; }
        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }

        /// <summary>
        ///     打孔类型
        /// </summary>
        public string DrillingRoute { get; set; }
    }
}