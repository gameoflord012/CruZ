using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new EditorForm());
        }
    }
}