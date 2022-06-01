//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：FpzCcBase.cs
//   创建时间：2018-10-15 15:25
//   作    者：
//   说    明：
//   修改时间：2018-10-15 15:25
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.ConfigurationCenter.Client;

namespace NJIS.FPZWS.Config.CC
{
    [ConfigCenter("Configs\\FpzCcSetting.json", IsMonitor = false, IsLocal = true)]
    public class FpzCcSetting : NJIS.ConfigurationCenter.Client.ConfigBase<FpzCcSetting>
    {
        public string Env { get; set; } = "uat";

        public string Dep { get; set; } = "";

        public string EmqttClientType { get; set; } = "TaskCenter";
    }
}