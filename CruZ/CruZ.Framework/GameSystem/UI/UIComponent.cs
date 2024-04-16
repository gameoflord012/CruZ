using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.Framework.GameSystem.UI
{
    /// <summary>
    /// Allow embeded UIControl to a component
    /// </summary>
    public class UIComponent : Component
    {
        public UIComponent()
        {
            EntryControl = new UIControl();
        }

        internal UIControl EntryControl { get; private set; }
    }
}
