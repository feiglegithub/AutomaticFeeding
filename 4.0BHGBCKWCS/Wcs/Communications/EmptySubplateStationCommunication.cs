using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WCS.OPC;

namespace WCS.Communications
{
    public class EmptySubplateStationCommunication:Singleton<EmptySubplateStationCommunication>
    {
        private EmptySubplateStationCommunication() { }

        /// <summary>
        /// 空垫板垛的数量
        /// </summary>
        public int EmptySubplateCount=> OpcHsc.ReadEmptyBuffersCount();
    }
}
