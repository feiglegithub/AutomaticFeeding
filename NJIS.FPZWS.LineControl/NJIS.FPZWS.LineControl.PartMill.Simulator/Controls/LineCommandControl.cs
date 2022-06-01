using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Controls
{
    public partial class LineCommandControl : UserControl
    {
        public LineCommandControl()
        {
            InitializeComponent();
            var formItemList = new List<RadListDataItem>
            {
                new RadListDataItem(ELineName.None.ToString(), ELineName.None.ToString()),
                new RadListDataItem(ELineName.Gt5001.ToString(), ELineName.Gt5001.ToString()),
                new RadListDataItem(ELineName.Gt5002.ToString(), ELineName.Gt5002.ToString()),
                new RadListDataItem(ELineName.Xz5003.ToString(), ELineName.Xz5003.ToString()),
                new RadListDataItem(ELineName.Gt5004.ToString(), ELineName.Gt5004.ToString()),
                new RadListDataItem(ELineName.Gt5005.ToString(), ELineName.Gt5005.ToString()),
                new RadListDataItem(ELineName.Gt5006.ToString(), ELineName.Gt5006.ToString()),

            };

            var targetItemList = new List<RadListDataItem>
            {
                new RadListDataItem(ELineName.None.ToString(), ELineName.None.ToString()),
                new RadListDataItem(ELineName.Gt5001.ToString(), ELineName.Gt5001.ToString()),
                new RadListDataItem(ELineName.Gt5002.ToString(), ELineName.Gt5002.ToString()),
                new RadListDataItem(ELineName.Xz5003.ToString(), ELineName.Xz5003.ToString()),
                new RadListDataItem(ELineName.Gt5004.ToString(), ELineName.Gt5004.ToString()),
                new RadListDataItem(ELineName.Gt5005.ToString(), ELineName.Gt5005.ToString()),
                new RadListDataItem(ELineName.Gt5006.ToString(), ELineName.Gt5006.ToString()),

            };
            fromList.DropDownMinSize = new Size(200, 300);
            targetList.DropDownMinSize = new Size(200, 300);
            fromList.Items.AddRange(formItemList);
            targetList.Items.AddRange(targetItemList);

        }

        public RadListDataItem SelectedFromItem
        {
            get => fromList.SelectedItem;
            set => fromList.SelectedItem = value;
        }

        public RadListDataItem SelectedTargetItem
        {
            get => targetList.SelectedItem;
            set => targetList.SelectedItem = value;
        }

        public string PilerNo => txtPilerNo.Text;

        public string Amount => txtAmount.Text;

        public RadButton ExecuteButton => btnExecute;

    }
}
