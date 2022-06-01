//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：DrillingBuilder.cs
//   创建时间：2018-11-23 15:35
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public abstract class DrillingBuilder
    {
        /// <summary>
        ///     创建入板器
        /// </summary>
        /// <returns></returns>
        public abstract InParter CreateInParter();

        /// <summary>
        ///     创建链式缓存
        /// </summary>
        /// <returns></returns>
        public abstract List<ChainBuffer> CreateChainBuffers();

        /// <summary>
        ///     创建抽检器
        /// </summary>
        /// <returns></returns>
        public abstract ISploter CreateSploter();
    }
}