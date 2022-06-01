using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Controls
{
    public partial class LineModelControl : UserControl
    {
        private LineModel _data = new LineModel(){LineName = ELineName.None.ToString()};
        public LineModel Data
        {
            get => _data;
            set
            {
                _data = value;
                UpdateView();
            }
        }
        public LineModelControl()
        {
            InitializeComponent();
            UpdateView();
        }

        public string LineName
        {
            get => Data.LineName;
            set => Data.LineName = value;
            //get => (ELineName)Enum.Parse(typeof(ELineName), Data.LineName) ;
            //set => Data.LineName = value.ToString();
        }

        private void UpdateView()
        {
            txtAmount.Text = _data.Amount.ToString();
            txtBackup1.Text = _data.BackupShort.ToString();
            txtBackup2.Text = _data.BackupString;
            txtLineName.Text = _data.LineName.ToString();
            txtHasBoard.Text = _data.HasBoard.ToString();
            txtIsFinished.Text = _data.IsFinished.ToString();
            txtIsRun.Text = _data.NeedRun.ToString();
            txtPilerNo.Text = _data.PilerNo.ToString();
            txtTarget.Text = _data.Target.ToString();
        }
    }
}
