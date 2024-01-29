using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Systems;
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public PropertyGrid Inspector_PropertyGrid  { get => inspector_PropertyGrid; }
        public WorldViewControl WorldViewControl    { get => worldViewControl; }

        private EditorForm()
        {
            KeyPreview = true;

            InitializeComponent();
            worldViewControl.OnSelectedEntityChanged += WorldViewControl_OnSelectedEntityChanged;
            worldViewControl.SceneLoadEvent += WorldViewControl_SceneLoadEvent;
            entities_ComboBox.SelectedIndexChanged += Entities_ComboBox_SelectedIndexChanged;
            entities_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == (Keys.Control | Keys.Z))
            {
                UndoService.Undo();
                return true;
            }

            if(keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                UndoService.Redo();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Entities_ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
                worldViewControl.SelectEntity((TransformEntity)entities_ComboBox.SelectedItem);
        }

        private void WorldViewControl_SceneLoadEvent(object? sender, GameScene e)
        {
            entities_ComboBox.Items.Clear();

            for(int i = 0; i < e.Entities.Count(); i++)
            {
                entities_ComboBox.Items.Add(e.Entities[i]);
            }
        }

        private void WorldViewControl_OnSelectedEntityChanged(object? sender, Components.TransformEntity e)
        {
            Inspector.Instance.DisplayEntity(e);
            Trace.Assert(entities_ComboBox.Items.Contains(e));
            entities_ComboBox.SelectedItem = e;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CacheService.CallWriteCaches();
        }

        private void OpenScene_Clicked(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            var scene = ResourceManager.LoadResource<GameScene>(sceneFile, out _);
            worldViewControl.LoadScene(scene);
        }

        private void SaveScene_Clicked(object sender, EventArgs args)
        {
            if (worldViewControl.CurrentGameScene == null) return;

            try
            {
                ResourceManager.SaveResource(worldViewControl.CurrentGameScene);
            }
            catch(System.Exception e)
            {
                ShowExceptionDialog(e);
            }

        }

        private void File_Menu_Clicked(object sender, EventArgs e)
        {
        }

        private void SaveAsScene_Clicked(object sender, EventArgs e)
        {
            var savePath = DialogHelper.GetSaveScenePath();
            if (savePath == null) return;

            ResourceManager.CreateResource(
                savePath,
                worldViewControl.CurrentGameScene, 
                true);
        }

        private void LoadScene_Clicked(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter Scene name to load", "Load scene Prompt");

            if(string.IsNullOrWhiteSpace(input)) return;

            try
            {
                worldViewControl.LoadScene(SceneManager.GetSceneAssets(input));
            }
            catch (SceneAssetNotFoundException ex)
            {
                ShowExceptionDialog(ex);
            }
        }

        private void ShowExceptionDialog(System.Exception ex)
        {
            MessageBox.Show(
                $"{ex}\nInner Error: {ex.InnerException}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
        
        static EditorForm? _instance;
        public static EditorForm Instance => _instance ??= new EditorForm();
    }
}
