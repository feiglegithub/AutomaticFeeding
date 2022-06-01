//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Message
//   文 件 名：ChainBufferArgs.cs
//   创建时间：2018-11-30 10:02
//   作    者：
//   说    明：
//   修改时间：2018-11-30 10:02
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Drilling.Message
{
    [Serializable]
    public class ChainBufferArgs : MqttMessageArgsBase
    {
        public ChainBufferArgs()
        {
            CreatedTime = DateTime.Now;
            Parts = new List<PartInfoArgs>();
        }

        public string Code { get; set; }
        public int Size { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }

        public List<PartInfoArgs> Parts { get; set; }
    }
}