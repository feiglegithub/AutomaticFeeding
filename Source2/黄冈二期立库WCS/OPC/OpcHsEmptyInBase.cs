using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCS
{
    class OpcHsEmptyInBase
    {
        int cpuNo;
        int itemStart;
        public string hsName;
        public int ledNo;

        public OpcHsEmptyInBase(int cpu, int item, string name, int led)
        {
            cpuNo = cpu;
            itemStart = item;
            hsName = name;
            ledNo = led;
        }

        public string ReadPallet()
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart);
            return item.ReadString();
        }

        //写入站台号
        public string WriteIn(int station)
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 1);
            return item.Write(station);
        }

        public bool IsRequestIn()
        {
            try
            {
                Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 2);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }
    }
}
