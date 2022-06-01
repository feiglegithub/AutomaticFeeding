//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：DefaultDrillingBuilder.cs
//   创建时间：2018-11-23 15:38
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:38
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public abstract class DefaultDrillingBuilder : DrillingBuilder
    {
        public override InParter CreateInParter()
        {
            return new InParter();
        }

        public override List<ChainBuffer> CreateChainBuffers()
        {
            return new List<ChainBuffer>();
        }

        public override ISploter CreateSploter()
        {
            return new SploterBase();
        }
    }
}