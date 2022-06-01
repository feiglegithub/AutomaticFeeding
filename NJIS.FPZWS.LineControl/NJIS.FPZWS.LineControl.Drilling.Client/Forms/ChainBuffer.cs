//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：ChainBuffer.cs
//   创建时间：2018-11-28 15:51
//   作    者：
//   说    明：
//   修改时间：2018-11-28 15:51
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class ChainBuffer : DrillingForm
    {
        private readonly Guid _clientId = Guid.NewGuid();
        public ChainBuffer()
        {
            InitializeComponent();
        }
        private void ChainBuffer_Load(object sender, System.EventArgs e)
        {
            MqttManager.Current.Subscribe($"{EmqttSetting.Current.PcsInitChainBufferRep}/{_clientId}", new MessageHandler<List<ChainBufferArgs>>(
                (s, chainBuffers) =>
                {
                    flowLayoutPanel1.InvokeExecute(() =>
                    {
                        flowLayoutPanel1.Controls.Clear();
                        foreach (var chainBufferArgse in chainBuffers)
                        {
                            flowLayoutPanel1.Controls.Add(new ChainBufferUserControl(chainBufferArgse));
                        }
                    });

                }));

            MqttManager.Current.Subscribe(EmqttSetting.Current.PcsChainBufferRep, new MessageHandler<ChainBufferArgs>((s, d) =>
             {
                 flowLayoutPanel1.InvokeExecute(() =>
                 {
                     foreach (var cbc in flowLayoutPanel1.Controls)
                     {
                         var cbu = cbc as ChainBufferUserControl;
                         if (cbu != null && cbu.Data != null && cbu.Data.Code == d.Code)
                         {
                             cbu.UpdateSetPartInfo(d.Parts);
                         }
                     }
                 });
             }));
            MqttManager.Current.Publish(EmqttSetting.Current.PcsInitChainBufferReq, _clientId);
        }

        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            MqttManager.Current.Publish(EmqttSetting.Current.PcsInitChainBufferReq, _clientId);
        }
    }
}