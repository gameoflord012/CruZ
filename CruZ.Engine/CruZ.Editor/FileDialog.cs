using System.Windows.Forms;

namespace CruZ_Engine.Editor
{
    class Dialog
    {
        [STAThread]
        public static string[] SelectSceneFiles()
        {
            OpenFileDialog openFileDialog = new();

            // Set properties for the OpenFileDialog
            openFileDialog.InitialDirectory = CruZ.CONTENT_ROOT;
            openFileDialog.Title = "Select a Scene";
            openFileDialog.Filter = "Scene File (*.scene)|*.scene"; // You can customize the file types allowed
            openFileDialog.Multiselect = true; // Set to true if you want to allow multiple file selection

            // Show the dialog and check if the user clicked OK
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