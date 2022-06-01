using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    public partial class MinModeControl : UserControl,IView
    {
        /// <summary>
        /// 绑定当前任务 接收数据类型
        /// </summary>
        public const string BindingCurTask = nameof(BindingCurTask);

        public const string BindingDatas = nameof(BindingDatas);
        private CurTaskDetailPresenter presenter = null;
        public MinModeControl()
        {
            InitializeComponent();
            Register();
            this.RegisterTipsMessage();
            presenter = CurTaskDetailPresenter.GetInstance();
            this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        private void Register()
        {
            this.Register<SpiltMDBResult>(BindingCurTask, ExecuteBindingCurTask);
            //this.Register<DataSet>(BindingDatas, ExecuteBindingDatas);
            this.Register<Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>>>(BindingDatas, ExecuteBindingDatas);
            //this.Register<List<CuttingPattern>>(BindingDatas, ExecuteBindingDatas);
        }
        private void ExecuteBindingCurTask(SpiltMDBResult result)
        {
            if (result != null)
            {
                txtCurTask.Text = result.ItemName;
                txtBatchName.Text = result.BatchName;
            }
            else
            {
                txtCurTask.Text = "";
                txtBatchName.Text = "";
                //ViewClear();
            }
        }

        private void ExecuteBindingDatas(
            Tuple<List<CuttingPattern>, DataSet, List<CuttingTaskDetail>, List<CuttingTaskDetail>> tuple)
        {

            CuttingPattern curTask = GetCurTask(tuple.Item1);
            CuttingPattern nextTask = GetNextTask(tuple.Item1);

            lbNextCuttingPattern.Text = nextTask == null
                ? "无"
                : (CuttingSetting.Current.DisplayOldPTN_INDEX
                    ? nextTask.PatternName.ToString()
                    : nextTask.NewPatternName.ToString());
            lbCurCuttingPattern.Text = curTask == null
                ? "无"
                : (CuttingSetting.Current.DisplayOldPTN_INDEX
                    ? curTask.PatternName.ToString()
                    : curTask.NewPatternName.ToString());
        }

        private CuttingPattern GetCurTask(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>().Concat(cuttingPatterns).ToList();

            if (IsBegin(tmpList))
            {
                var errorTasks = GetErrorTasks(tmpList);
                tmpList.RemoveAll(item => errorTasks.Contains(item));
                tmpList.RemoveAll(item => item.FinishedProgress >= 1);
                if (tmpList.Count == 0) return null;
                if (tmpList.Exists(item => item.FinishedProgress > 0 && item.FinishedProgress < 1))
                {
                    return tmpList.Find(item => item.FinishedProgress > 0 && item.FinishedProgress < 1);
                }
                tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                return tmpList[0];

            }

            return cuttingPatterns[0];
        }

        private CuttingPattern GetNextTask(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>().Concat(cuttingPatterns).ToList();
            if (IsBegin(tmpList))
            {
                var errorTasks = GetErrorTasks(tmpList);
                tmpList.RemoveAll(item => errorTasks.Contains(item));
                tmpList.RemoveAll(item => item.FinishedProgress >= 1);
                if (tmpList.Count == 0) return null;
                if (tmpList.Exists(item => item.FinishedProgress > 0 && item.FinishedProgress < 1))
                {
                    tmpList.RemoveAll(item => item.FinishedProgress > 0 && item.FinishedProgress < 1);
                    tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                }
                else
                {
                    tmpList.Sort((x, y) => x.NewPatternName.CompareTo(y.NewPatternName));
                    tmpList.RemoveAt(0);
                }
                return tmpList.Count > 0 ? tmpList[0] : null;
            }

            return cuttingPatterns.Count > 1 ? cuttingPatterns[1] : null;
        }

        /// <summary>
        /// 任务是否开始执行
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private bool IsBegin(List<CuttingPattern> cuttingPatterns)
        {
            return cuttingPatterns.FindAll(item => item.FinishedProgress > 0).Count > 0;
        }

        /// <summary>
        /// 获取异常锯切图任务
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private List<CuttingPattern> GetErrorTasks(List<CuttingPattern> cuttingPatterns)
        {
            List<CuttingPattern> tmpList = new List<CuttingPattern>(cuttingPatterns);
            if (IsBegin(cuttingPatterns))
            {
                var cuttingTasks = GetCuttingTasks(cuttingPatterns);
                tmpList.Sort((x, y) => x.UpdatedTime.CompareTo(y.UpdatedTime));
                var lastCuttingTask = tmpList.Last();
                if (lastCuttingTask.FinishedProgress > 0 && lastCuttingTask.FinishedProgress < 1)
                {
                    cuttingTasks.RemoveAll(item => item.PatternName == lastCuttingTask.PatternName);
                }

                return cuttingTasks;
            }

            return new List<CuttingPattern>();
        }

        /// <summary>
        /// 获取正在开料的锯切图任务
        /// </summary>
        /// <param name="cuttingPatterns"></param>
        /// <returns></returns>
        private List<CuttingPattern> GetCuttingTasks(List<CuttingPattern> cuttingPatterns)
        {
            return cuttingPatterns.FindAll(item => item.FinishedProgress < 1 && item.FinishedProgress > 0).ToList();
        }
    }
}
