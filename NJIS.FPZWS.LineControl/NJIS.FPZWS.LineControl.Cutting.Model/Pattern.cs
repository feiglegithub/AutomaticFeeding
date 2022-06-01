using System;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 锯切图列表
    /// </summary>
    public class Pattern
    {
        public long LineId { get; set; }
        public DateTime PlanDate { get; set; }
        public string BatchName { get; set; }
        public string MdbName { get; set; }
        public int PatternId { get; set; }
        public string Color { get; set; }
        public int BookCount { get; set; }
        public int TotallyTime { get; set; }
        public int PartCount { get; set; }
        public int OffPartCount { get; set; }
        public bool IsNodePattern { get; set; }
        public int OldPatternId { get; set; }
        public bool IsEnable { get; set; }
        public int Status { get; set; }
        public string DeviceName { get; set; }
        public string FileFullPath { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string PlanStackName { get; set; }
        public string ActuallyStackName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public string PlanDeviceName { get; set; }
        public string ActualDeviceName { get; set; }

    }
}
