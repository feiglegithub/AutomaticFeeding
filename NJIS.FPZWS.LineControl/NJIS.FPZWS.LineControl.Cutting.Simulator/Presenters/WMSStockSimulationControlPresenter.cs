using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.Simulator.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Presenters
{
    public class WMSStockSimulationControlPresenter:PresenterBase
    {
        /// <summary>
        /// 获取未备料的数据 -- 接收数据类型<see cref="DateTime"/> 生产日期
        /// </summary>
        public const string GetUnStockDatas = nameof(GetUnStockDatas);

        /// <summary>
        /// 获取正在备料的数据 -- 接收数据类型<see cref="DateTime"/> 生产日期
        /// </summary>
        public const string GetStockingDatas = nameof(GetStockingDatas);

        /// <summary>
        /// 开始备料
        /// </summary>
        public const string BeginStock = nameof(BeginStock);

        /// <summary>
        /// 备料完毕
        /// </summary>
        public const string EndStock = nameof(EndStock);

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ??
            (SimulatorSettings.Current.IsWcf
                ? WcfClient.GetProxy<ILineControlCuttingContract>()
                : new LineControlCuttingService());

        public WMSStockSimulationControlPresenter()
        {
            Register<DateTime>(GetUnStockDatas, ExecuteGetUnStockDatas);
            Register<DateTime>(GetStockingDatas, ExecuteGetStockingDatas);
            Register<List<WMSCuttingStackList>>(BeginStock, ExecuteBeginStock);
            Register<List<WMSCuttingStackList>>(EndStock, ExecuteEndStock);
        }

        private void ExecuteGetUnStockDatas(DateTime planDate)
        {
            var result = LineControlCuttingContract.GetWmsCuttingStackLists(planDate.Date, false);
            List<WMSCuttingStackList> datas = new List<WMSCuttingStackList>();
            foreach (var group in result.GroupBy(item => item.StackName))
            {
                var tmpList = group.ToList();
                var data = tmpList.FirstOrDefault();
                if (data == null) continue;
                datas.Add(data);
            }

            Send(WMSStockSimulationControl.BindingDatas, datas);
        }

        private void ExecuteGetStockingDatas(DateTime planDate)
        {
            List<WMSCuttingStackList> result = LineControlCuttingContract.GetWmsCuttingStackLists(planDate.Date, true);
            List<WMSCuttingStackList> datas = new List<WMSCuttingStackList>();
            foreach (var group in result.GroupBy(item=>item.StackName))
            {
                var tmpList = group.ToList();
                var data = tmpList.FirstOrDefault();
                if(data==null) continue;
                datas.Add(data);
                //datas.Add(data??tmpList.Find(item => item.WMSStatus == 0));
            }
            Send(WMSStockSimulationControl.BindingDatas, datas);
        }

        private void ExecuteBeginStock(List<WMSCuttingStackList> wmsCuttingStackLists)
        {
            if (wmsCuttingStackLists == null || wmsCuttingStackLists.Count == 0) return;
            wmsCuttingStackLists.ForEach(item=>item.WMSStatus=1);
            var wms = wmsCuttingStackLists[0];
            LineControlCuttingContract.UpdateWMSCuttingStackLists(wmsCuttingStackLists);
            ExecuteGetStockingDatas(wms.PlanDate.Value.Date);
        }

        private void ExecuteEndStock(List<WMSCuttingStackList> wmsCuttingStackLists)
        {
            if (wmsCuttingStackLists == null || wmsCuttingStackLists.Count == 0) return;
            List<WMSStacktFeedBack> wmsStacktFeedBacks = new List<WMSStacktFeedBack>();
            wmsCuttingStackLists.ForEach(item=> wmsStacktFeedBacks.Add(ConvertToWmsStacktFeedBack(item)));
            LineControlCuttingContract.BulkInsertWMSStacktFeedBack(wmsStacktFeedBacks);
            ExecuteGetStockingDatas(wmsCuttingStackLists[0].PlanDate.Value.Date);
        }


        private WMSStacktFeedBack ConvertToWmsStacktFeedBack(WMSCuttingStackList wmsCuttingStackList)
        {
            WMSStacktFeedBack wmsStacktFeedBack = new WMSStacktFeedBack()
            {
                IsSuccess = true,
                StackName = wmsCuttingStackList.StackName
            };
            return wmsStacktFeedBack;
        }
    }
}
