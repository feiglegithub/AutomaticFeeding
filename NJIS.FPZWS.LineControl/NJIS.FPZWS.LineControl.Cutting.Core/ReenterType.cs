using System.ComponentModel;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public enum ReenterType
    {

        /// <summary>
        /// NG
        /// </summary>
        [Description("NG")]
        NG = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal =1,
        /// <summary>
        /// 抽检
        /// </summary>
        [Description("抽检")]
        Spot =2,
        /// <summary>
        /// 余料
        /// </summary>
        [Description("余料")]
        Offcut=3,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        other =4
    }
}
