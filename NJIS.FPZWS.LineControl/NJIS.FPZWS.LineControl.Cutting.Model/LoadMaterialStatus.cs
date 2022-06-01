using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 上料状态
    /// </summary>
    public enum LoadMaterialStatus
    {
        /// <summary>
        /// 等待上料
        /// </summary>
        UnLoadMaterial=10,
        /// <summary>
        /// 上料中
        /// 
        /// </summary>
        LoadingMaterial=20,
        /// <summary>
        /// 上料结束
        /// </summary>
        LoadedMaterial=30
    }
}
