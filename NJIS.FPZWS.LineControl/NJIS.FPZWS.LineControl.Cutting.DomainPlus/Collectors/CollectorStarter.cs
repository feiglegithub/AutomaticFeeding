using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.Log;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Collectors
{
    public class CollectorStarter : IModularStarter
    {
        private ILogger _log = LogManager.GetLogger(typeof(CollectorStarter).Name);
        public StarterLevel Level { get; }

        private ConcurrentQueue<ICollector> CollectorQueue { get; } = new ConcurrentQueue<ICollector>();

        private Thread Thread { get; }

        public CollectorStarter()
        {
            Thread = new Thread(TaskExecute) {IsBackground = true};
            CuttingContext.Instance.Collectors.ForEach(item => CollectorQueue.Enqueue(item));
        }

        public void Start()
        {
            Thread?.Start();
        }

        public void Stop()
        {
            Thread?.Abort();
        }

        private void TaskExecute()
        {
            while (true)
            {
                if (CollectorQueue.Count > 0)
                {
                    var ret = CollectorQueue.TryDequeue(out ICollector collector);
                    if (ret)
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            ExecuteCollection(collector);
                            CollectorQueue.Enqueue(collector);
                        });
                    }
                }
                
                Thread.Sleep(200);
            }
        }

        private void ExecuteCollection(ICollector collector)
        {
            try
            {
                collector.Execute();
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }
            
        }
    }
}
