using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace ArithmeticsTest
{
    public class SolutionDetail
    {
        public int SolutionId { get; set; }
        public string DeviceName { get; set; }
        public int TotalTime { get; set; }
        public double TotalTimeRatio { get; set; }
        public float TotalTimeExpect { get; set; }
        public float Ratio { get; set; }
        public int OffPartNum { get; set; }
        public int OffPartNumRatio { get; set; }
        public float OffPartNumExpect { get; set; }
        public float OffPartNumRate { get; set; }
        public int PartNum { get; set; }
        public int PartNumRatio { get; set; }
        public float PartNumExpect { get; set; }
        public float PartNumRate { get; set; }
        public double PartSpeed { get; set; }
        public double PartSpeedRatio { get; set; }
        public float PartSpeedExpect { get; set; }
        public float PartSpeedRate { get; set; }
        public double Overlapping { get; set; }
        public double OverlappingRatio { get; set; }
        public float OverlappingExpect { get; set; }
        public float OverlappingRate { get; set; }
        public double ExpectTotalTime { get; set; }
        public List<AllTask> AllTasks { get; set; }
    }
}