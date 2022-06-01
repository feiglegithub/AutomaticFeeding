using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class PushTaskControlPresenter:PresenterBase
    {
        /// <summary>
        /// 推送MDB文件 -- 接收消息类型：List(string)  堆垛名
        /// </summary>
        public const string PushTask = nameof(PushTask);
        /// <summary>
        /// 获取堆垛信息 -- 接收信息类型：DataTime 生产时间
        /// </summary>
        public const string GetCanPushTask = nameof(GetCanPushTask);

        public const string GetDeviceInfos = nameof(GetDeviceInfos);

        private ILineControlCuttingContract _lineControlCuttingContract = null;//WcfClient.GetProxy<IWorkStationContract>();

        private ILineControlCuttingContract LineControlCuttingContract => 
            _lineControlCuttingContract ?? (_lineControlCuttingContract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService());

        public PushTaskControlPresenter()
        {
            Register<List<SpiltMDBResult>>(PushTask, ExecutePushTask);
            Register<DateTime>(GetCanPushTask, ExecuteGetCanPushTask);
            Register<string>(GetDeviceInfos, ExecuteGetDeviceInfos);
        }

        private void ExecuteGetDeviceInfos(object sender, string strDisableValue = "")
        {
            var recipient = sender;
            try
            {
                var deviceInfos = LineControlCuttingContract.GetCuttingDeviceInfos();
                Send(PushTaskControl.ReceiveDeviceInfos, recipient, deviceInfos);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取设备列表失败:\n" + e.Message, recipient);
            }


        }

        private void ExecutePushTask(object sender, List<SpiltMDBResult> spiltMdbResults)
        {
            //var recipient = sender;
            //foreach (var item in spiltMdbResults)
            //{
            //    item.FinishedStatus = Convert.ToInt32(FinishedStatus.NeedToSaw);
            //}

            //try
            //{
            //    var ret = LineControlCuttingContract.BulkUpdateFinishedStatus(spiltMdbResults);
            //    if (ret)
            //    {
            //        //var task = spiltMdbResults[0];
            //        SendTipsMessage($"推送{spiltMdbResults.Count}条任务成功", recipient);
            //        var taskGroups = spiltMdbResults.GroupBy(item => item.DeviceName);
            //        List<PushTaskArgs> tasksArgses = new List<PushTaskArgs>();
            //        foreach (var taskGroup in taskGroups)
            //        {
            //            var tasks = taskGroup.ToList();
            //            PushTaskArgs taskArgs = new PushTaskArgs();
            //            taskArgs.DeviceName = taskGroup.Key;
            //            taskArgs.PlanDate = tasks[0].PlanDate;
            //            taskArgs.PushTask = tasks[0];
            //            tasksArgses.Add(taskArgs);

            //        }
            //        MqttManager.Current.Publish(EmqttSettings.Current.PcsTaskRep, tasksArgses);

            //        ExecuteGetCanPushTask(sender, spiltMdbResults[0].PlanDate);
            //    }
            //    else
            //    {
            //        SendTipsMessage($"任务推送失败。", recipient);
            //    }
            //}
            //catch (Exception e)
            //{
            //    string strError = "任务推送失败:\n" + e.Message;
            //    SendTipsMessage(strError, recipient);
            //}

        }

        private void ExecuteGetCanPushTask(object sender, DateTime planTime)
        {
            var recipient = sender;
            try
            {
                List<SpiltMDBResult> resultList = LineControlCuttingContract.GetCanPushSpiltMDBResults(planTime);
                Send(PushTaskControl.ReceiveDatas, recipient, resultList);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message, recipient);
            }

        }
    }
}

