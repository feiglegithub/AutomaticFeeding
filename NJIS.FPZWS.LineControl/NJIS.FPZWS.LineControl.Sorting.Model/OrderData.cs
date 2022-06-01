//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：OrderData.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class PartData
    {
        public string PackCode { get; set; }
        public string PanelName { get; set; }
        public string ColorCode { get; set; }
        public int VehicleNum { get; set; }
    }

    public class BatchData
    {
        public string OrderNumber { get; set; }
        public string LotNumber { get; set; }
        public string ScheduleDeliveryDate { get; set; }
        public int Sequences { get; set; }
        public string Attribute7 { get; set; }
    }
}