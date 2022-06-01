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
    public class CuttingSimulationControlPresenter:PresenterBase
    {

        public const string GetCurrTasks = nameof(GetCurrTasks);

        public const string GetTaskParts = nameof(GetTaskParts);

        private ILineControlCuttingContract _lineControlCuttingContract = null;

        private ILineControlCuttingContract LineControlCuttingContract => 
            _lineControlCuttingContract ?? 
            (_lineControlCuttingContract = SimulatorSettings.Current.IsWcf?WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService());

        public CuttingSimulationControlPresenter()
        {
            Register();
        }

        private void Register()
        {
            Register<DateTime>(GetCurrTasks,ExecuteGetCurrTasks);
            Register<SpiltMDBResult>(GetTaskParts, ExecuteGetTaskParts);
        }

        private void ExecuteGetCurrTasks(DateTime planDate)
        {
            DateTime date = planDate.Date;
            List<SpiltMDBResult> result = LineControlCuttingContract.GetCuttingsCurrTasks(date);
            List<Tuple<string,List<SpiltMDBResult>>> tuples = new List<Tuple<string, List<SpiltMDBResult>>>();
            foreach (var group in result.GroupBy(item=>item.DeviceName))
            {
                tuples.Add(new Tuple<string, List<SpiltMDBResult>>(group.Key,group.ToList()));
            }

            Send(CuttingSimulationControl.BindingCurrTasks, tuples);
        }

        private void ExecuteGetTaskParts(SpiltMDBResult task)
        {
            var dt = LineControlCuttingContract.GetTaskPartsInfo(task);
            Send(CuttingSimulationControl.BindingTaskParts,dt);
        }
    }
}
