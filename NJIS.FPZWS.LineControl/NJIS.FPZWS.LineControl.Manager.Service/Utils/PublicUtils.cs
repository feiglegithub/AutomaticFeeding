using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Manager.Service.Utils
{
    public class PublicUtils
    {
        public static PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            string version = "15.5.18";

            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = $"开料管理服务版本：{version}|{detail}";
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

                //已下发的锯切图名称
                string sawFileName = getSawFileName(lineControlCuttingServicePlus, batchNamePilerNoBind.BatchName);

                //已分配给6号锯的垛
                string stackNames = getStackName6(lineControlCuttingServicePlus, batchNamePilerNoBind.BatchName);

                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelation(batchNamePilerNoBind.BatchName, sawFileName, stackNames, SawType.TYPE1,"SawFileName");

                //所有锯切图做完，更新批次状态已完成
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

        /// <summary>
        /// 根据批次号获取已经下发的垛
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static string getStackName(LineControlCuttingServicePlus lineControlCuttingServicePlus,string batchName)
        {
            //已下发给1-5号锯的垛
            string stackNames = "";
            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus.
                GetBatchNamePilerNoBindByBatchName(batchName);
            for (int i = 0; i < listBatchNamePilerNoBind.Count; i++)
            {
                stackNames += $"'{listBatchNamePilerNoBind[i].StackName}'";
                if (i < listBatchNamePilerNoBind.Count - 1)
                {
                    stackNames += ",";
                }
            }

            //已经下发给6号锯的垛
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByBatchNameAndSawType(batchName, SawType.TYPE2);

            if (listBatchNamePilerNoBind.Count > 0 && listCuttingSawFileRelationPlus.Count > 0)
                stackNames += ",";

            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                stackNames += $"'{listCuttingSawFileRelationPlus[i].StackName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                {
                    stackNames += ",";
                }
            }

            if (string.IsNullOrEmpty(stackNames))
                stackNames = "''";

            return stackNames;
        }

        /// <summary>
        /// 根据批次号获取CuttingSawFileRelationPlus表中的所有锯切图名字,但不包含未分配数据
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static string getSawFileName(LineControlCuttingServicePlus lineControlCuttingServicePlus,string batchName)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus =
                lineControlCuttingServicePlus.GetCuttingSawFileRelationPlus(batchName, SawType.TYPE1,
                CuttingSawFileRelationPlusStatus.Unassigned);
            //已下发的锯切图名称
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }
            if (string.IsNullOrEmpty(sawFileName))
                sawFileName += "''";

            return sawFileName;
        }

        /// <summary>
        /// 根据批次号获取CuttingSawFileRelationPlus表中已经分配给6号锯的垛名字
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static string getStackName6(LineControlCuttingServicePlus lineControlCuttingServicePlus, string batchName)
        {
            string stackNames = "";
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlus(batchName, SawType.TYPE2, CuttingSawFileRelationPlusStatus.Unassigned);

            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                stackNames += $"'{listCuttingSawFileRelationPlus[i].StackName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                {
                    stackNames += ",";
                }
            }

            if (string.IsNullOrEmpty(stackNames))
                stackNames = "''";

            return stackNames;
        }
    }
}
