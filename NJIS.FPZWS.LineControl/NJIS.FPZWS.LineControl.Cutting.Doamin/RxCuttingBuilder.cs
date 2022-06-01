using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.Domain
{
    public class RxCuttingBuilder:DefaultCuttingBuilder
    {
        private static readonly ILineControlCuttingContract _service = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();
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
    }
}
