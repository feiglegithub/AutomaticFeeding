//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：InParter.cs
//   创建时间：2018-11-23 15:30
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    /// <summary>
    ///     入板器
    /// </summary>
    public class InParter
    {
        /// <summary>
        ///     加入一块板件
        /// </summary>
        /// <returns></returns>
        public virtual PartInfo InPart(string partId,int position)
        {
            return new PartInfo();
        }
    }
}