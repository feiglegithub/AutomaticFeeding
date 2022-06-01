using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    public interface IStackManage
    {
        List<Stack> GetStacksByDeviceName(string deviceName);

        Stack GetStackByStackName(string stackName);

        List<Stack> GetStacksByStatus(StackStatus stackStatus);


        List<Stack> GetStacksByPlanDate(DateTime planDate);

        List<StackDetail> GetStackDetailsByPlanDate(DateTime planDate);

        List<StackDetail> GetStackDetailsByStatus(BookStatus bookStatus);

        List<StackDetail> GetStackDetailsByStackName(string stackName);

        bool SaveStacks(List<Stack> stacks);

        bool SaveStackDetails(List<StackDetail> stackDetails);

        void RemoveFinished(DateTime planDate);
    }
}
