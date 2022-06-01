using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Entitys
{
    public class InPartOutputEntity: EntityBase
    {
        /// <summary>
        ///     10:正常
        ///     20：找不到板件
        ///     100：线控故障
        /// </summary>
        public int Result { get; set; }

        public string PartId { get; set; }

        public string BatchName { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Thickness { get; set; }

        ///// <summary>
        /////     板件位置
        ///// </summary>
        //public int Place { get; set; }

        /// <summary>
        ///     
        ///     10：余料
        ///     20：抽检板件（NG）
        ///     30:正常
        ///     
        /// </summary>
        public int PartType { get; set; }
    }
}
