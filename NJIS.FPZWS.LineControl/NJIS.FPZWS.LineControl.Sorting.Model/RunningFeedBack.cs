//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：RunningFeedBack.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class RunningFeedBack
    {
        public string PartId { get; set; }
        public string RotbotGroupCode { get; set; }
        public string FrameCode { get; set; }
        public string Layer { get; set; }
        public int ProductionLine { get; set; }
        public string FeedBackStatus { get; set; }
        public string CreatedTime { get; set; }
        public string Status { get; set; }
        public string BatchName { get; set; }
        public string PackslipNumber { get; set; }
        public string OrderNumber { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Thickness { get; set; }
        public int Weight { get; set; }
        public DateTime ProductionDate { get; set; }

        public PartStatusEnum StatusDesc
        {
            get
            {
                var defStatus = PartStatusEnum.未知;
                Enum.TryParse(Status, out defStatus);
                return defStatus;
            }
        }
    }

    public enum PartStatusEnum
    {
        上线扫码 = 1,
        分配库位 = 2,
        开始入库 = 3,
        入库完成 = 4,
        开始出库 = 11,
        出库完成 = 12,
        开始汇流 = 13,
        汇流完成 = 14,
        未知
    }
}