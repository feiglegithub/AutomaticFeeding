using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PartMill.Simulator.CommunicationBase.Plc;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Interfaces
{
    public interface IMachineHand
    {
        bool IsRequest { get; }

        bool IsFinished { get; }

        PlcOperator Plc { get; }

        bool IsFullBaseBoard { get; }

        bool ClearBoseBoard();

        bool FinishedTaskFeedBack(short commandValue=100);

        //bool ClearLastTask();

        short WriteValue { get; set; }

        short FinishedValue { get; }

    }
}
