using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 板材的状态
    /// </summary>
    public enum BookStatus
    {
        //0：未分配锯切图，9：已分配锯切图，11：开料中，19：开料完成

        /// <summary>
        /// 未分配锯切图
        /// </summary>
        [Description("未分配锯切图")]
        UndistributedPattern = 0,

        /// <summary>
        /// 已分配锯切图
        /// </summary>
        [Description("已分配锯切图")]
        DistributedPattern = 9,

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
