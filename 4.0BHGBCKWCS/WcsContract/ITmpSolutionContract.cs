using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface ITmpSolutionContract
    {
        List<TmpSolution> GeTmpSolutions(long groupId);

        bool BulkInsertTmpSolutions(List<TmpSolution> tmpSolutions);


    }
}
