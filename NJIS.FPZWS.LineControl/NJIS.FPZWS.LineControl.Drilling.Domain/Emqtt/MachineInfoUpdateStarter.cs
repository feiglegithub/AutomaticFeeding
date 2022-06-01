using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    public class MachineInfoUpdateStarter : ModularStarterBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(MachineInfoUpdateStarter).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override StarterLevel Level => StarterLevel.High;

        private bool IsStop { get; set; } = false;
        public override void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (!IsStop)
                {
                    try
                    {
                        var dts = _service.FindAllMachine();
                        var datas = (from ps in dts
                                     select new MachineArgs
                                     {
                                         Code = ps.Code,
                                         SN = ps.SN,
                                         Status = ps.Status.GetHashCode(),
                                         Name = ps.Name,
                                         IsProcessSingle = ps.IsProcessSingle,
                                         IsProcessDouble = ps.IsProcessDouble
                                     }).ToList();
                        MqttManager.Current.Publish($"{EmqttSetting.Current.PcsMachineInitRep}", datas);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }

                    Thread.Sleep(DrillingSetting.Current.MachineInfoRefreshInterval);
                }
            });
        }

        public override void Stop()
        {
            IsStop = true;
            base.Stop();
        }
    }
}