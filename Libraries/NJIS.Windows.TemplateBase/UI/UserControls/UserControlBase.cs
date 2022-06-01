using NJIS.Windows.TemplateBase.UI.Dialogs;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NJIS.Windows.TemplateBase.UI.UserControls
{
    public delegate object DataRequireMethod();

    [ToolboxItem(false)]
    public class UserControlBase : UserControl
    {
        public event DataRequireMethod OnDataNeeded;
        public event EventHandler<DialogEventArgs> OnDataCommitted;

        public UserControlBase() { }
        public virtual object Arguments { get; private set; }

        public virtual void DataRequire()
        {
            if (OnDataNeeded != null)
                Arguments = OnDataNeeded();
            else
                Arguments = null;
        }

        public virtual void DataCommitted(DialogEventArgs e)
        {
            if (!e.Cancel)
                OnDataCommitted?.Invoke(this, e);
        }
    }
}
