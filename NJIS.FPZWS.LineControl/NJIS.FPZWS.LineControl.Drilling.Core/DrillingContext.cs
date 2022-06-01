//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：DrillingContext.cs
//   创建时间：2018-11-23 15:30
//   作    者：
//   说    明：
//   修改时间：2018-11-23 15:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public class DrillingContext
    {
        private static DrillingContext _instance;

        public static DrillingContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DrillingContext();
                }

                return _instance;
            }
            set => _instance = value;
        }

        internal bool Build(DrillingBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            InParter = builder.CreateInParter();
            ChainBuffers = builder.CreateChainBuffers();
            Sploter = builder.CreateSploter();
            
            return true;
        }


        public ISploter Sploter { get; protected set; }
        public InParter InParter { get; protected set; }

        public List<ChainBuffer> ChainBuffers { get; protected set; }
    }
}