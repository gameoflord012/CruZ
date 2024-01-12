

using CruZ.Resource;
using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Dialog
    {
        [STAThread]
        public static string[] SelectSceneFiles()
        {
            OpenFileDialog openFileDialog = new();

            openFileDialog.InitialDirectory = ResourceManager.RESOURCE_ROOT;
            openFileDialog.Title = "Select a Scene";
            openFileDialog.Filter = "Scene File (*.scene)|*.scene";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileNames;
            }
            else
            {
                Console.WriteLine("File selection canceled.");
                return new string[] { };
            }
        }
    }
}