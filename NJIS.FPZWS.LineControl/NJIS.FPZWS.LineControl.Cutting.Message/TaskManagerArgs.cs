using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Message
{
    /// <summary>
    /// 分配任务参数
    /// </summary>
    [Serializable]
    public class AssigningTaskArgs:MqttMessageArgsBase
    {
        public AssigningTaskArgs() : base()
        {

        }
        public DateTime PlanDate { get; set; }
        public string DeviceName { get; set; }
        public List<SpiltMDBResult> TaskList { get; set; }
    }    
    /// <summary>
    /// 推送任务参数
    /// </summary>
    [Serializable]
    public class PushTaskArgs:MqttMessageArgsBase
    {
        public PushTaskArgs() : base()
        {

        }
        public DateTime PlanDate { get; set; }
        public string DeviceName { get; set; }
        public SpiltMDBResult PushTask { get; set; }
    }
}
