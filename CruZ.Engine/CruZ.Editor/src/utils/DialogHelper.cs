

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

        [STAThread]
        public static string? GetSaveScenePath()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = "scene";
                saveFileDialog.Filter = "Scene files (*.scene)|*.scene|All files (*.*)|*.*";

                DialogResult result = saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}