using System;

namespace WCS.model
{
    public class TaskInfo
    {
        public long TaskId { get; set; }
        public long ReqId { get; set; }
        public int PilerNo { get; set; }
        public string ProductCode { get; set; }
        public int Amount { get; set; }
        public int TaskType { get; set; }
        /// <summary>
        /// 是否有上保护板
        /// </summary>
        public bool HasUpProtect { get; set; }
        public int Priority { get; set; }
        public string FromPosition { get; set; }
        public string ToPosition { get; set; }
        public int TaskStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DdjTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string ErrorMsg { get; set; }

        public int ddj { get; set; }
        public int target { get; set; }

        public int ToLayer { get; set; }
        public int ToRow { get; set; }
        public int ToColumn { get; set; }

        public int Port { get; set; }

        public int FromRow { get; set; }
        public int FromColumn { get; set; }
        public int FromLayer { get; set; }
        public int InOut_Port { get; set; }
    }
}
