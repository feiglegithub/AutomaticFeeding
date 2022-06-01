using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.UI.Common.Controls;
using NJIS.FPZWS.UI.Common.Message;

namespace ArithmeticsTest.Views
{
    public partial class CommandMsgView : UserControl
    {
        public CommandMsgView()
        {
            InitializeComponent();
            ViewInit();
            this.HandleCreated += (sender, args) =>
            {
                BroadcastMessage.Register<CommandMsg>(nameof(CommandMsg), this, msg =>
                {
                    ErrorMsgs.Add(msg);
                    gvbCommand.DataSource = null;
                    gvbCommand.DataSource = ErrorMsgs;
                });
            };
        }

        private List<CommandMsg> ErrorMsgs { get; set; } = new List<CommandMsg>();
        private void ViewInit()
        {
            CommandMsg em;
            gvbCommand.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo(+800)
                    {HeaderText = "操作", FieldName = nameof(em.CommandName), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "耗时", FieldName = nameof(em.TimeSpan), DataType = typeof(TimeSpan), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "开始时间", FieldName = nameof(em.StartTime), DataType = typeof(DateTime), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "结束时间", FieldName = nameof(em.FinishedTime), DataType = typeof(DateTime), ReadOnly = true},
            });
        }
    }
}
