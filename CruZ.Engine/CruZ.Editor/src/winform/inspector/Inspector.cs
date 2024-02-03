using CruZ.Components;
using CruZ.Editor.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Inspector
    {
        PropertyGrid PropertyGrid => EditorForm.GetPropertyGrid();

        public Inspector()
        {
        }

        public void DisplayEntity(TransformEntity? e)
        {
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

        //TODO: refresh property grid every frames or when properties changed
        private void WorldViewControl_DrawEvent(XNA.GameTime obj)
        {
            if(PropertyGrid.ContainsFocus) return;
            PropertyGrid.Refresh();
        }

        static Inspector? _instance;
        static public Inspector Instance => _instance ??= new Inspector();
    }
}