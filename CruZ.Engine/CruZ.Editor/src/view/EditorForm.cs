using CruZ.Components;
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public FlowLayoutPanel InspectorPanel => inspectorPanel;

        private EditorForm()
        {
            InitializeComponent();
            worldViewControl.OnSelectedEntityChanged += WorldViewControl_OnSelectedEntityChanged;
        }

        private void WorldViewControl_OnSelectedEntityChanged(object? sender, Components.TransformEntity e)
        {
            Inspector.Instance.DisplayEntity(e);
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
            if (worldViewControl.CurrentGameScene == null) return;

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

        static EditorForm? _instance;

        private void loadSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter Scene name to load", "Load scene Prompt");

            try
            {
                worldViewControl.LoadScene(SceneManager.GetSceneAssets(input));
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(
                    $"{ex}\nInner Error: {ex.InnerException}",
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        public static EditorForm Instance => _instance ??= new EditorForm();
    }
}
