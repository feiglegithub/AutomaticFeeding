using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCS
{
    class OpcHsEmptyReqBase
    {
        int cpuNo;
        int itemStart;
        public string hsName;
        public int ledNo;

        public OpcHsEmptyReqBase(int cpu, int item, string name, int led)
        {
            cpuNo = cpu;
            itemStart = item;
            hsName = name;
            ledNo = led;
        }

        public bool IsRequestOut()
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

        //写入正在执行的往此口的任务数量
        public string WriteIn(int taskNum)
        {
            Item item1 = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart);
            Item item2 = OpcBaseManage.opcBase.GetItem(cpuNo, itemStart + 3);
            item1.Write(taskNum);
            return item2.Write(1);
        }
    }
}
