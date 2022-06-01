using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using NJIS.FPZWS.LineControl.Manager.Presenters;
using NJIS.FPZWS.LineControl.Manager.ViewModels;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Manager.Views
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
            this.RegisterTipsMessage();
            this.HandleCreated += (sender, args) => this.BindingPresenter(presenter);
            this.Disposed += (sender, args) => this.UnBindingPresenter();
            

            //DeviceInfoses = new List<DeviceInfos>()
            //{
            //    new DeviceInfos(){DeviceName = "1#"},
            //    new DeviceInfos(){DeviceName = "2#"},
            //    new DeviceInfos(){DeviceName = "3#"},
            //    new DeviceInfos(){DeviceName = "4#"},
            //    new DeviceInfos(){DeviceName = "5#"},
            //};
        }

        private List<BookStack> DeviceStacks { get; set; }

        private List<PatternStack> PatternStacks { get; set; } = new List<PatternStack>();

        private List<BatchStack> BatchStacks { get; set; } = new List<BatchStack>();

        private List<TmpBatchInfo> BatchInfos { get; set; } = new List<TmpBatchInfo>();
        private void Register()
        {
            this.Register<List<DeviceInfos>>(CreatedStackPresenter.BindingData, data =>
            {
                gvbDevice.DataSource = data;
            });
            this.Register<List<TmpBatchInfo>>(CreatedStackPresenter.BindingData,  data =>
            {
                BatchInfos = data;
                gvbTmpBatchInfos.DataSource = null;
                gvbTmpBatchInfos.DataSource = BatchInfos;
                //gvbTmpBatchInfos.EndWait();
                this.btnSearch.Enabled = true;
            });

            this.Register<List<BatchStack>>(CreatedStackPresenter.BindingData, data =>
            {
                BatchStacks = data;
                var batchNames = data.Select(item => item.BatchName).ToList();
                cmbBatch.DataSource = batchNames.Count==0?null:batchNames;

                Updated();
                gvbTmpBatchInfos.EndWait();
                this.btnCreatedStack.Enabled = true;
                this.btnSearch.Enabled = true;
            });

            this.Register<List<BatchGroup>>(CreatedStackPresenter.BindingData, data =>
                {
                    gvbBatchGroup.DataSource = null;
                    gvbBatchGroup.DataSource = data;
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
            if (gvbRightPatterns.DataSource is List<Pattern> rightPatterns)
            {
                txtRightBookCount.Text = "实际板材数:" + rightPatterns.Sum(item => item.BookCount);
            }

            if (gvbLeftPatterns.DataSource is List<Pattern> leftPatterns)
            {
                txtLeftBookCount.Text = "实际板材数:" + leftPatterns.Sum(item => item.BookCount);
            }

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

            DeviceInfos di;
            gvbDevice.AddColumns(new List<ColumnInfo>()
            {
                //new ColumnInfo(-20){FieldName = nameof(di.DepartmentId),HeaderText = "部门",DataType = typeof(int)},
                new ColumnInfo(5){FieldName = nameof(di.DeviceName),HeaderText = "设备名",DataType = typeof(string)},
                new ColumnInfo(20){FieldName = nameof(di.DeviceType),HeaderText = "设备类型",DataType = typeof(string)},
                new ColumnInfo(EColumnType.ComboBox){ HeaderText="状态", FieldName=nameof(di.State)
                    , DataType=typeof(int), ReadOnly=true,DataSource = new Dictionary<int,string>(){{0,"禁用"},{1,"启用"}}
                    ,DisplayMember = "value",ValueMember = "key"},
                new ColumnInfo(){FieldName = nameof(di.ProcessName),HeaderText = "工段",DataType = typeof(string)},
                new ColumnInfo(){ HeaderText="设备描述", FieldName=nameof(di.DeviceDescription), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(){FieldName = nameof(di.Remark),HeaderText = "备注",DataType = typeof(string)},
            });
            gvbDevice.SetDefaultSelectRows(row => row.DataBoundItem is DeviceInfos ? ((DeviceInfos)row.DataBoundItem).State != 0 : false);

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
            gvbBatchGroup.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(-30){ HeaderText="组号", FieldName=nameof(bg.GroupId), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-20){ HeaderText="批次顺序", FieldName=nameof(bg.BatchIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="前批次号", FieldName=nameof(bg.FrontBatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(+5){ HeaderText="批次号", FieldName=nameof(bg.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(-10){ HeaderText="后批次号", FieldName=nameof(bg.NextBatchName), DataType=typeof(string), ReadOnly=true },
            });

            InitPatternGridView(gvbLeftPatterns);
            InitPatternGridView(gvbRightPatterns);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var planDate = dtpPlanDate.Value.Date;
            this.Send(CreatedStackPresenter.GetData, planDate);
            gvbDevice.BeginWait();
            gvbTmpBatchInfos.BeginWait();
            this.btnSearch.Enabled = false;
        }

        private void btnCreatedStack_Click(object sender, EventArgs e)
        {
            var devices = gvbDevice.GetSelectItemsData<DeviceInfos>();
            if (devices.Count == 0)
            {
                Tips("请选择分配设备！");
                return;
            }
            this.Send(CreatedStackPresenter.CreatedStack, devices);
            this.btnCreatedStack.Enabled = false;
            this.btnSearch.Enabled = false;
            gvbTmpBatchInfos.BeginWait();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BatchStacks.Count == 0)
            {
                Tips("没有可保存的数据！");
                return;
            }

            var errorStack = BatchStacks.FirstOrDefault(item => item.PatternStacks.Exists(i => i.ActualBookCount > 40));
            if (errorStack != null)
            {
                var stack = errorStack.PatternStacks.First(item => item.ActualBookCount > 40);
                Tips($"批次:{errorStack.BatchName}\n垛号：{stack.StackName}\n板材数超过40,不可保存！\n请手动微调整垛或联系管理员");
                return;
            }

            errorStack = BatchStacks.FirstOrDefault(item => item.PatternStacks.Exists(i => i.Colors.Count > 3));
            if (errorStack != null)
            {
                var stack = errorStack.PatternStacks.First(item => item.Colors.Count > 3);
                Tips($"批次:{errorStack.BatchName}\n垛号：{stack.StackName}\n板材花色超过3种,不可保存！\n请手动微调整垛或联系管理员");
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
                cmbLeftStack.DataSource = null;
                cmbRigthStack.DataSource = null;
                gvbLeftPatterns.DataSource = null;
                gvbRightPatterns.DataSource = null;
                return;
            }
            
            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            if(string.IsNullOrWhiteSpace(selectBatchName)) return;
            if(BatchStacks==null || BatchStacks.Count==0) return;
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var fromStackNames = batchStack.PatternStacks.ConvertAll(item => new
            {
                StackName= item.Stack.StackName ,
                Patterns = new List<Pattern>().Concat(item.Patterns.FindAll(it=>!item.MoveLastStackPatterns.Contains(it))).Concat(item.NextStackPatterns).ToList()
            });
            var toStackNames = batchStack.PatternStacks.ConvertAll(item => new
            {
                StackName = item.Stack.StackName,
                Patterns = new List<Pattern>().Concat(item.Patterns.FindAll(it => !item.MoveLastStackPatterns.Contains(it))).Concat(item.NextStackPatterns).ToList()
            });
            cmbLeftStack.DataSource = fromStackNames;
            cmbLeftStack.DisplayMember = "StackName";
            cmbLeftStack.ValueMember = "Patterns";
            
            cmbRigthStack.DataSource = toStackNames;
            cmbRigthStack.DisplayMember = "StackName";
            cmbRigthStack.ValueMember = "Patterns";

        }

        private void cmbFromStack_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //var selectPatterns = cmbFromStack.SelectedValue as List<Pattern>;
            if (cmbLeftStack.SelectedValue is List<Pattern> selectPatterns)
            {
                gvbLeftPatterns.DataSource = selectPatterns;
                txtLeftBookCount.Text = "实际板材数:"+ selectPatterns.Sum(item=>item.BookCount);
            }
            
        }

        private void cmbToStack_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //var selectPatterns = cmbToStack.SelectedValue as List<Pattern>;
            if(cmbRigthStack.SelectedValue is List<Pattern> selectPatterns)
            {
                gvbRightPatterns.DataSource = selectPatterns;
                txtRightBookCount.Text = "实际板材数:"+ selectPatterns.Sum(item => item.BookCount);
            }
            
        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            if(gvbLeftPatterns.DataSource == null || gvbRightPatterns.DataSource == null) return;
            var leftStackName = cmbLeftStack.SelectedItem.Text;
            var rightStackName = cmbRigthStack.SelectedItem.Text;
            if (leftStackName == rightStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var leftPatterns = gvbLeftPatterns.GetSelectItemsData<Pattern>();
            if (leftPatterns.Count == 0)
            {
                Tips("请勾选右移的锯切图");
                return;
            }

            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == leftStackName);

            leftPatterns =
                fromPatternStack.Patterns.FindAll(leftPatterns.Contains);
            leftPatterns.ForEach(item=>item.ActuallyStackName=item.PlanStackName= rightStackName);
            var toPatternStack = batchStack.PatternStacks.First(item => item.StackName == rightStackName);
            toPatternStack.Patterns.AddRange(leftPatterns);
            fromPatternStack.Patterns.RemoveAll(leftPatterns.Contains);

            gvbLeftPatterns.DataSource = null;
            gvbLeftPatterns.DataSource = fromPatternStack.Patterns;
            gvbRightPatterns.DataSource = null;
            gvbRightPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action) (() => RadMessageBox.Show(tips)));
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {
            if (gvbLeftPatterns.DataSource == null || gvbRightPatterns.DataSource == null) return;
            var leftStackName = cmbLeftStack.SelectedItem.Text;
            var rightStackName = cmbRigthStack.SelectedItem.Text;
            if (leftStackName == rightStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var rightPatterns = gvbRightPatterns.GetSelectItemsData<Pattern>();
            if (rightPatterns.Count == 0)
            {
                Tips("请勾选左移的锯切图");
                return;
            }

            var selectBatchName = cmbBatch.SelectedItem.Value.ToString();
            var batchStack = BatchStacks.First(item => item.BatchName == selectBatchName);
            var toPatternStack = batchStack.PatternStacks.First(item => item.StackName == rightStackName);

            rightPatterns =
                toPatternStack.Patterns.FindAll(rightPatterns.Contains);
            rightPatterns.ForEach(item=>item.ActuallyStackName=item.PlanStackName=leftStackName);
            //fromPatterns =
            //fromPatternStack.Patterns.FindAll(item => fromPatterns.Exists(i => i.PatternId == item.PatternId));

            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == leftStackName);

            fromPatternStack.Patterns.AddRange(rightPatterns);

            toPatternStack.Patterns.RemoveAll(rightPatterns.Contains);

            gvbLeftPatterns.DataSource = null;
            gvbLeftPatterns.DataSource = fromPatternStack.Patterns;
            gvbRightPatterns.DataSource = null;
            gvbRightPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }

        private void btnLeftRight_Click(object sender, EventArgs e)
        {
            if (gvbLeftPatterns.DataSource == null || gvbRightPatterns.DataSource == null) return;
            var fromStackName = cmbLeftStack.SelectedItem.Text;
            var toStackName = cmbRigthStack.SelectedItem.Text;
            if (fromStackName == toStackName)
            {
                Tips("垛号不能一致");
                return;
            }

            var fromPatterns = gvbLeftPatterns.GetSelectItemsData<Pattern>();
            if (fromPatterns.Count == 0)
            {
                Tips("请勾选右移的锯切图");
                return;
            }


            var toPatterns = gvbRightPatterns.GetSelectItemsData<Pattern>();
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
            toPatterns.ForEach(item=>item.ActuallyStackName=item.PlanStackName=fromStackName);

            var fromPatternStack = batchStack.PatternStacks.First(item => item.StackName == fromStackName);

            fromPatterns =
                fromPatternStack.Patterns.FindAll(fromPatterns.Contains);
            fromPatterns.ForEach(item => item.ActuallyStackName = item.PlanStackName = toStackName);

            fromPatternStack.Patterns.AddRange(toPatterns);
            toPatternStack.Patterns.AddRange(fromPatterns);

            toPatternStack.Patterns.RemoveAll(toPatterns.Contains);

            fromPatternStack.Patterns.RemoveAll(fromPatterns.Contains);

            gvbLeftPatterns.DataSource = null;
            gvbLeftPatterns.DataSource = fromPatternStack.Patterns;
            gvbRightPatterns.DataSource = null;
            gvbRightPatterns.DataSource = toPatternStack.Patterns;

            Updated();
        }
    }
}
