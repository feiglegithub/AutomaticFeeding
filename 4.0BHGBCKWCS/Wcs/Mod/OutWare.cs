using System;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS.Mod
{
    public class OutWare
    {
        public static void OutWareAll()
        {
            SortStationTaskDone();
            OutTaskDone();
            CutTaskDone();
        }

        //2001,2002,2004,2006拣选上料任务完成反馈
        static int[] array1 = { 2001, 2002, 2004, 2006 };
        public static void SortStationTaskDone()
        {
            try
            {
                foreach (int stationNo in array1)
                {
                    if (!OpcHsc.ReadIsTaskDone(stationNo)) { continue; }

                    var pilerno = OpcHsc.ReadPilerNoByStationNo(stationNo);
                    var task = WCSSql.GetTaskByPilerNo(pilerno, 2);
                    if (task == null)
                    {
                        WCSSql.InsertLog($"[SortStationTaskDone]出口{stationNo}完成失败！找不到任务，垛号：{pilerno}", "ERROR");
                        OpcHsc.ClearPLCRequest(stationNo); //清掉反馈信号，以免重复处理
                        continue;
                    }

                    if (stationNo < 2005)
                    {
                        //写花色信息给PLC
                        OpcHsc.WriteProductCodeToStaion(stationNo, task.ProductCode);

                        if (task.HasUpProtect)
                        {
                            OpcHsc.WriteBoradsCountToStaion(stationNo, task.Amount + 2);
                        }
                        else
                        {
                            OpcHsc.WriteBoradsCountToStaion(stationNo, task.Amount + 1);
                        }
                    }
                    else
                    {
                        //写板件数量PLC
                        OpcHsc.WriteBoradsCountToStaion(2006, task.Amount);
                    }

                    WCSSql.UpdateTaskStatus(task.TaskId, 98, "finish");
                    WCSSql.InsertLog($"线体出库完成！垛号：{pilerno}，目标工位：{stationNo}", "LOG");
                    OpcHsc.ClearPLCRequest(stationNo); //清掉反馈信号，以免重复处理
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("线体出库完成失败[SortStationTaskDone]：" + ex.Message, "EEROR");
            }
        }

        //100,GT116，1001,GT208出库完成反馈
        static int[] array2 = { 100, 1001 };
        public static void OutTaskDone()
        {
            try
            {
                foreach (int stationNo in array2)
                {
                    if (!OpcHsc.ReadIsTaskDone(stationNo)) { continue; }

                    var pilerno = OpcHsc.ReadPilerNoByStationNo(stationNo);
                    if (stationNo == 1001)
                    {
                        var manualTask = WCSSql.GetCutting67ManualTask(20, pilerno);
                        if (manualTask != null)
                        {
                            WCSSql.UpdateCutting67ManualTask(manualTask.TaskId, 29);
                            OpcHsc.ClearPLCRequest(stationNo);
                            return;
                        }
                    }
                    var task = WCSSql.GetTaskByPilerNo(pilerno, 2);
                    if (task == null)
                    {
                        WCSSql.InsertLog($"[OutTaskDone]出口{stationNo}完成失败！找不到任务，垛号：{pilerno}", "ERROR");
                        OpcHsc.ClearPLCRequest(stationNo);
                        continue;
                    }

                    switch (task.ToPosition)
                    {
                        case "100":
                            WCSSql.UpdateTaskStatus(task.TaskId, 98, "finish");
                            WCSSql.InsertLog($"出口100出库完成！垛号：{pilerno}", "LOG");
                            break;
                        case "1001":
                            WCSSql.UpdateTaskStatus(task.TaskId, 98, "finish");
                            WCSSql.InsertLog($"出口1001出库完成！垛号：{pilerno}", "LOG");
                            break;
                        default:
                            // 上料出库
                            //创建RGV任务
                            WCSSql.CreateRGVTask(new RGVTask() { TaskType = 1, FromPosition = 3014, ToPosition = int.Parse(task.ToPosition), PilerNo = task.PilerNo });
                            WCSSql.InsertLog($"创建RGV任务成功！垛号：{task.PilerNo}，起始位：3014，目标位：{task.ToPosition}", "LOG");
                            break;
                    }

                    OpcHsc.ClearPLCRequest(stationNo); //清掉反馈信号，以免重复处理
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog("线体出库完成失败[OutTaskDone]：" + ex.Message, "ERROR");
            }
        }

        //3001，3002，3003，3004，3005，3006，3007 上料完成反馈
        static int[] array3 = { 3001, 3002, 3003, 3004, 3005, 3006, 3007 };
        public static void CutTaskDone()
        {
            try
            {
                foreach (int stationNo in array3)
                {
                    if (!OpcHsc.ReadIsTaskDone(stationNo)) { continue; }

                    var pilerno = OpcHsc.ReadPilerNoByStationNo(stationNo);
                    var task = WCSSql.GetTaskByPilerNo(pilerno, 2);

                    if (task == null)
                    {
                        WCSSql.InsertLog($"出口{stationNo}完成失败[CutTaskDone]！找不到任务，垛号：{pilerno}", "ERROR");
                        OpcHsc.ClearPLCRequest(stationNo);
                        continue;
                    }

                    //告诉PLC是否有上保护板
                    OpcHsc.WriteUpToCutStation(stationNo, task.HasUpProtect ? 1 : 0);

                    WCSSql.UpdateTaskStatus(task.TaskId, 98, "finish");
                    WCSSql.InsertLog($"出库完成！垛号：{pilerno}，目标工位：{stationNo}", "LOG");
                    OpcHsc.ClearPLCRequest(stationNo); //清掉反馈信号，以免重复处理
                }
            }
            catch(Exception ex)
            {
                WCSSql.InsertLog("线体出库完成失败[CutTaskDone]：" + ex.Message, "ERROR");
            }
        }
    }
}
