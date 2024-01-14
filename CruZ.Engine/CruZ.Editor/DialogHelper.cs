

using CruZ.Resource;
using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class DialogHelper
    {
        [STAThread]
        public static string[] SelectSceneFile(bool multiselect)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = ResourceManager.RESOURCE_ROOT,
                Title = "Select a Scene",
                Filter = "Scene File (*.scene)|*.scene",
                Multiselect = multiselect,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileNames;
            }
            else
            {
                Console.WriteLine("File selection canceled.");
                return [];
            }
        }
    }
}