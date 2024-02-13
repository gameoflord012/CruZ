using CruZ.Components;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditComponentDialog : Form
    {
        class ComboBoxItem
        {
            public ComboBoxItem(Type compTy, string text)
            {
                CompType = compTy;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }

            public string Text { get; }
            public Type CompType { get; }
        }

        public EditComponentDialog(TransformEntity e)
        {
            InitializeComponent();

            _e = e;

            foreach (var comp in TransformEntity.GetAllComponents(e))
            {
                selectComponent_ComboBox.Items.Add(
                    new ComboBoxItem(
                        comp.GetType(), 
                        comp.GetType().Name + "*")
                );
            }

            var compTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => 
                    !t.IsInterface && 
                    typeof(IComponent).IsAssignableFrom(t) && 
                    !_e.HasComponent(t));

            foreach (var compTy in compTypes)
            {
                selectComponent_ComboBox.Items.Add(
                    new ComboBoxItem(
                        compTy,
                        compTy.Name));
            }

            ok_Button.DialogResult = DialogResult.OK;
            ok_Button.Click += (sender, e) => Close();
        }

        TransformEntity _e;
    }
}
