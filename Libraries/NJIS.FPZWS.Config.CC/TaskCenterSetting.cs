//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：TaskCenterSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class TaskCenterSetting : ConfigWapper<TaskCenterSetting>
    {
        public TaskCenterSetting()
        {
        }

        public string ClientType { get; set; } = "Business";
        public string IP { get; set; }
        public int Port { get; set; } = 1883;
        public string User { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string DataInitTopic { get; set; }
        public string DataDistributionTopic { get; set; } = "/sfy/pdd/tc/distribution/";
        public int EnableHeartbeat { get; set; } = 1;
        public string HeartbeatTopic { get; set; } = "/sfy/heartbeat";
        public int HeartbeatInterval { get; set; } = 5000;
        public override string Path { get; protected set; } = "TaskCenterSetting";

        public override string GetPath()
        {
            var depStr = "";
            if (!string.IsNullOrEmpty(FpzCcSetting.Current.Dep))
            {
                depStr = $"{FpzCcSetting.Current.Dep}";
            }
            else
            {
                depStr = "湖北未来工厂项目";
            }

            var s = "";
            if (!string.IsNullOrEmpty(FpzCcSetting.Current.EmqttClientType))
            {
                s = "_" + FpzCcSetting.Current.EmqttClientType;
            }
            return $"{depStr}/{FpzCcSetting.Current.Env}/{Path}{s}";
        }
    }
}