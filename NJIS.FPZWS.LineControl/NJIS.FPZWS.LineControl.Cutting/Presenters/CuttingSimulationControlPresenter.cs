using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Presenters
{
    public class CuttingSimulationControlPresenter:PresenterBase
    {
        //private List<CuttingTaskDetail> _cuttingTaskDetails = null;
        public const string GetCurrTasks = nameof(GetCurrTasks);

        public const string GetTaskDetail = nameof(GetTaskDetail);

        public const string BeginCutting = nameof(BeginCutting);

        //private IWorkStationContract _workStationContract = null;

        //private IWorkStationContract LineControlCuttingContract => _workStationContract ?? (_workStationContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<IWorkStationContract>(): new WorkStationService());

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract => 
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService());

        public CuttingSimulationControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<DateTime>(GetCurrTasks,ExecuteGetCurrTasks);
            Register<SpiltMDBResult>(GetTaskDetail, ExecuteGetTaskDetail);
            Register<List<CuttingTaskDetail>>(BeginCutting, ExecuteBeginCutting);
        }

        private void ExecuteBeginCutting(List<CuttingTaskDetail> cuttingTaskDetails)
        {
            //var count = cuttingTaskDetails.Count / 3;
            //for (int i = 0; i < count; i++)
            //{
            //    LineControlCuttingContract.BulkInsertCuttingManualLabelPrinters(new List<CuttingManualLabelPrinter>()
            //    {
            //        ConvertToCuttingManualLabelPrinter(cuttingTaskDetails[i]),
            //        ConvertToCuttingManualLabelPrinter(cuttingTaskDetails[i + 1]),
            //        ConvertToCuttingManualLabelPrinter(cuttingTaskDetails[i + 2]),
            //    });
            //}
            cuttingTaskDetails.RemoveAll(item => item.PartFinishedStatus);
            for (int i = 0; i < cuttingTaskDetails.Count; )
            {
                try
                {
                    LineControlCuttingContract.BulkInsertCuttingManualLabelPrinters(
                        new List<CuttingManualLabelPrinter>()
                        {
                            ConvertToCuttingManualLabelPrinter(cuttingTaskDetails[i]),
                        });
                    i++;
                }
                catch (Exception e)
                {
                    string s = e.Message;
                }
                finally
                {
                    Thread.Sleep(500);
                }
                
                
            }

            ExecuteGetCurrTasks(cuttingTaskDetails[0].PlanDate.Value);
            SendTipsMessage($"开料任务[{cuttingTaskDetails[0].ItemName}]已完成");

        }

        private CuttingManualLabelPrinter ConvertToCuttingManualLabelPrinter(CuttingTaskDetail cuttingTaskDetail)
        {
            CuttingManualLabelPrinter cuttingManualLabelPrinter = new CuttingManualLabelPrinter();
            cuttingManualLabelPrinter.WorkpieceID = cuttingTaskDetail.PartId;
            cuttingManualLabelPrinter.Status = 10;
            return cuttingManualLabelPrinter;
        }

        private void ExecuteGetCurrTasks(DateTime planDate)
        {
            DateTime date = planDate.Date;
            List<SpiltMDBResult> result = LineControlCuttingContract.GetCurrCuttingTasks(CuttingSetting.Current.CurrDeviceName,date);
            List<Tuple<string,List<SpiltMDBResult>>> tuples = new List<Tuple<string, List<SpiltMDBResult>>>();
            foreach (var group in result.GroupBy(item=>item.DeviceName))
            {
                tuples.Add(new Tuple<string, List<SpiltMDBResult>>(group.Key,group.ToList()));
            }

            Send(CuttingSimulationControl.BindingCurrTasks, tuples);
        }

        private void ExecuteGetTaskDetail(SpiltMDBResult task)
        {
             var cuttingTaskDetails = LineControlCuttingContract.GetDeviceCuttingTaskDetail(task.DeviceName,task.ItemName,task.PlanDate);
            cuttingTaskDetails.Sort((x,y)=>x.NewPTN_INDEX.CompareTo(y.NewPTN_INDEX));
            Send(CuttingSimulationControl.BindingTaskParts, cuttingTaskDetails);
        }
    }
}
