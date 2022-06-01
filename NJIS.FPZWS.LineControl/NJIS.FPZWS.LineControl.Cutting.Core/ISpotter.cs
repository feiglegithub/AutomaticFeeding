using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public interface ISpotter
    {
        /// <summary>
        ///     抽检器名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     验证板件是否需要抽检
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        bool IsSpot(string partId);
    }
}
