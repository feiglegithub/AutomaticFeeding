//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：ISploter.cs
//   创建时间：2018-11-23 15:30
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public interface ISploter
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
        bool IsSplot(string partId);
    }
}