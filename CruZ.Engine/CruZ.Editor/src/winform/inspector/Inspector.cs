using CruZ.Components;
using CruZ.Editor.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Inspector
    {
        public static PropertyGrid PropertyGrid => EditorForm.GetPropertyGrid();

        public static void DisplayEntity(TransformEntity? e)
        {
            GameApplication.UnregisterDraw(GameApp_Draw);

            if(e != null)
                GameApplication.RegisterDraw(GameApp_Draw);

            if(PropertyGrid.InvokeRequired)
            {
                Action safeInvoke = delegate { DisplayEntity(e); };
                PropertyGrid.Invoke(safeInvoke);
            }
            else
            {
                if(e == null) PropertyGrid.SelectedObject = null;
                else PropertyGrid.SelectedObject = new EntityWrapper(e);
            }
        }

        private static void GameApp_Draw(GameTime time)
        {
            RefreshPropertyGrid();
        }

        private static void RefreshPropertyGrid()
        {
            if(PropertyGrid.InvokeRequired)
            {
                Action safeInvoke = delegate { RefreshPropertyGrid(); };
                PropertyGrid.Invoke(safeInvoke);
            }
            else
            {
                if(PropertyGrid.ContainsFocus) return;
                PropertyGrid.Refresh();
            }
        }
    }
}