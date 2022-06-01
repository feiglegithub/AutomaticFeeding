using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public abstract class CollectorBase:ICollector
    {
        protected CollectorBase(string deviceName)
        {
            DeviceName = deviceName;
        }
        public string DeviceName { get; protected set; }
        public abstract void Execute();

    }
}
