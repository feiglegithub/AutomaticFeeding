using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace NJIS.Windows.TemplateBase.UI
{
    public interface IOperationEntry
    {
        /// <summary>
        /// Including the button entry event
        /// </summary>
        RadRibbonBarButtonGroup ButtonGroup { get;  }
        void QuickStart();
    }
}
