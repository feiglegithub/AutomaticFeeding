using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Controls;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Views
{
    public partial class LineControlView : UserControl,IView
    {

        private LineControlPresenter presenter = new LineControlPresenter();
        public LineControlView()
        {
            InitializeComponent();
            this.RegisterTipsMessage();
            
            this.lineCommandControl1.ExecuteButton.Click += ExecuteButton_Click;
            this.HandleCreated += LineControlView_HandleCreated;
        }

        private void LineControlView_HandleCreated(object sender, EventArgs e)
        {
            Register();
            this.BindingPresenter(presenter);
        }

        private void Register()
        {
            this.Register<List<LineModel>>(LineControlPresenter.BindingData, data =>
            {
                foreach (var control in radGroupBox1.Controls)
                {
                    if (control is LineModelControl lineModel)
                    {
                        var item = data.FirstOrDefault(i => i.LineName == lineModel.LineName.ToString());
                        if (item != null)
                        {
                            lineModel.Data = item;
                        }
                    }
                }
            });

            
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if(sender is Control control)
            if (control.Parent is LineCommandControl lineCommand)
            {
                ELineName fromLineName = ELineName.None;
                ELineName targetLineName = ELineName.None;
                if (Enum.Parse(typeof(ELineName), lineCommand.SelectedFromItem.Value.ToString()) is ELineName lineName1)
                {
                    fromLineName = lineName1;
                }

                if (Enum.Parse(typeof(ELineName), lineCommand.SelectedTargetItem.Value.ToString()) is ELineName lineName2)
                {
                    targetLineName = lineName2;
                }

                short amount = 0;
                int pilerNo = 0;

                if (!short.TryParse(lineCommand.Amount, out amount))
                {
                    Tips("请输入正确的数量");
                    return;
                    
                }
                if (!int.TryParse(lineCommand.Amount, out pilerNo))
                {
                    Tips("请输入正确的垛号");
                    return;

                }


                //short amount = Convert.ToInt16(lineCommand.Amount);
                //int pilerNo = Convert.ToInt32(lineCommand.PilerNo);

                if (fromLineName == ELineName.None || targetLineName == ELineName.None)
                {
                    Tips("请选择正确的位置");
                    return;
                }

                if (fromLineName == targetLineName)
                {
                    Tips("起始位置与目标位置不能相同");
                    return;
                }

                this.Send(LineControlPresenter.WriteTarget,new Tuple<ELineName,ELineName,short,int>(fromLineName,targetLineName, amount,pilerNo));
            }
        }

        private void Tips(string tips)
        {
            this.BeginInvoke((Action)(() => RadMessageBox.Show(tips)));
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            this.Send(LineControlPresenter.Listen,"");
            btnStop.Enabled = true;
            btnListen.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Send(LineControlPresenter.Stop,"");
            btnStop.Enabled = false;
            btnListen.Enabled = true;
        }
    }
}
