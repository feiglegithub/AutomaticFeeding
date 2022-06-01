using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public enum CuttingSawFileRelationPlusStatus
    {
        [Description("未分配")]
        Unassigned = 1,
        [Description("已分配")]
        Assigned = 2,
        [Description("下载中")]
        Downloading = 3,
        [Description("已下载")]
        Downloaded = 4,
        [Description("下载失败")]
        Fail = 5,
        [Description("手动分配")]
        ManualAllocation = 6,
        [Description("推板完成")]
        PushCompleted = 7,
        [Description("进料完成")]
        FeedingComplete = 8
    }
}
