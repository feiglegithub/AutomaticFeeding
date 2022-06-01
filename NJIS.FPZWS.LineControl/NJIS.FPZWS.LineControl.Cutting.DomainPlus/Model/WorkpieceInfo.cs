using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Model
{
    /// <summary>
    /// 工件信息
    /// </summary>
    public class WorkpieceInfo
    {
        /// <summary>
        /// 板件编号
        /// </summary>
        [Description("板件编号")]
        public string PartId { get; set; }

        /// <summary>
        /// 是否为余板
        /// </summary>
        [Description("是否为余板")]
        public bool IsOffPart { get; set; } = false;

        /// <summary>
        /// 工件长度
        /// </summary>
        [Description("工件长度")]
        public double Length { get; set; }

        /// <summary>
        /// 工件宽度
        /// </summary>
        [Description("工件宽度")]
        public double Width { get; set; }

        /// <summary>
        /// 工件厚度
        /// </summary>
        [Description("工件厚度")]
        public double Thickness { get; set; }

        /// <summary>
        /// 工件面积
        /// </summary>
        [Description("工件面积")]
        public double Area => Length * Width * PartCount;

        public int PartCount { get; set; }
    }
}
