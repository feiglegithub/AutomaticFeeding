//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：ReportInfo.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class RunedBatchNumber
    {
        public string ProductionLine { get; set; }
        public string BatchNumber { get; set; }
        public DateTime MinInTime { get; set; }
        public DateTime MaxInTime { get; set; }

        public int BatchCount { get; set; }

        public int InCount { get; set; }
        public int OutCount { get; set; }
        public int ConvergeCount { get; set; }

        public int Status { get; set; }

        /// <summary>
        ///     欠件数量
        /// </summary>
        public int LackCount => BatchCount - InCount;
    }

    public class RunReportInfo
    {
        public List<RunningInfo> RunningInfo { get; set; }
        public List<RunTimeInfo> RunTimeInfo { get; set; }
        public List<RunRobotInfo> RunRobotInfo { get; set; }
        public List<RunAnalysisInfo> RunAnalysisInfo { get; set; }
        public List<RunStageAnalysisInfo> RunStageInAnalysisInfo { get; set; }
        public List<RunStageAnalysisInfo> RunStageOutAnalysisInfo { get; set; }
        public List<RunStageAnalysisInfo> RunStageConvergeAnalysisInfo { get; set; }
    }

    public class RunningInfo
    {
        public string PartId { get; set; }
        public string Robot { get; set; }
        public string InScan { get; set; }
        public string InHanding { get; set; }
        public string InHanded { get; set; }
        public string OutHanding { get; set; }
        public string OutHanded { get; set; }
        public string Converging { get; set; }
        public string Converged { get; set; }
    }

    public class RunTimeInfo
    {
        public string PartId { get; set; }
        public string Robot { get; set; }
        public string DeliveryTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ConvergeStopTime { get; set; }
        public string ConvergeTime { get; set; }
    }

    public class RunRobotInfo
    {
        public string Robot { get; set; }
        public string InCount { get; set; }
        public string DeliveryTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ConvergeStopTime { get; set; }
        public string ConvergeTime { get; set; }
    }

    public class RunAnalysisInfo
    {
        public string AllCount { get; set; }
        public string AllRunTime { get; set; }
        public string ReckonPartCount { get; set; }
        public string NormalTime { get; set; }
        public string FaultTime { get; set; }
        public string Capacity { get; set; }
        public string FaultStopTime { get; set; }
        public string Robot { get; set; }
        public string PartCount { get; set; }
        public string RunTime { get; set; }
        public string AvgRunTime { get; set; }
        public string RobotWaitTime { get; set; }
        public string RobotRunProportion { get; set; }
    }

    public class RunStageAnalysisInfo
    {
        public string PartCount { get; set; }
        public string RunTime { get; set; }
        public string ReckonPartCount { get; set; }
        public string NormalTime { get; set; }
        public string FaultTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Capacity { get; set; }
    }
}