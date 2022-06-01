using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.Simulator.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Presenters
{
    public class WMSSimulationControlPresenter:PresenterBase
    {
        public const string GetLoadDatas = nameof(GetLoadDatas);
        public const string GetLoadingDatas = nameof(GetLoadingDatas);
        public const string GetLoadedDatas = nameof(GetLoadedDatas);
        public const string LoadMaterial = nameof(LoadMaterial);
        public const string LoadingMaterial = nameof(LoadingMaterial);
        public const string LoadedMaterial = nameof(LoadedMaterial);
        //private IWorkStationContract _workStationContract = null;//WcfClient.GetProxy<IWorkStationContract>();

        //private IWorkStationContract LineControlCuttingContract => _workStationContract ?? (_workStationContract = WcfClient.GetProxy<IWorkStationContract>());

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = SimulatorSettings.Current.IsWcf?WcfClient.GetProxy<ILineControlCuttingContract>():new LineControlCuttingService());

        public WMSSimulationControlPresenter()
        {
            Register<DateTime>(GetLoadDatas, ExecuteGetLoadDatas);
            Register<DateTime>(GetLoadingDatas, ExecuteGetLoadingDatas);
            Register<DateTime>(GetLoadedDatas, ExecuteGetLoadedDatas);
            Register<List<SpiltMDBResult>>(LoadMaterial, ExecuteLoadMaterial);
            Register<List<SpiltMDBResult>>(LoadingMaterial, ExecuteLoadingMaterial);
            Register<List<SpiltMDBResult>>(LoadedMaterial, ExecuteLoadedMaterial);
        }

        private void ExecuteLoadMaterial(object sender,List<SpiltMDBResult> spiltMdbResults)
        {
            foreach (var item in spiltMdbResults)
            {
                item.FinishedStatus = Convert.ToInt32(FinishedStatus.WaitMaterial);
            }
            LineControlCuttingContract.BulkUpdateFinishedStatus(spiltMdbResults);
            ExecuteGetLoadDatas(sender, spiltMdbResults[0].PlanDate);
        }
        private void ExecuteLoadingMaterial(object sender,List<SpiltMDBResult> spiltMdbResults)
        {
            var data = spiltMdbResults.ConvertAll(item =>
                new Tuple<string, LoadMaterialStatus>(item.ItemName, LoadMaterialStatus.LoadingMaterial));
            LineControlCuttingContract.BulkUpdateCuttingStackProductionList(data);
            //foreach (var item in spiltMdbResults)
            //{
            //    item.FinishedStatus = Convert.ToInt32(FinishedStatus.LoadingMaterial);
            //}
            //WorkStationContract.BulkUpdateFinishedStatus(spiltMdbResults);
            ExecuteGetLoadingDatas(sender, spiltMdbResults[0].PlanDate);
        }
        private void ExecuteLoadedMaterial(object sender,List<SpiltMDBResult> spiltMdbResults)
        {
            //foreach (var item in spiltMdbResults)
            //{
            //    item.FinishedStatus = Convert.ToInt32(FinishedStatus.LoadedMaterial);
            //}
            //WorkStationContract.BulkUpdateFinishedStatus(spiltMdbResults);

            var data = spiltMdbResults.ConvertAll(item =>
                new Tuple<string, LoadMaterialStatus>(item.ItemName, LoadMaterialStatus.LoadedMaterial));
            LineControlCuttingContract.BulkUpdateCuttingStackProductionList(data);

            ExecuteGetLoadedDatas(sender, spiltMdbResults[0].PlanDate);
        }

        private void ExecuteGetLoadDatas(object sender, DateTime planTime)
        {
            var recipient = sender;
            try
            {
                List<SpiltMDBResult> resultList = LineControlCuttingContract.GetCanLoadMaterialMdbResults(planTime);
                Send(WMSLoadSimulationControl.ReceiveDatas, recipient, resultList);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message, recipient);
            }

        }

        private void ExecuteGetLoadingDatas(object sender, DateTime planTime)
        {
            var recipient = sender;
            try
            {
                List<SpiltMDBResult> resultList = LineControlCuttingContract.GetCanLoadingMaterialMdbResults(planTime);
                Send(WMSLoadSimulationControl.ReceiveDatas, recipient, resultList);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message, recipient);
            }

        }

        private void ExecuteGetLoadedDatas(object sender, DateTime planTime)
        {
            var recipient = sender;
            try
            {
                List<SpiltMDBResult> resultList = LineControlCuttingContract.GetCanLoadedMaterialMdbResults(planTime);
                Send(WMSLoadSimulationControl.ReceiveDatas, recipient, resultList);
            }
            catch (Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message, recipient);
            }

        }
    }
}
