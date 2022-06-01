using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJIS.Model.Attributes;

namespace NJIS.FPZWS.Platform.Model
{
    [Table("Edgebanding")]
    public class Edgebanding
    {
        [Key]
        [Identity]
        public long Id { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string BatchName { get; set; }
        public string OrderNumber { get; set; }
        public string TaskId { get; set; }
        public string TaskDistributeId { get; set; }
        [CreatedAt]
        public DateTime? CreatedTime { get; set; }
        [UpdatedAt]
        public DateTime? UpdatedTime { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Thickness { get; set; }
        public float L1_OFFCUT { get; set; }
        public int L1_FORMAT { get; set; }
        public string L1_EDGE { get; set; }
        public string L1_CORNER { get; set; }
        public string L1_GROOVE { get; set; }
        public string L1_EDGECODE { get; set; }
        public int L2_FORMAT { get; set; }
        public string L2_EDGE { get; set; }
        public string L2_CORNER { get; set; }
        public string L2_EDGECODE { get; set; }
        public float C1_OFFCUT { get; set; }
        public int C1_FORMAT { get; set; }
        public string C1_EDGE { get; set; }
        public string C1_CORNER { get; set; }
        public string C1_EDGECODE { get; set; }
        public string C1_GROOVE { get; set; }
        public int C2_FORMAT { get; set; }
        public string C2_EDGE { get; set; }
        public string C2_CORNER { get; set; }
        public string C2_EDGECODE { get; set; }
    }
}
