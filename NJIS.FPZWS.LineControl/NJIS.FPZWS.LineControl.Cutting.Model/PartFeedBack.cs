using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public class PartFeedBack
    {
        public long LineId { get; set; }
        public string PartId { get; set; }
        public string DeviceName { get; set; }
        public int Status { get; set; }
        public string BatchName { get; set; }
        public long DevicePPCD_ID { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Thickness { get; set; }
        public string Color { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
