using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCS
{
    public class DdjInfo
    {
        public int No;
        public bool IsAuto;
        public bool IsFree;
        public bool IsFinished;
        public string Pallet;
        public int TaskType;
        public string CurrentLocation;
        public string FromWare;
        public string ToWare;
        public long ErrorMsg;
        public bool IsActivation;
        public bool IsFirstOutFree;
        public bool IsSecondInFree;
        public bool IsSecondOutFree;
        public string ErrorList;
    }

    public class Ddj
    {
        public int DdjNo { get; set; }
        public bool Activate { get; set; }
        public long PilerNo { get; set; }
        public string CurrentPos { get; set; }
        public string ToPos { get; set; }
        public int TaskType { get; set; }
        public bool DdjAutomatic { get; set; }
        public bool DmgAutomatic { get; set; }
        public bool State { get; set; }
        public bool OutStationState { get; set; }
        public bool InStationState { get; set; }
        public string ErrorMsg { get; set; }
    }
}
