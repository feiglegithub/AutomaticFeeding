using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.MqttClient.Setting;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Service;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    class ProductionTaskControlPresenter:PresenterBase
    {
        /// <summary>
        /// 任务分配 -- 接收消息类型：List(string)  堆垛名
        /// </summary>
        public const string PushMdbFile = nameof(PushMdbFile);
        /// <summary>
        /// 获取可推送的任务 -- 接收信息类型：DataTime 生产时间
        /// </summary>
        public const string GetStackLists = nameof(GetStackLists);

        public const string GetDeviceInfos = nameof(GetDeviceInfos);

        private ILineControlCuttingContract _lineControlCuttingContract = null;//WcfClient.GetProxy<IWorkStationContract>();

        private ILineControlCuttingContract LineControlCuttingContract => 
            _lineControlCuttingContract??(_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>():new LineControlCuttingService());

        public ProductionTaskControlPresenter()
        {
            //Register<List<SpiltMDBResult>>(PushMdbFile, ExecutePushMdbFiles);
            Register<DateTime>(GetStackLists, ExecuteGetStackLists);
            Register<string>(GetDeviceInfos, ExecuteGetDeviceInfos);
        }

        private void ExecuteGetDeviceInfos(object sender,string strDisableValue = "")
        {
            var recipient = sender;
            try
            {
                var deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
                Send(ProductionTaskControl.ReceiveDeviceInfos,recipient, deviceInfos);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取设备列表失败:\n" + e.Message,recipient);
            }


        }

        ///// <summary>
        ///// 任务分配
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="spiltMdbResults"></param>
        //private void ExecutePushMdbFiles(object sender,List<SpiltMDBResult> spiltMdbResults)
        //{
        //    var recipient = sender;
        //    foreach (var item in spiltMdbResults)
        //    {
        //        //item.FinishedStatus = Convert.ToInt32(FinishedStatus.MdbUnloaded);
        //        item.MdbStatus = Convert.ToInt32(FinishedStatus.MdbUnloaded);
        //    }
        //    try
        //    {
        //        //更新任务状态以及mdb下载状态
        //        var ret = LineControlCuttingContract.BulkUpdateMdbStatus(spiltMdbResults);
        //        //var ret = WorkStationContract.BulkUpdateTaskAndMdbStatus(spiltMdbResults);
        //        //var ret = WorkStationContract.BulkUpdateFinishedStatus(spiltMdbResults);
        //        if (ret)
        //        {
        //            var task = spiltMdbResults[0];
        //            SendTipsMessage($"推送{spiltMdbResults.Count}条任务成功",recipient);
        //            var taskGroups = spiltMdbResults.GroupBy(item => item.DeviceName);
        //            //List< AssigningTaskArgs > tasksArgses = new List<AssigningTaskArgs>();
        //            //foreach (var taskGroup in taskGroups)
        //            //{
        //            //    var tasks = taskGroup.ToList();
        //            //    AssigningTaskArgs taskArgs = new AssigningTaskArgs();
        //            //    taskArgs.DeviceName = taskGroup.Key;
        //            //    taskArgs.PlanDate = tasks[0].PlanDate;
        //            //    taskArgs.TaskList = tasks;
        //            //    tasksArgses.Add(taskArgs);
                        
        //            //}
        //            //MqttManager.Current.Publish(EmqttSettings.Current.PcsDownMdbRep, tasksArgses);

        //            ExecuteGetStackLists(sender, spiltMdbResults[0].PlanDate);
        //        }
        //        else
        //        {
        //            SendTipsMessage($"任务推送失败。",recipient);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string strError = "任务推送失败:\n" + e.Message;
        //        SendTipsMessage(strError,recipient);
        //    }

        //}



        private void ExecuteGetStackLists(object sender,DateTime planTime)
        {
            var recipient = sender;
            try
            {
                List<SpiltMDBResult> resultList = LineControlCuttingContract.GetSpiltMDBResults(planTime);
                Send(ProductionTaskControl.ReceiveDatas,recipient, resultList);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message,recipient);
            }

        }
    }
}
