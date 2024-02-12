using CruZ.Scene;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class LoadRuntimeSceneForm : Form
    {
        public string ReturnSceneName { get; private set; }
        public LoadRuntimeSceneForm()
        {
            InitializeComponent();

            Text = "Select runtime scene";

            selectScene_ComboBox.SelectedIndexChanged += SelectScene_SelectedIndexChanged;
            
            ok_Button.DialogResult = DialogResult.OK;
            ok_Button.Click += (sender, e) => Close();

            foreach (var name in SceneManager.GetSceneNames())
            {
                selectScene_ComboBox.Items.Add(name);
            }
        }

        private void SelectScene_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ReturnSceneName = (string)selectScene_ComboBox.SelectedItem;
        }
    }
}
