using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCS
{
    class OpcHsInBase
    {
        int cpuNo;
        int itemStart;
        public string hsName;
        public int ledNo;
        
        public OpcHsInBase(int cpu,int item,string name,int led)
        {
            cpuNo = cpu;
            itemStart = item;
            hsName = name;
            ledNo = led;
        }
        public bool IsRequestIn()
        {
            try
            {
                Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        public string ReadPallet()
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 1);
            return item.ReadString();
        }

        //写入值 -1为回退，其余为站台号
        public string WriteIn(int station)
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 2);
            return item.Write(station);
        }

        public int ReadPalletHeight()
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 3);
            return item.ReadInt();
        }


        public int ReadError()
        {
            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 4);
            return item.ReadInt();
        }
    }
}
