using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PartMill.Simulator.CommunicationBase.Plc;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Interfaces;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Communications
{
    public class MachineHandCommunication: IMachineHand
    {
        private const string _DefaultWriteAddr = "DB100.0";
        private const string _DefaultWriteFinishedAddr = "DB100.6";
        private const string _DefaultFullWriteAddr = "DB100.4";
        private const string _DefaultReadAddr = "DB102.0";
        private const string _DefaultStatusAddr = "DB102.2";
        private const string _DefaultFullReadAddr = "DB102.4";

        private const string ip = "192.168.100.10";
        private PlcOperator _plc = null;
        public PlcOperator Plc => _plc ?? (_plc = new PlcOperator(ip));

        public MachineHandCommunication()
        {
            _plc = new PlcOperator(ip);
            Plc.Connect();
        }

        public bool IsRequest =>  Plc.ReadShort(_DefaultStatusAddr) == 100;

        public bool IsFinished
        {
            get
            {
                var finishedValue = FinishedValue;
                return finishedValue>0;
            }
        }

        public bool IsFullBaseBoard => Plc.ReadShort(_DefaultFullReadAddr) == 17;

        public short FinishedValue => Plc.ReadShort(_DefaultReadAddr);

        public bool ClearBoseBoard()
        {
           var ret = Plc.Write(_DefaultFullWriteAddr, (short) 17);
            return ret.IsSuccess;
        }

        public short WriteValue
        {
            get => Plc.ReadShort(_DefaultWriteAddr);
            set => Plc.Write(_DefaultWriteAddr, value);
        }

        //public bool ClearLastTask()
        //{
        //    var ret = Plc.Write(_DefaultWriteAddr, (short) 0);
        //    return ret.IsSuccess;
        //}

        public bool FinishedTaskFeedBack(short commandValue=100)
        {
            if (FinishedValue != WriteValue) return false;
            var ret = Plc.Write(_DefaultWriteFinishedAddr, (short)100);
            return ret.IsSuccess;
        }



    }
}
