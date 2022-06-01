using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum PartFeedBackStatus
    {
        /// <summary>
        /// 未完成更新锯切图明细
        /// </summary>
        [Description("未完成更新锯切图明细")]
        UnFinished =0,
        /// <summary>
        /// 完成更新锯切图明细
        /// </summary>
        [Description("完成更新锯切图明细")]
        Finished=10,
    }
}
