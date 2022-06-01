using NJIS.FPZWS.LineControl.PartMill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Contract
{
    public interface IContract
    {
        List<line_config> GetLineConfigs();

        List<read_labelfile_config> GetReadLabelFileConfigs();

        bool SaveUpFileInfo(List<object> objects);

        List<line_task> GetLineTasks();

        List<mh_task> GetMhTasks();

        Tuple<bool, string> BeginLineTask(line_task lineTask);

        Tuple<bool, string> BeginMhTask(mh_task mhTask);

        ResponseModel FinishedLineTask(int position);

        ResponseModel LineAcceptTask();

        ResponseModel FinishedMhTask();

        

        /// <summary>
        /// 创建退整垛板任务
        /// </summary>
        /// <returns></returns>
        ResponseModel CreatedBackPilerTask();
    }
}
