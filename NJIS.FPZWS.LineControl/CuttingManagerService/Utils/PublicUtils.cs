using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Manager.Utils
{
    public class PublicUtils
    {
        public static PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            string version = "10.3.15";

            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = $"开料管理软件版本：{version}|{detail}";
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

        /// <summary>
        /// 手动下发Saw文件
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="cuttingSawFileRelation"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ManuallyIssueSawFile(LineControlCuttingServicePlus lineControlCuttingServicePlus
            ,CuttingSawFileRelation cuttingSawFileRelation,out string error)
        {
            error = "";
            bool flag = false;

            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus
                .GetBatchNamePilerNoBindByStackName(cuttingSawFileRelation.StackName);

            if (listBatchNamePilerNoBind.Count < 0)
            {
                error = $"BatchNamePilerNoBind表中不存在垛号：{cuttingSawFileRelation.StackName}";
                return flag;
            }

            flag = CuttingSawFileRelationPlusDBUtils.AddAssignedByCuttingSawFileRelation(lineControlCuttingServicePlus,
                    cuttingSawFileRelation);

            if (flag)
            {
                BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];
                //更新板垛剩余数量
                batchNamePilerNoBind.Count -= cuttingSawFileRelation.BoardCount;
                lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);

                //所有锯切图做完，更新批次状态已完成
                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelationByBatchNameAndMinStackIndex(batchNamePilerNoBind.BatchName);
                if (listCuttingSawFileRelation.Count < 1)
                {
                    List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
                    BatchGroupPlus batchGroupPlus = new BatchGroupPlus();

                    batchGroupPlus.BatchName = batchNamePilerNoBind.BatchName;
                    batchGroupPlus.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    batchGroupPlus.UpdatedTime = DateTime.Now;

                    listBatchGroupPlus.Add(batchGroupPlus);

                    lineControlCuttingServicePlus.BulkUpdateBatchGroupPlusStatusByBatchName(listBatchGroupPlus);

                    BatchProductionDetails batchProductionDetails = new BatchProductionDetails();
                    batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                    batchProductionDetails.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    lineControlCuttingServicePlus.UpdateBatchProductionDetailsStatusByBatchName(batchProductionDetails);

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", TriggerType.LineControl,
                        $"所有锯切图已下发，更新BatchGroupPlus、BatchProductionDetails Status已完成({(int)BatchGroupPlusStatus.ProductionIsCompleted})"
                        , LogType.GENERAL));
                }

                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", TriggerType.LineControl, $"手动下发" +
                    $"锯切图：{cuttingSawFileRelation.SawFileName}", LogType.GENERAL));
            }
            else
                error = "数据库新增动作失败！";

            return flag;
        }
    }
}
