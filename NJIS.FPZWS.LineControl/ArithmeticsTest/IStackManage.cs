using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace ArithmeticsTest
{
    public interface IStackManage
    {
        List<Stack> GetStacksByDeviceName(string deviceName);

        Stack GetStackByStackName(string stackName);

        List<Stack> GetStacksByStatus(StackStatus stackStatus);


        List<Stack> GetStacksByPlanDate(DateTime planDate);

        List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate);

        List<StackDetail> GetStackDetailsByStackName(string stackName);

        bool SaveStacks(List<Stack> stacks);

        bool SaveStackDetails(List<StackDetail> stackDetails);
    }
}
