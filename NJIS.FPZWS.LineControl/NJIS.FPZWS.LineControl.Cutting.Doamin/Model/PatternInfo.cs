using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Model
{
    /// <summary>
    /// 锯切图信息
    /// </summary>
    public class PatternInfo
    {
        public string BatchName { get; set; }
        /// <summary>
        /// 锯切图
        /// </summary>
        [Description("锯切图")]
        public int Pattern { get; set; }

        /// <summary>
        /// 板材数
        /// </summary>
        [Description("板材数")]
        public int TotalBookCount { get; set; }

        /// <summary>
        /// 板材长
        /// </summary>
        [Description("板材长")]
        public double BookLength { get; set; }

        /// <summary>
        /// 板材厚
        /// </summary>
        [Description("板材厚")]
        public double BookThick { get; set; }

        /// <summary>
        /// 板材数
        /// </summary>
        [Description("板材宽")]
        public double BookWidth { get; set; }

        /// <summary>
        /// 板材总面积
        /// </summary>
        [Description("板材总面积")]
        public double BookTotalArea => BookLength * BookWidth * TotalBookCount / 1000000;
        /// <summary>
        /// 物料编码
        /// </summary>
        [Description("物料编码")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 总工件数
        /// </summary>
        [Description("总工件数")]
        public int TotalPartCount => NormalPartCount + OffPartCount;

        /// <summary>
        /// 板件数
        /// </summary>
        [Description("板件数")]
        public int NormalPartCount { get; set; }

        /// <summary>
        /// 余板数
        /// </summary>
        [Description("余板数")]
        public int OffPartCount { get; set; }



        /// <summary>
        /// 工件总面积
        /// </summary>
        [Description("工件总面积")]
        public double TotalArea => NormalArea + OffArea;

        private double _normalArea = 0;
        /// <summary>
        /// 板件总面积
        /// </summary>
        [Description("板件总面积")]
        public double NormalArea
        {
            get { return _normalArea; }
            set
            {
                _normalArea = value / 1000000;
            }
        }

        private double _offArea = 0;
        /// <summary>
        /// 余板总面积
        /// </summary>
        [Description("余板总面积")]
        public double OffArea
        {
            get { return _offArea; }
            set
            {
                _offArea = value / 1000000;
            }
        }

        /// <summary>
        /// 废料面积占比
        /// </summary>
        [Description("废料面积占比")]
        public double WastePercentage => 1 - TotalArea / BookTotalArea;

        /// <summary>
        /// 板件面积占比
        /// </summary>
        [Description("板件面积占比")]
        public double NormalPercentage => NormalArea / BookTotalArea;

        /// <summary>
        /// 余板面积占比
        /// </summary>
        [Description("余板面积占比")]
        public double OffPercentage => OffArea / BookTotalArea;

        /// <summary>
        /// 总耗时
        /// </summary>
        [Description("总耗时")]
        public int TotalTime { get; set; }
    }


    
}
