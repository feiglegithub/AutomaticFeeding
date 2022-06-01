using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WcsModel;
using WCS.model;
using WCS.OPC;

namespace WCS.Communications
{
    /// <summary>
    /// 堆垛车交互类
    /// </summary>
    public class PilerCommunication : PilerCommunicationBase
    {
        public PilerCommunication(EPiler ePiler) : base(ePiler) { }

        public override bool IsFree => OpcSc.CanRun(Convert.ToInt32(Piler));
        public override bool IsFinished => OpcSc.IsFinished(Convert.ToInt32(Piler));

        public override string PilerStackName => OpcSc.RTask(Convert.ToInt32(Piler));

        public override bool WritePilerInStockTask(WMS_Task taskInfo)
        {
            int pilerNo = Convert.ToInt32(Piler);
            return OpcSc.WTaskIn(taskInfo, pilerNo);
        }

        public override bool ClearTaskFinished()
        {
            return OpcSc.ClearTaskFinished(Convert.ToInt32(Piler));
        }

        public override int CurrentColumn => int.Parse(OPCExecute.AsyncRead(Convert.ToInt32(Piler) - 1, 5).ToString());

        public override bool WritePilerOutStockTask(WMS_Task taskInfo)
        {
            return OpcSc.WTaskOut(taskInfo, Convert.ToInt32(Piler));
        }
    }
}
