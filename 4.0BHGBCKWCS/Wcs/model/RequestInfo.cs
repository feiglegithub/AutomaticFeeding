using System;

namespace WCS.model
{
    public class RequestInfo
    {
        public long ReqId { get; set; }
        public long TaskId { get; set; }
        public string ProductCode { get; set; }
        public int PilerNo { get; set; }
        public int Amount { get; set; }
        public int ReqType { get; set; }
        public string FromPosition { get; set; }
        public string ToPosition { get; set; }
        public int ResponseStatus { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public string ErrorMsg { get; set; }
    }
}
