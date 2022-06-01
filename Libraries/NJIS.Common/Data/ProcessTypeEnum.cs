using System.ComponentModel;

namespace NJIS.Common.Data
{
    public enum ProcessTypeEnum
    {
        [Description("开料")]
        Cutting,
        [Description("前缓存架")]
        CacheBefore,
        [Description("后缓存架")]
        CacheAfter,
        [Description("封边")]
        Edge,
        [Description("钻孔")]
        Drill,
        [Description("分拣")]
        Check,
        [Description("包装")]
        Packing,
    }
}
