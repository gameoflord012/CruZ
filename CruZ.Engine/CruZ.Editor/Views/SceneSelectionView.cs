using CurZ.Editor;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CruZ.Editor
{
    public class SceneSelectionView : IViewDrawCallback
    {
        public void DrawView(GameTime gameTime)
        {
            ImGui.Begin("Scene Selection", ImGuiWindowFlags.DockNodeHost);

            float sceneListWidth = ImGui.GetContentRegionAvail().X;

            ImGui.Text("Scenes:");
            ImGui.SameLine();
            AddSceneButton(sceneListWidth);

            DrawSceneList(sceneListWidth);

            if(ImGui.Button("Save Scene"))
            {
                SaveScene();
            }

            ImGui.End();
        }

        private void SaveScene()
        {
            SceneManager.SaveScene(SceneManager.CurrentScene, _savePath);
        }

        void AddSceneButton(float total_X)
        {
            string text = "Add Scene";
            ImGui.SameLine(total_X - ImGui.CalcTextSize(text).X);
            if (ImGui.Button(text))
            {
                var files = Dialog.SelectSceneFiles();
                foreach (var file in files)
                {
                    AddSceneFile(file);
                }
            }
        }

        private void DrawSceneList(float sceneListWidth)
        {
            ImGui.PushItemWidth(sceneListWidth);
            if (ImGui.BeginListBox(""))
            {
                foreach (var file in _sceneFiles)
                {
                    var relative = Path.GetFileName(file);
                    if (ImGui.Selectable(relative))
                    {
                        LoadScene(file);
                        _savePath = file;
                    }
                }
                ImGui.EndListBox();
            }
            ImGui.PopItemWidth();
        }

        private void AddSceneFile(string file)
        {
            if (!_sceneFiles.Contains(file)) _sceneFiles.Add(file);
            _sceneFiles.RemoveAll(f => string.IsNullOrEmpty(f));
            _sceneFiles.Sort();
        }

        private void LoadScene(string scenePath)
        {
            var scene = SceneManager.GetSceneFromFile(scenePath);
            if (scene == null) return;

            SceneManager.LoadScene(scene);
        }

        [JsonProperty]
        private List<string> _sceneFiles = new();
        private string _savePath = "";
    }
}