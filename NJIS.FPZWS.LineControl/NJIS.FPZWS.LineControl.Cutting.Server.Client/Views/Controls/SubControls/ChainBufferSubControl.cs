using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Message;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls
{
    public partial class ChainBufferSubControl : UserControl
    {
        public ChainBufferSubControl()
        {
            InitializeComponent();
        }

        public void BindingDatas(ChainBufferArgs data)
        {
            this.txtCode.Text = data.CuttingChainBuffer.Code;
            this.txtSize.Text = data.CuttingChainBuffer.Size.ToString();
            this.txtStatus.Text = data.CuttingChainBuffer.Status.ToString();
            this.txtRemark.Text = data.CuttingChainBuffer.Remark;
            this.gridViewBase1.DataSource = data.PartInfoArgses;

        }
    }
}
