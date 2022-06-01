using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 锯切图明细
    /// </summary>
    public class PatternDetail
    {
        public long LineId { get; set; }
        public string BatchName { get; set; }
        public int PatternId { get; set; }
        public int OldPatternId { get; set; }
        public string PartId { get; set; }
        public bool IsOffPart { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Thickness { get; set; }
        public string Color { get; set; }
        public bool IsFinished { get; set; }
        public int PartCount { get; set; }
        public int FinishedPartCount { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
