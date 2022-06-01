using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus
{
    public class RxCuttingBuilder:DefaultCuttingBuilder
    {
        private static readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();
        private readonly ILogger _log = LogManager.GetLogger<RxCuttingBuilder>();

        public override List<ChainBufferInfo> CreateChainBuffers()
        {
            var chainBuffers = base.CreateChainBuffers();
            try
            {
                var cbs = _service.GetCuttingChainBuffers();
                foreach (var cb in cbs)
                {
                    chainBuffers.Add(new ChainBufferInfo() { CuttingChainBuffer = new CuttingChainBuffer(){Code = cb.Code,LineId = cb.LineId,Remark = cb.Remark,Size = cb.Size,Status = cb.Status} });
                }
            }
            catch (Exception e)
            {
                _log.Info("获取缓存架列表失败：" + e.Message);
            }

            return chainBuffers;
        }

        public override InParter CreateInParter()
        {
            return  new RxInParter();
        }

        public override ISpotter CreateSploter()
        {
            return new RxSpotter();
        }

        public override List<ICollector> CreateCollectors()
        {
            List<ICollector> collectors = new List<ICollector>
            {
                new RxCollector("0-240-07-8013"),
                new RxCollector("0-240-07-8014"),
                new RxCollector("0-240-07-8015"),
                new RxCollector("0-240-07-8016"),
                new RxCollector("0-240-07-8017"),
                new RxCollector("0-240-07-8018")
            };
            return collectors;
        }
    }
}
