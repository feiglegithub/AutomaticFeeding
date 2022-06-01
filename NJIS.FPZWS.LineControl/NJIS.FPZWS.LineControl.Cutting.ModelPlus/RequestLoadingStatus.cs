using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum RequestLoadingStatus
    {
        /// <summary>
        /// 请求上料
        /// </summary>
        [Description("请求上料")]
        RequestLoad=10,
        /// <summary>
        /// 上料中
        /// </summary>
        [Description("上料中")]
        WmsLoading =20,
        /// <summary>
        /// 上料完成
        /// </summary>
        [Description("上料完成")]
        Loaded =30
    }
}
