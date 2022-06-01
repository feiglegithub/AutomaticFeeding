using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
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
