using CruZ.Components;
using CruZ.Editor.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Inspector
    {
        PropertyGrid PropertyGrid => EditorForm.Instance.Inspector_PropertyGrid;

        public void DisplayEntity(TransformEntity e)
        {
            PropertyGrid.SelectedObject = new EntityWrapper(e);
        }

        static Inspector? _instance;
        static public Inspector Instance => _instance ??= new Inspector();
    }
}