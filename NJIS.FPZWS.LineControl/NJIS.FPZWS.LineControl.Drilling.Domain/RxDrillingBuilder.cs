//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：RxDrillingBuilder.cs
//   创建时间：2018-11-29 8:38
//   作    者：
//   说    明：
//   修改时间：2018-11-29 8:38
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Core;
using NJIS.FPZWS.LineControl.Drilling.Domain.Cache;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Drilling.Domain
{
    public class RxDrillingBuilder : DefaultDrillingBuilder
    {
        private static readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();
        private readonly ILogger _logger = LogManager.GetLogger(typeof(RxDrillingBuilder));

        public override List<ChainBuffer> CreateChainBuffers()
        {
            var chainBuffers = base.CreateChainBuffers();
            try
            {
                var cbs = _service.FindChainBuffers();
                foreach (var cb in cbs)
                {
                    var chainBUffer = new ChainBuffer
                    {
                        Code = cb.Code,
                        Remark = cb.Remark,
                        Size = cb.Size,
                        Status = cb.Size
                    };
                    chainBuffers.Add(chainBUffer);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }

            return chainBuffers;
        }

        public override InParter CreateInParter()
        {
            return new RxInParter();
        }

        public override ISploter CreateSploter()
        {
            return base.CreateSploter();
        }
    }

    public class RxInParter : InParter
    {
        private static readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();
        private readonly ILogger _logger = LogManager.GetLogger(typeof(RxInParter));

        public override PartInfo InPart(string partId, int position)
        {
            var drilling = PartInfoCache.GetDrilling(partId);
            if (drilling == null)
            {
                _logger.Info("找不到板件");
                return null;
            }

            var piq = _service.InsertPartInfoQueues(drilling, position);
            var partInfo = PartInfoCache.GetPartInfo(drilling);

            if (piq != null)
            {
                partInfo.Place = piq.Place;
                partInfo.NextPlace = piq.NextPlace;
                partInfo.PcsMessage = piq.PcsMessage;
                if (piq.IsNg != null) partInfo.IsNg = piq.IsNg.Value;
            }

            return partInfo;
        }
    }
}