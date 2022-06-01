using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms
{
    public partial class DetailForm : Telerik.WinControls.UI.RadForm
    {
        public DetailForm()
        {
            InitializeComponent();
        }

        public void BindingDatas(List<DeviceTaskInfo> datas)
        {
            cuttingAPSSubControl1.BindingDatas(datas);
        }
    }
}
