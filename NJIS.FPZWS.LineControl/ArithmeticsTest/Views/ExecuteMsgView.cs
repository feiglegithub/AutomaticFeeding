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
    public partial class ExecuteMsgView : UserControl
    {
        public ExecuteMsgView()
        {
            InitializeComponent();
            ViewInit();
            this.HandleCreated += (sender, args) =>
            {
                BroadcastMessage.Register<ExecuteMsg>(nameof(ExecuteMsg), this, msg =>
                {
                    ErrorMsgs.Add(msg);
                    gvbExecute.DataSource = null;
                    gvbExecute.DataSource = ErrorMsgs;
                });
            };
        }

        private List<ExecuteMsg> ErrorMsgs { get; set; } = new List<ExecuteMsg>();
        private void ViewInit()
        {
            ExecuteMsg em;
            gvbExecute.AddColumns(new List<ColumnInfo>()
            {
                new ColumnInfo()
                    {HeaderText = "对象", FieldName = nameof(em.Object), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "类型", FieldName = nameof(em.Type), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo()
                    {HeaderText = "命令", FieldName = nameof(em.Command), DataType = typeof(string), ReadOnly = true},
                new ColumnInfo(+800)
                    {HeaderText = "执行信息", FieldName = nameof(em.Msg), DataType = typeof(string), ReadOnly = true},
            });
        }
    }
}
