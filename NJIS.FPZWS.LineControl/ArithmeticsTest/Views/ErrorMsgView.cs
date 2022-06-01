using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace ArithmeticsTest.Views
{
    public partial class ErrorMsgView : UserControl,IView
    {

        
        public ErrorMsgView()
        {
            InitializeComponent();
            ViewInit();
            this.HandleCreated += (sender, args) =>
            {
                BroadcastMessage.Register<ErrorMsg>(nameof(ErrorMsg), this, msg =>
                {
                    ErrorMsgs.Add(msg);
                    gvbMsg.DataSource = null;
                    gvbMsg.DataSource = ErrorMsgs;
                });

                BroadcastMessage.Register<List<ErrorMsg>>(nameof(ErrorMsg), this, msgs =>
                {
                    ErrorMsgs.AddRange(msgs);
                    gvbMsg.DataSource = null;
                    gvbMsg.DataSource = ErrorMsgs;
                });
            };
        }
        private List<ErrorMsg> ErrorMsgs { get; set; } = new List<ErrorMsg>();
        private void ViewInit()
        {
            ErrorMsg em;
            gvbMsg.AddColumns(new List<ColumnInfo>()
            {

                new ColumnInfo()
                    {HeaderText = "类型", FieldName = nameof(em.Type), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "命令", FieldName = nameof(em.Command), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo(+900)
                    {HeaderText = "异常信息", FieldName = nameof(em.Msg), DataType = typeof(string), ReadOnly = true},
            });
        }
    }
}
