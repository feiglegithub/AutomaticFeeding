using NJIS.Windows.TemplateBase.UI.Dialogs;
using NJIS.Windows.TemplateBase.UI.UserControls;
using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace NJIS.Windows.TemplateBase
{
    /// <summary>
    /// Defines numerous re-useable utility functions.
    /// </summary>
    public partial class UIUtils
    {
        /// <summary>
        /// Handles an exception.
        /// </summary>
        public static void HandleException(string caption, Exception e)
        {
            ExceptionDlg.Show(caption, e);
        }

        public static T ShowMdiChild<T>(Form owner, bool isDialog = false, int width = 0, int height = 0, int top = -1, int left = -1)
            where T : RadForm, new()
        {
            bool openned = false;
            foreach (var f in owner.MdiChildren)
            {
                if (f is T)
                {
                    f.Activate();
                    openned = true;
                    break;
                }
            }
            if (openned)
                return null;

            if (owner.IsMdiContainer)
            {
                T win = new T();
                win.MdiParent = owner;
                if (width > 0 && height > 0)
                {
                    win.Width = width;
                    win.Height = height;
                }
                else
                {
                    win.WindowState = FormWindowState.Maximized;
                }

                win.LayoutMdi(MdiLayout.Cascade);
                win.Show();
                if (top >= 0 && left >= 0)
                {
                    win.Top = top;
                    win.Left = left;
                }
                return win;
            }
            else
            {
                T win = new T();
                var container = owner as IContainer;
                if (container != null)
                {
                    if (isDialog)
                    {
                        win.ShowDialog();
                    }
                    else
                    {
                        container.AddChild(win);
                    }
                }
                else
                {
                    win.Show();
                }
                return win;
            }
        }

        public static T ShowDialog<T>(Form owner, string caption,
            DataRequireMethod dataNeededEvent = null,
            EventHandler<DialogEventArgs> dataCommittedEvent = null,
            int width = 0, int height = 0) where T : UserControlBase, new()
        {
            T control = new T();
            if (dataNeededEvent != null)
                control.OnDataNeeded += dataNeededEvent;
            if (dataCommittedEvent != null)
                control.OnDataCommitted += dataCommittedEvent;

            FormDlg dlg = new FormDlg(caption, control);
            if (width > 0 && height > 0)
                dlg.SetSize(width, height);
            dlg.ShowDialog(owner);
            return control;
        }

        public static RadTextBox GetReadonlyTextBox()
        {
            RadTextBox txt = new RadTextBox();
            txt.ReadOnly = true;
            txt.BackColor = Color.Transparent;
            txt.UseGenericBorderPaint = false;
            txt.Cursor = Cursors.Default;
            txt.TextBoxElement.Border.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            return txt;
        }

        public static DialogResult Ask(IWin32Window owner, string caption, string text, int defaultButton = 0)
        {
            return MessageBox.Show(owner, text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, defaultButton == 0 ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2);
        }
    }

    public interface IContainer
    {
        void AddChild(RadForm from);
    }

}
