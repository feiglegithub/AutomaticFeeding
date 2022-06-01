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
    public partial class MachineHandCommandControl : UserControl
    {
        public MachineHandCommandControl()
        {
            InitializeComponent();
            var list = new List<RadListDataItem>
            {
                new RadListDataItem("吊顶板", EMachineHandCommand.GrabFirst),
                new RadListDataItem("放顶板", EMachineHandCommand.PutFirst),
                //new RadListDataItem("去位置1（取放板）", EMachineHandCommand.LoadMaterial1),
                //new RadListDataItem("去位置3（取放板）", EMachineHandCommand.LoadMaterial3),
                new RadListDataItem("去位置4（取放板）", EMachineHandCommand.LoadMaterial4),
                new RadListDataItem("去位置5（取放板）", EMachineHandCommand.LoadMaterial5),
                new RadListDataItem("取板", EMachineHandCommand.GrabBoard),
                new RadListDataItem("放板", EMachineHandCommand.PutBoard),
                new RadListDataItem("放底板", EMachineHandCommand.PutLast),
                new RadListDataItem("取底板", EMachineHandCommand.GrabLast),
                //new RadListDataItem("去位置1（不取放板）", EMachineHandCommand.ToPosition1),
                new RadListDataItem("去位置2（不取放板）", EMachineHandCommand.ToPosition2),
                //new RadListDataItem("去位置3（不取放板）", EMachineHandCommand.ToPosition3),
                new RadListDataItem("去位置4（不取放板）", EMachineHandCommand.ToPosition4),
                new RadListDataItem("去位置5（不取放板）", EMachineHandCommand.ToPosition5),
                new RadListDataItem("清底板数量", EMachineHandCommand.ClearBaseBoard),
            };
            cmbCommand.DropDownMinSize = new Size(200, 300);
            //cmbCommand.FitItemsToSize = true;
            cmbCommand.Items.AddRange(list);
            

        }

        

        public bool IsChecked => checkBox.IsChecked;
        public RadListDataItem SelectedItem
        {
            get => cmbCommand.SelectedItem;
            set => cmbCommand.SelectedItem = value;
        }
        public RadButton ExecuteButton=>btnExecute;
    }
}
