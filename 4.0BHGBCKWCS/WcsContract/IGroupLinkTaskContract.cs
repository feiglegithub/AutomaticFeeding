using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IGroupLinkTaskContract
    {
        /// <summary>
        /// 获取未创建解决方案的要料任务
        /// </summary>
        /// <returns></returns>
        List<GroupLinkTask> GetUnCreatedSolutionGroupLinkTask();

        bool UpdatedGroupLinkTask(GroupLinkTask groupLinkTask);

        List<GroupLinkTask> GetGroupLinkTasksByPilerNo(int pilerNo);
    }
}
