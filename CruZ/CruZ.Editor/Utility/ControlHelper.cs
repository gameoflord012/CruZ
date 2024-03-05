using System;
using System.Windows.Forms;

namespace CruZ.Editor.Utility
{
    static class ControlHelper
    {
        public static void SafeInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}