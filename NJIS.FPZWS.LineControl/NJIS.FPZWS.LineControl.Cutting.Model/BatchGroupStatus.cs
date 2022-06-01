using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 批次组的状态
    /// </summary>
    public enum BatchGroupStatus
    {
        [Description("异常")]
        Error=-1,

        //0:未下发备料(不存在)，1：已下发备料，5：备料中，9：备料完成，10：请求上料，11：上料中，19：上料完成，21：开料中，29:开料完成
        /// <summary>
        /// 未备料
        /// </summary>
        [Description("未备料")]
        UnStock = 1,
        /// <summary>
        /// 备料中
        /// </summary>
        [Description("备料中")]
        Stocking = 5,
        /// <summary>
        /// 备料完成
        /// </summary>
        [Description("备料完成")]
        Stocked = 9,

        /// <summary>
        /// 等待物料（请求上料）
        /// </summary>
        [Description("请求上料")]
        WaitMaterial = 10,

        /// <summary>
        /// 上料中
        /// </summary>
        [Description("上料中")]
        LoadingMaterial = 11,

        /// <summary>
        /// 上料结束
        /// </summary>
        [Description("上料结束")]
        LoadedMaterial = 19,

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
