using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    public enum MdbParseStatus
    {
        /// <summary>
        /// 未解析
        /// </summary>
        UnParse=0,
        /// <summary>
        /// 解析中
        /// </summary>
        Parsing=10,
        /// <summary>
        /// 解析完成
        /// </summary>
        Parsed=99
    }
}
