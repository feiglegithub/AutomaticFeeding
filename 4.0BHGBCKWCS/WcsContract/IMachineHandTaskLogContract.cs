using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IMachineHandTaskLogContract
    {
        List<MachineHandTaskLog> GetMachineHandTaskLogs();

        ExecuteResult BeginMachineHandTask(long id);

        ExecuteResult FinishedMachineHandTask(long id);

        ExecuteResult MachineHandCanBegin(long id);

        bool UpdatedMachineHand(MachineHandTaskLog machineHandTaskLog);
    }
}
