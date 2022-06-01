using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ningji.PanelSorting.DataHelper.Model;
using Ningji.PanelSorting.WinApp.UI.Dialogs;
using NJIS.AppUtility.Helper;
using NJIS.AppUtility.Localization;
using Telerik.WinControls.UI;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace Ningji.PanelSorting.WinApp.UI.UserControls
{
    public partial class UCEditRackTypecs : UserControlBase
    {
        SYS_RackType info;

        public UCEditRackTypecs()
        {
            InitializeComponent();
            
            Load += UCEditRackTypecs_Load;
        }

        private void UCEditRackTypecs_Load(object sender, EventArgs e)
        {
          
        }        

        public override void DataRequire()
        {
            base.DataRequire();
            if (Arguments != null)
            {
                info = (SYS_RackType)Arguments;
                bindingSource1.DataSource = info;
                //layout.LoadNjLayout("SYS_RackType");
                if (info.RackTypeId == 0)
                {
                    lcCreatedTime.Visibility = lcCreatedTimeValue.Visibility = lcCreatedBy.Visibility = lcCreatedByValue.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    lcModifieTime.Visibility = lcModifiedTimeValue.Visibility = lcModifiedBy.Visibility = lcModifiedByValue.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                }

            }
        }


        public override void DataCommitted(DialogEventArgs e)
        {
            info = (SYS_RackType)bindingSource1.DataSource;
            info.ModifiedBy = "Ben";
            info.ModifiedTime = DateTime.Now;
            DataHelper.Manager.SYS_RackTypeManager mgr = new DataHelper.Manager.SYS_RackTypeManager();
            if (info.RackTypeId > 0)
                mgr.Update(info);
            else
            {
                info.CreatedBy = "Ben";
                info.CreatedTime = DateTime.Now;
                mgr.Insert(info);
            }
            base.DataCommitted(e);
        }
        
    }
}
