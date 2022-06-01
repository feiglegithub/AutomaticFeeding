using System;

namespace NJIS.FPZWS.LineControl.Manager.ViewModels
{
    internal class TmpBatchInfo
    {
        public string BatchName { get; set; }
        public string Color { get; set; }
        public int TotallyTime { get; set; }
        public int BookCount { get; set; }
        public int PartCount { get; set; }
        public int OffPartCount { get; set; }
        public int ColorCount { get; set; }
        public DateTime PlanDate { get; set; }
    }
}