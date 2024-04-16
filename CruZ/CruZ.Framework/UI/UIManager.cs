using CruZ.Framework.UI;
using System;
using System.Collections.Generic;

namespace CruZ.Framework.UI
{
    /// <summary>
    /// Exposed API of UISystem
    /// </summary>
    public class UIManager
    {
        public static event Action<UIInfo>? MouseClick
        {
            add => UISystem.MouseClick += value;
            remove => UISystem.MouseClick -= value;
        }

        public static UIRoot Root => UISystem.Root;
    }
}