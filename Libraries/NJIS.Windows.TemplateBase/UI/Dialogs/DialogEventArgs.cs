using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.Windows.TemplateBase.UI.Dialogs
{
    public class DialogEventArgs
    {
        public DialogEventArgs() {
            Result = DialogResult.Cancel;
        }
        public DialogResult Result { get; set; }
        public bool Cancel { get; set; }
        public object ReturnValue { get; set; }
    }
}
