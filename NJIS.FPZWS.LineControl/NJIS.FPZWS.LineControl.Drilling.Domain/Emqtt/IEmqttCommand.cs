//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：IEmqttCommand.cs
//   创建时间：2018-11-28 16:09
//   作    者：
//   说    明：
//   修改时间：2018-11-28 16:09
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     Emqtt 命令定义
    /// </summary>
    public interface IEmqttCommand
    {
        string SendTopic { get; set; }
        string ReceiveTopic { get; set; }

        string Name { get; set; }
        void Execute(MqttMessageBase input);
    }
}