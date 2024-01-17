using CruZ.Resource;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
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
            _currentOpenningSceneFile = sceneFile;

            var scene = ResourceManager.LoadResource<GameScene>(sceneFile);
            worldViewControl.LoadScene(scene);
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceManager.CreateResource(
                _currentOpenningSceneFile,
                worldViewControl.CurrentGameScene,
                true);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private string _currentOpenningSceneFile;
    }
}
