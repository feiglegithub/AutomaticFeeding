//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：ChainBuffer.cs
//   创建时间：2018-11-23 15:30
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    /// <summary>
    ///     链式缓存
    /// </summary>
    public class ChainBuffer
    {
        public ChainBuffer()
        {
            Parts=new List<PartInfo>();
        }
        /// <summary>
        ///     编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 容量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 状态
        /// 10：禁用
        /// 20：启用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        public List<PartInfo> Parts { get; set; }
    }
}