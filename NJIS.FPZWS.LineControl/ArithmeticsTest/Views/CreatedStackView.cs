using ArithmeticsTest.Helpers;
using ArithmeticsTest.Presenters;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Message;
using Telerik.WinControls;

namespace ArithmeticsTest.Views
{
    public partial class CreatedStackView : UserControl,IView
    {
        private CreatedStackPresenter presenter = new CreatedStackPresenter();
        private List<DeviceInfos> DeviceInfoses { get; set; }
        public CreatedStackView()
        {
            InitializeComponent();
            ViewInit();
            dtpPlanDate.Value = DateTime.Today;
            Register();
            BroadcastMessage.Register<string>("StackName",this,(msg)=>this.txtMsg.Text=msg);
            this.RegisterTipsMessage();
            this.HandleCreated += (sender, args) => this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
            

            DeviceInfoses = new List<DeviceInfos>()
            {
                new DeviceInfos(){DeviceName = "1#",Ratio = 1,LineId = 1,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "2#",Ratio = 1,LineId = 2,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "3#",Ratio = 1,LineId = 3,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "4#",Ratio = 1,LineId = 4,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
                new DeviceInfos(){DeviceName = "5#",Ratio = 1,LineId = 5,OffPartNumRate = 1,PartSpeedRate = 1,PartNumRate = 1,OverlappingRate = 1},
            };
        }

        private List<BookStack> DeviceStacks { get; set; }

        private List<PatternStack> PatternStacks { get; set; } = new List<PatternStack>();

        private List<BatchStack> BatchStacks { get; set; } = new List<BatchStack>();

        private List<TmpBatchInfo> BatchInfos { get; set; } = new List<TmpBatchInfo>();
        private void Register()
        {

            this.Register<List<TmpBatchInfo>>(CreatedStackPresenter.BindingData,  data =>
            {
                BatchInfos = data;
                gvbTmpBatchInfos.DataSource = null;
                gvbTmpBatchInfos.DataSource = BatchInfos;
                this.btnSearch.Enabled = true;
            });

            this.Register<List<BatchStack>>(CreatedStackPresenter.BindingData, data =>
            {
                BatchStacks = data;
                var batchNames = data.Select(item => item.BatchName).ToList();
                cmbBatch.DataSource = batchNames.Count==0?null:batchNames;

                Updated();

                this.btnCreatedStack.Enabled = true;
                this.btnSearch.Enabled = true;
            });

            this.Register<List<BatchGroup>>(CreatedStackPresenter.BindingData, data =>
                {
                    gvbBatchIndex.DataSource = null;
                    gvbBatchIndex.DataSource = data;
                });
            this.Register<bool>(CreatedStackPresenter.BindingData, result =>
            {
                this.btnCreatedStack.Enabled = true;
                this.btnSearch.Enabled = true;
                this.btnSave.Enabled = true;
            });
        }

        private void Updated()
        {
            PatternStacks = BatchStacks.SelectMany(p => p.PatternStacks).ToList(); 
            gvbStack.DataSource = null;
            gvbStack.DataSource = PatternStacks;
            gvbBatchStack.DataSource = null;
            gvbBatchStack.DataSource = BatchStacks;
        }

        private void InitPatternGridView(GridViewBase gridViewBase)
        {
            Pattern pattern;
            gridViewBase.ShowCheckBox = true;
            
            gridViewBase.AddColumns(new List<ColumnInfo>
            {
                new ColumnInfo(+20){ HeaderText="批次", FieldName=nameof(pattern.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="锯切图号", FieldName=nameof(pattern.PatternId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(pattern.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材数", FieldName=nameof(pattern.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="耗时", FieldName=nameof(pattern.TotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="工件数", FieldName=nameof(pattern.PartCount), DataType=typeof(int), ReadOnly=true },
                //new ColumnInfo(){ HeaderText="垛号", FieldName=nameof(pattern.PlanStackName), DataType=typeof(string), ReadOnly=true },
            });
        }

        private void ViewInit()
        {
            TmpBatchInfo t;
            gvbTmpBatchInfos.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+50){ HeaderText="批次", FieldName=nameof(t.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="花色", FieldName=nameof(t.Color), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="耗时", FieldName=nameof(t.TotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材数", FieldName=nameof(t.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件数", FieldName=nameof(t.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="余料数", FieldName=nameof(t.OffPartCount), DataType=typeof(int), ReadOnly=true },
            });

            PatternStack p;
            gvbStack.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+70){ HeaderText="批次", FieldName=nameof(p.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+70){ HeaderText="垛号", FieldName=nameof(p.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材数", FieldName=nameof(p.BookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="余板数", FieldName=nameof(p.OffPartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件数", FieldName=nameof(p.PartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="速度s/块", FieldName=nameof(p.DisplaySpeed), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="总时间", FieldName=nameof(p.TotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="叠板数", FieldName=nameof(p.BookPileCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="叠板率", FieldName=nameof(p.DisplayBookRatio), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(+10){ HeaderText="花色1", FieldName=nameof(p.FirstColor), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材1", FieldName=nameof(p.FirstBookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件1", FieldName=nameof(p.FirstColorPartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="时间1", FieldName=nameof(p.FirstTotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="速度1", FieldName=nameof(p.DisplayFirstSpeed), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(+10){ HeaderText="花色2", FieldName=nameof(p.SecondColor), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材2", FieldName=nameof(p.SecondBookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件2", FieldName=nameof(p.SecondColorPartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="时间2", FieldName=nameof(p.SecondTotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="速度2", FieldName=nameof(p.DisplaySecondSpeed), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(+10){ HeaderText="花色3", FieldName=nameof(p.ThirdColor), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板材3", FieldName=nameof(p.ThirdBookCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="板件3", FieldName=nameof(p.ThirdColorPartCount), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="时间3", FieldName=nameof(p.ThirdTotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(){ HeaderText="速度3", FieldName=nameof(p.DisplayThirdSpeed), DataType=typeof(string), ReadOnly=true },
            });

            //StackDetail sd;
            BatchStack bs;
            gvbBatchStack.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+30){ HeaderText="批次", FieldName=nameof(bs.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="平均速度", FieldName=nameof(bs.DisplayAvgSpeed), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="速度标准差", FieldName=nameof(bs.DisplaySpeedStd), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="速度偏差", FieldName=nameof(bs.DisplaySpeedDeflectionDegree), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="平均时间", FieldName=nameof(bs.DisplayAvgTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="时间标准差", FieldName=nameof(bs.DisplayTimeStd), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){ HeaderText="时间偏差", FieldName=nameof(bs.DisplayTimeDeflectionDegree), DataType=typeof(string), ReadOnly=true },

                new ColumnInfo(-5){ HeaderText="最慢", FieldName=nameof(bs.MaxTotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-5){ HeaderText="最快", FieldName=nameof(bs.MinTotallyTime), DataType=typeof(int), ReadOnly=true },
                new ColumnInfo(-5){ HeaderText="等待(s)", FieldName=nameof(bs.WaitingTime), DataType=typeof(int), ReadOnly=true },

            });
            BatchGroup bg;
            gvbBatchIndex.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(-30){ HeaderText="组号", FieldName=nameof(bg.GroupId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="批次顺序", FieldName=nameof(bg.BatchIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="前批次号", FieldName=nameof(bg.FrontBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="批次号", FieldName=nameof(bg.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="后批次号", FieldName=nameof(bg.NextBatchName), DataType=typeof(string), ReadOnly=true },
            });

            InitPatternGridView(gvbFromPatterns);
            InitPatternGridView(gvbToPatterns);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = dtpPlanDate.Value.Date;
            this.Send(CreatedStackPresenter.GetData, planDate);
            this.btnSearch.Enabled = false;
        }

        private void btnCreatedStack_Click(object sender, EventArgs e)
        {
            this.Send(CreatedStackPresenter.CreatedStack, DeviceInfoses);
            this.btnCreatedStack.Enabled = false;
            this.btnSearch.Enabled = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BatchStacks.Count == 0)
            {
                Tips("没有可保存的数据！");
                return;
            }

            this.Send(CreatedStackPresenter.Save, BatchStacks);
            this.btnCreatedStack.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnSave.Enabled = false;
        }

        private void cmbBatch_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (cmbBatch.SelectedItem == null)
            {
                cmbFromStack.DataSource = null;
                cmbToStack.DataSource = null;
                gvbFromPatterns.DataSource = null;
                gvbToPatterns.DataSource = null;
                return;
            }
            
            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            if(string.IsNullOrWhiteSpace(selectBatchName)) return;
            if(BatchStacks==null || BatchStacks.Count==0) return;
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var fromStackNames = batchStack.PatternStacks.ConvertAll(item => new{StackName= item.Stack.StackName ,Patterns= item.Patterns });
            var toStackNames = batchStack.PatternStacks.ConvertAll(item => new { StackName = item.Stack.StackName, Patterns = item.Patterns });
            cmbFromStack.DataSource = fromStackNames;
            cmbFromStack.DisplayMember = "StackName";
            cmbFromStack.ValueMember = "Patterns";

            cmbToStack.DataSource = toStackNames;
            cmbToStack.DisplayMember = "StackName";
            cmbToStack.ValueMember = "Patterns";

        }

        private void cmbFromStack_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            var selectPatterns = cmbFromStack.SelectedValue as List<Pattern>;
            gvbFromPatterns.DataSource = selectPatterns;
        }

        private void cmbToStack_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            var selectPatterns = cmbToStack.SelectedValue as List<Pattern>;
            gvbToPatterns.DataSource = selectPatterns;
        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            if(gvbFromPatterns.DataSource == null || gvbToPatterns.DataSource == null) return;
            var fromStackName = cmbFromStack.SelectedItem.Text;
            var toStackName = cmbToStack.SelectedItem.Text;
            if (fromStackName == toStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var fromPatterns = gvbFromPatterns.GetSelectItemsData<Pattern>();
            if (fromPatterns.Count == 0)
            {
                Tips("请勾选右移的锯切图");
                return;
            }

            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == fromStackName);

            fromPatterns =
                fromPatternStack.Patterns.FindAll(fromPatterns.Contains);

            var toPatternStack = batchStack.PatternStacks.First(item => item.StackName == toStackName);
            toPatternStack.Patterns.AddRange(fromPatterns);
            fromPatternStack.Patterns.RemoveAll(fromPatterns.Contains);

            gvbFromPatterns.DataSource = null;
            gvbFromPatterns.DataSource = fromPatternStack.Patterns;
            gvbToPatterns.DataSource = null;
            gvbToPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(tips)));
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {
            if (gvbFromPatterns.DataSource == null || gvbToPatterns.DataSource == null) return;
            var fromStackName = cmbFromStack.SelectedItem.Text;
            var toStackName = cmbToStack.SelectedItem.Text;
            if (fromStackName == toStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var toPatterns = gvbToPatterns.GetSelectItemsData<Pattern>();
            if (toPatterns.Count == 0)
            {
                Tips("请勾选左移的锯切图");
                return;
            }

            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var toPatternStack = batchStack.PatternStacks.First(item => item.StackName == toStackName);

            toPatterns =
                toPatternStack.Patterns.FindAll(toPatterns.Contains);

            //fromPatterns =
            //fromPatternStack.Patterns.FindAll(item => fromPatterns.Exists(i => i.PatternId == item.PatternId));

            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == fromStackName);

            fromPatternStack.Patterns.AddRange(toPatterns);

            toPatternStack.Patterns.RemoveAll(toPatterns.Contains);

            gvbFromPatterns.DataSource = null;
            gvbFromPatterns.DataSource = fromPatternStack.Patterns;
            gvbToPatterns.DataSource = null;
            gvbToPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }

        private void btnLeftRight_Click(object sender, EventArgs e)
        {
            if (gvbFromPatterns.DataSource == null || gvbToPatterns.DataSource == null) return;
            var fromStackName = cmbFromStack.SelectedItem.Text;
            var toStackName = cmbToStack.SelectedItem.Text;
            if (fromStackName == toStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var fromPatterns = gvbFromPatterns.GetSelectItemsData<Pattern>();
            if (fromPatterns.Count == 0)
            {
                Tips("请勾选右移的锯切图");
                return;
            }


            var toPatterns = gvbToPatterns.GetSelectItemsData<Pattern>();
            if (toPatterns.Count == 0)
            {
                Tips("请勾选左移的锯切图");
                return;
            }

            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);

            


            var toPatternStack = batchStack.PatternStacks.First(item => item.StackName == toStackName);

            toPatterns =
                toPatternStack.Patterns.FindAll(toPatterns.Contains);

            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == fromStackName);

            fromPatterns =
                fromPatternStack.Patterns.FindAll(fromPatterns.Contains);

            fromPatternStack.Patterns.AddRange(toPatterns);
            toPatternStack.Patterns.AddRange(fromPatterns);

            toPatternStack.Patterns.RemoveAll(toPatterns.Contains);

            fromPatternStack.Patterns.RemoveAll(fromPatterns.Contains);

            gvbFromPatterns.DataSource = null;
            gvbFromPatterns.DataSource = fromPatternStack.Patterns;
            gvbToPatterns.DataSource = null;
            gvbToPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }
    }
}
