using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public enum CheckObject
    {
        /// <summary>
        /// 批次作为抽检对象
        /// </summary>
        [Description("批次")]
        Batch = 1,
        /// <summary>
        /// 设备作为抽检对象
        /// </summary>
        [Description("设备")]
        Device = 2,
        /// <summary>
        /// 任务作为抽检对象
        /// </summary>
        [Description("任务")]
        Task = 3,
        
    }

    
}
