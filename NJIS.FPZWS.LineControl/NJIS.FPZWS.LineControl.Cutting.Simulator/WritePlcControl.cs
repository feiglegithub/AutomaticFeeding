using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class WritePlcControl : UserControl,IView
    {
        private readonly MainFormPresenter _presenter = MainFormPresenter.GetInstance();
        public WritePlcControl()
        {
            InitializeComponent();
            this.RegisterTipsMessage();
            this.BindingPresenter(_presenter);
            //this.Disposed += (sender, args) => this.UnBindingPresenter();
        }

        public new string Text
        {
            get => txtPartId.Text;
            set => txtPartId.Text=value;
        }

        public new Color ForeColor
        {
            get => btnWritePartId.ForeColor;
            set => btnWritePartId.ForeColor = value;
        }

        public string ButtonText
        {
            get => btnWritePartId.Text;
            set => btnWritePartId.Text = value;
        }

        private void btnInPartId_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt32(Tag);
            string partId = txtPartId.Text.Trim();
            this.Send(MainFormPresenter.WritePartId,new Tuple<string,int>(partId,position));
        }
    }
}
