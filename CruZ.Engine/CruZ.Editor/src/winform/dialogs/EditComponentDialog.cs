using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Services;
using CruZ.Utility;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditComponentDialog : Form
    {
        class BoxItem
        {
            public BoxItem(Type compTy, string text)
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
            this.Text = "Component Editor";

            _e = e;

            foreach (var comp in TransformEntity.GetAllComponents(e))
            {
                int id = component_ListBox.Items.Count;

                component_ListBox.Items.Add(
                    new BoxItem(
                        comp.GetType(), 
                        comp.GetType().Name + "*")
                );

                component_ListBox.SetItemCheckState(id, CheckState.Checked);
            }

            var compTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => 
                    !t.IsAbstract && 
                    typeof(Component).IsAssignableFrom(t) && 
                    !_e.HasComponent(t));

            foreach (var compTy in compTypes)
            {
                component_ListBox.Items.Add(
                    new BoxItem(
                        compTy,
                        compTy.Name));
            }

            ok_Button.DialogResult = DialogResult.OK;
            ok_Button.Click += Ok_Button_Click;
        }

        private void Ok_Button_Click(object? sender, EventArgs e)
        {
            for(int i = 0; i < component_ListBox.Items.Count; i++)
            {
                var boxItem = (BoxItem)component_ListBox.Items[i];
                var compTy = boxItem.CompType;

                if (component_ListBox.GetItemChecked(i))
                {
                    if (_e.HasComponent(compTy)) continue;

                    GameApplication.MarshalInvoke(delegate
                    {
                        var comp = ComponentHelper.GetDefaultComponentInstance(compTy);
                        _e.AddComponent(comp);
                    });
                }
                else
                {
                    if(!_e.HasComponent(compTy)) continue;

                    GameApplication.MarshalInvoke(delegate
                    {
                        _e.RemoveComponent(compTy);
                    });
                }
            }

            InvalidatedService.SendInvalidated(InvalidatedEvents.EntityComponentChanged);
        }

        TransformEntity _e;
    }
}
