using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Manager.ViewModels
{
    public partial class CuttingTaskDetail
    {
        public long LineId { get; set; }
        public Nullable<System.Guid> TaskDistributeId { get; set; }
        public string BatchName { get; set; }
        public string DeviceName { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> JOB_INDEX { get; set; }
        public Nullable<int> PART_INDEX { get; set; }
        public Nullable<System.DateTime> PlanDate { get; set; }
        public string PartId { get; set; }
        public bool PartFinishedStatus { get; set; }
        public Nullable<bool> TaskEnable { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<double> Width { get; set; }
        public int OldPTN_INDEX { get; set; }
        public int NewPTN_INDEX { get; set; }
        public bool IsNg { get; set; }
        public bool IsOffPart { get; set; }
        public string Color { get; set; }
    }
}
