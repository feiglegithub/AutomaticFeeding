using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WcsModel;
using WCS.OPC;

namespace WCS.Communications
{
    /// <summary>
    /// Rgv交互类
    /// </summary>
    public class RgvCommunication:Singleton<RgvCommunication>
    {
        private RgvCommunication() { }

        /// <summary>
        /// 是否空闲
        /// </summary>
        public bool IsFree => OpcHsc.RGVCanDo();

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished => OpcHsc.ReadRGVFinished();

        /// <summary>
        /// 垛号
        /// </summary>
        public int StackNo => OpcHsc.ReadRGVPilerNo();

        /// <summary>
        /// 收到完成信号后，处理完毕需要给Rgv回写一个FeedBack信号
        /// </summary>
        public bool FeedBackRgv()
        {
            return OpcHsc.FeedBackRGV();
        }

        /// <summary>
        /// Rgv是否接收任务
        /// </summary>
        /// <returns></returns>
        public bool RgvIsAcceptTask()
        {
            return OpcHsc.ReadRGVAccepted();
        }

        /// <summary>
        /// 清除Rgv任务
        /// </summary>
        /// <returns></returns>
        public bool ClearRgvTask()
        {
            return OpcHsc.ClearRGVTask();
        }

        /// <summary>
        /// 写任务
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="stackNo"></param>
        /// <returns></returns>
        public bool WriteTask(int from, int to, int stackNo)
        {
            return OpcHsc.WriteRGVTask(from, to, stackNo);
        }
    }
}
