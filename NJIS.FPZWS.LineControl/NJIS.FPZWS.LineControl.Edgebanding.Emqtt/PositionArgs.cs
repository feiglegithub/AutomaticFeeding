//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Emqtt
//   文 件 名：PositionArgs.cs
//   创建时间：2018-12-13 16:10
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:10
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.LineControl.Edgebanding.Emqtt
{
    [Serializable]
    public class PositionArgs : MqttMessageArgsBase
    {
        /// <summary>
        ///     位置
        /// </summary>
        public int Place { get; set; }

        public int NextPlace { get; set; }

        /// <summary>
        ///     板件
        /// </summary>
        public string PartId { get; set; }
    }
}