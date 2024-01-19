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

            _positionControl = new();
            InspectorPanel.Controls.Add(_positionControl);

            if (_currentEntity == e) return;

            if (_currentEntity != null)
            {
                _currentEntity.Transform.OnPositionChanged -= UpdatePositionPropertyText;
                _positionControl.OnInputValueChanged -= UpdateEntityPosition;
            }

            _currentEntity = e;

            _positionControl.SetPropertyName(e.Name);
            _positionControl.SetValueText(e.Transform.Position);

            e.Transform.OnPositionChanged += UpdatePositionPropertyText;
            _positionControl.OnInputValueChanged += UpdateEntityPosition;
        }

        private void UpdateEntityPosition(object? sender, Vector3 e)
        {
            _currentEntity.Transform.Position = e;
        }

        private void UpdatePositionPropertyText(Vector3 p)
        {
            _positionControl.SetValueText(p);
        }

        TransformEntity?        _currentEntity;
        Vector3InspectorControl _positionControl;

        static Inspector? _instance;
        static public Inspector Instance => _instance ??= new Inspector();
    }
}