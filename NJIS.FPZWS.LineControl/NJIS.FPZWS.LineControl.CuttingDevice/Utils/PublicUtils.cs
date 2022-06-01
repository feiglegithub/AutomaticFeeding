using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Utils
{
    public class PublicUtils
    {
        public static PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            string version = "4.3.6";

            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = $"开料锯软件版本：{version}|{detail}";
            plcLog.LogType = logType.GetFinishStatusDescription().Item2;
            return plcLog;
        }

        /// <summary>
        /// 循环向plc写入int值，直到写入成功
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        public static void PLCWriteInt(PlcOperatorHelper plc,string addr,int value)
        {
            while (true)
            {
                plc.Write(addr, value);

                if (value == plc.ReadLong(addr))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 循环向plc写入string值，直到写入成功
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static void PLCWriteString(PlcOperatorHelper plc, string addr,string value,ushort length)
        {
            while (true)
            {
                plc.Write(addr, value, length);
                if (value.Equals(plc.ReadString(addr,length)))
                {
                    break;
                }
            }
        }
    }
}
