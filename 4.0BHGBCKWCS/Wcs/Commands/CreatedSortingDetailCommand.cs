using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using WcsModel;
using WcsService;
using WcsSortingAlgorithm;

namespace WCS.Commands
{
    public class CreatedSortingDetailCommand:CommandBase<List<GroupLinkTask>,string>
    {
        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract = WcsLogSevice.GetInstance());
        IGroupLinkTaskContract groupLinkTaskContract =  GroupLinkTaskService.GetInstance();
        ISortingTaskStatusContract sortingTaskStatusContract =  SortingTaskStatusService.GetInstance();
        ISortingDetailContract sortingDetailContract = SortingDetailService.GetInstance();

        public CreatedSortingDetailCommand(string baseArg="创建拣选明细") : base(baseArg)
        {
            this.Validating += CreatedSortingDetailCommand_Validating;
        }

        private void CreatedSortingDetailCommand_Validating(object arg1, Args.CancelEventArg<List<GroupLinkTask>> arg2)
        {
            arg2.Cancel = RequestData.Count == 0;
        }

        protected override List<GroupLinkTask> LoadRequest(string baseArg)
        {
            List<GroupLinkTask> groupLinkTasks = new List<GroupLinkTask>();
            try
            {
                groupLinkTasks = groupLinkTaskContract.GetUnCreatedSolutionGroupLinkTask();
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(this.GetType().ToString() +e.Message);
            }

            return groupLinkTasks;
        }

        protected override void ExecuteContent()
        {
            List<GroupLinkTask> groupLinkTasks = RequestData;
            var minGroupId = groupLinkTasks.Min(item => item.GroupId);
            groupLinkTasks = groupLinkTasks.FindAll(item => item.GroupId == minGroupId);
            List<SortingTaskStatus> sortingTaskStatuses = sortingTaskStatusContract.GetUnCreatedSolutionSortingTaskStatuses();

            sortingTaskStatuses = sortingTaskStatuses.FindAll(item => item.GroupId == minGroupId);
            if (sortingTaskStatuses.Count==0) return;
            LogContract.InsertWcsLog($"开始创建拣选明细，拣选组Id:{minGroupId}");
            var obj = CreatedSorting.CreatedSolutions(sortingTaskStatuses, groupLinkTasks);

            var solution1 = CreatedSorting.FilterSolution(obj);

            var details = CreatedSorting.CreatedSortingDetails(solution1, groupLinkTasks);

            var groupId = sortingTaskStatuses[0].GroupId;
            details.ForEach(item => item.GroupId = groupId);
            var pilers = CreatedSorting.ConvertBindingPilerNoes(solution1, groupId);
            ISortingBindingPilerNoContract sortingBindingPilerNoContract = SortingBindingPilerNoService.GetInstance();

            sortingBindingPilerNoContract.BulkInsertSortingBindingPilerNo(pilers);
            ISortingRequestGroupContract sortingRequestGroupContract = SortingRequestGroupService.GetInstance();
            sortingRequestGroupContract.UpdatedGroupIsCreatedSolution(new List<long>() { minGroupId });

            sortingDetailContract.BulkInsertSortingDetail(details);

            LogContract.InsertWcsLog($"拣选明细创建完成，拣选组Id：{minGroupId}");
        }
    }
}
