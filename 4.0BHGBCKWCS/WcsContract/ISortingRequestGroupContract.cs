using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface ISortingRequestGroupContract
    {
        /// <summary>
        /// 更新组为已创建方案状态
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        bool UpdatedGroupIsCreatedSolution(List<long> groupIds);
    }
}
