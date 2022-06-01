using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WcsModel;
using WCS.Interfaces;
using WCS.model;

namespace WCS.Communications
{
    public abstract class PilerCommunicationBase: IPiler
    {
        internal EPiler _piler;
        public virtual EPiler Piler { get=> _piler; internal set=> _piler=value; }

        public PilerCommunicationBase(EPiler ePiler)
        {
            _piler = ePiler;
        }

        

        /// <summary>
        /// 堆垛机是否空闲
        /// </summary>
        /// <returns></returns>
        public abstract bool IsFree { get; }

        /// <summary>
        /// 读取堆垛车的垛号
        /// </summary>
        /// <returns></returns>
        public abstract string PilerStackName { get;  }

        public abstract bool IsFinished { get; }

        /// <summary>
        /// 给堆垛车写任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public abstract bool WritePilerInStockTask(WMS_Task taskInfo);

        /// <summary>
        /// 给堆垛车写出库任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public abstract bool WritePilerOutStockTask(WMS_Task taskInfo);

        public abstract bool ClearTaskFinished();
        public abstract int CurrentColumn { get; }
    }
}
