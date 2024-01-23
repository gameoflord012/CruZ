using CruZ.Components;
using CruZ.Editor.Controls;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Inspector
    {
        FlowLayoutPanel InspectorPanel => EditorForm.Instance.InspectorPanel;

        public void DisplayEntity(TransformEntity e)
        {
            InspectorPanel.Controls.Clear();

            var positionControl = new Vector3InspectorControl();
            positionControl.SetPropertyName("Postition");
            
            var scaleControl    = new Vector3InspectorControl();
            scaleControl.SetPropertyName("Scale");

            InspectorPanel.Controls.Add(positionControl);
            InspectorPanel.Controls.Add(scaleControl);

            var positionBinding = new Binding.Binding(e.Transform).Property<Vector3>("Position");
            var scaleBinding    = new Binding.Binding(e.Transform).Property<Vector3>("Scale");
            
            positionControl .SetBinding(positionBinding);
            scaleControl    .SetBinding(scaleBinding);
        }

        static Inspector? _instance;
        static public Inspector Instance => _instance ??= new Inspector();
    }
}