using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using NJIS.FPZWS.UI.Common.Message.Extensions;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls
{
    public partial class ItemNameControl : UserControl,IView
    {
        private TaskControlPresenter _presenter = TaskControlPresenter.GetInstance();
        /// <summary>
        /// 转换结果
        /// </summary>
        public const string ConvertResult = nameof(ConvertResult);
        public ItemNameControl()
        {
            InitializeComponent();
            this.Register<string>(ConvertResult, itemName =>
            {
                var flag = itemName != ItemText.Trim();
                this.btnConvert.Enabled = flag;
                btnConvert.ForeColor = !flag ? Color.Black : Color.Coral;
            });
            this.BindingPresenter(_presenter);
            this.txtMdbName.Click += TxtMdbName_Click;
        }

        private void TxtMdbName_Click(object sender, EventArgs e)
        {
            this.MessageTranSpond(TaskControl.SelectItemName, txtMdbName.Text.Trim());
        }

        public string ItemText
        {
            get => txtMdbName.Text;
            set => txtMdbName.Text = value;
        }

        public new bool  Enabled
        {
            get => btnConvert.Enabled;
            set
            {
                btnConvert.Enabled = value;
                btnConvert.ForeColor = value? Color.Black:Color.Coral;
            }
        }

        //public RadButton MdButton
        //{
        //    get => btnConvert;
        //}

        private void btnConvert_Click(object sender, EventArgs e)
        {
            this.Parent.Enabled = false;
            this.Send(TaskControlPresenter.ConvertToSaw, txtMdbName.Text.Trim());
        }
    }
}
