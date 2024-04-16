using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.Framework.GameSystem.UI
{
    internal class UIComponent : Component
    {
        public UIComponent()
        {
            EntryControl = new UIControl();
        }

        internal UIControl EntryControl { get; private set; }
    }
}
