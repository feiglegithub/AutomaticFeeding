using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs;
using NJIS.FPZWS.UI.Common.Message;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class MessageControlPresenter:PresenterBase
    {
        public const string BindingDatas = nameof(BindingDatas);
        public MessageControlPresenter()
        {
            Register();
        }
        private List<PcsLogicAlarmArgs> pcsLogicAlarmArgses = new List<PcsLogicAlarmArgs>();
        private void Register()
        {
            Register<PcsLogicAlarmArgs>(EmqttSettings.Current.PcsAlarmRep, ExecuteReciveDatas, EExecutionMode.Asynchronization, true);
            Register<PartInfoQueueArgs>(EmqttSettings.Current.PcsInitQueueRep, ExecuteReciveDatas, EExecutionMode.Asynchronization, true);
        }

        private void ExecuteReciveDatas(PcsLogicAlarmArgs data)
        {
            pcsLogicAlarmArgses.Add(data);
            this.Send(BindingDatas, pcsLogicAlarmArgses);
        }

        private void ExecuteReciveDatas(PartInfoQueueArgs data)
        {
            this.Send(BindingDatas,  data );
        }

        private void Input(object sender, PcsLogicAlarmArgs e)
        {
            string result = "入板失败";
            string message = $"{e.Value}--{e.CreatedTime}";
        }

        private void Input(object sender, PartInfoQueueArgs e)
        {
            string result = "入板成功";
            string message = $"位置:{e.Position}扫码入板,板件号:{e.PartId},批次:{e.BatchName}---{e.CreatedTime}";
        }
    }
}
