using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.UI.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls
{
    public partial class ItemNamesControl : UserControl,IView
    {

        private TaskControlPresenter _presenter = TaskControlPresenter.GetInstance();
        public const string ReceiveFilesName = nameof(ReceiveFilesName);
        public ItemNamesControl()
        {
            InitializeComponent();
            this.Register<List<SpiltMDBResult>>(ReceiveFilesName, ExecuteBindingFilesName);
            this.BindingPresenter(_presenter);
        }

        private void ExecuteBindingFilesName(List<SpiltMDBResult> results)
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var result in results)
            {
                flowLayoutPanel1.Controls.Add(CreatedFileControl(result));
            }
        }

        private ItemNameControl CreatedFileControl(SpiltMDBResult result)
        {
            ItemNameControl control = new ItemNameControl() {ItemText = result.ItemName};
            control.Enabled = result.FinishedStatus < Convert.ToInt32(FinishedStatus.CreatingSaw);
            return control;
        }
    }
}
