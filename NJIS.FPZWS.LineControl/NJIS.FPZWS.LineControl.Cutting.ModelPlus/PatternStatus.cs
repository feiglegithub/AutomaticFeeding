using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum PatternStatus
    {
        //状态 (0：未分配，10：分配但未下载，11：下载中，19：已下载，20:待转换，21：转换中，29：转换完成，30：未开料，31：开料中，39：开料完成)

        /// <summary>
        /// 未分配
        /// </summary>
        [Description("未分配锯切图")]
        Undistributed = 0,

        /// <summary>
        /// 分配但未下载
        /// </summary>
        [Description("分配但未下载")]
        UndistributedButUnLoad =10,

        /// <summary>
        /// 下载中
        /// </summary>
        [Description("下载中")]
        Loading = 11,

        /// <summary>
        /// 已下载
        /// </summary>
        [Description("已下载")]
        Loaded = 19,

        /// <summary>
        /// 待转换
        /// </summary>
        [Description("待转换")]
        UnConvertSaw = 20,

        /// <summary>
        /// 转换中
        /// </summary>
        [Description("转换中")]
        ConvertingSaw = 21,

        /// <summary>
        /// 转换完成
        /// </summary>
        [Description("转换完成")]
        Converted = 29,

        /// <summary>
        /// 未开料
        /// </summary>
        [Description("未开料")]
        UnCut = 30,

        /// <summary>
        /// 开料中
        /// </summary>
        [Description("开料中")]
        Cutting = 31,

        /// <summary>
        /// 开料完成
        /// </summary>
        [Description("开料完成")]
        Cut = 39,

    }
}
