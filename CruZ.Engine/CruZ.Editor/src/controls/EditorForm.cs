using CruZ.Components;
using CruZ.Resource;
using System;
using System.Drawing.Design;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();

            worldViewControl.OnSelectedEntityChanged += WorldViewControl_OnSelectedEntityChanged;
        }

        private void WorldViewControl_OnSelectedEntityChanged(object? sender, Components.TransformEntity e)
        {
            // TODO: Extract class this function's logic
            if(_currentSelectedEntity != null)
            {
                _currentSelectedEntity.Transform.OnPositionChanged  -= UpdatePositionPropertyText;
                positionInspectorControl.OnInputValueChanged        -= UpdateEntityPosition;
            }

            _currentSelectedEntity = e;

            positionInspectorControl.SetPropertyName(e.Name);
            positionInspectorControl.SetValueText(e.Transform.Position);
            
            e.Transform.OnPositionChanged                   += UpdatePositionPropertyText;
            positionInspectorControl.OnInputValueChanged    += UpdateEntityPosition;
        }

        private void UpdateEntityPosition(object? sender, Vector3 e)
        {
             _currentSelectedEntity.Transform.Position = e;
        }

        private void UpdatePositionPropertyText(Vector3 p)
        {
            positionInspectorControl.SetValueText(p);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CacheService.CallWriteCaches();
        }

        private void openSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            var scene = ResourceManager.LoadResource<GameScene>(sceneFile);
            worldViewControl.LoadScene(scene);
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(worldViewControl.CurrentGameScene == null) return;

            ResourceManager.CreateResource(
                worldViewControl.CurrentGameScene,
                true);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        TransformEntity? _currentSelectedEntity;
    }
}
