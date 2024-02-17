using CruZ.Components;
using CruZ.Editor.Services;
using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        private void InitInspector()
        {
            inspector_PropertyGrid.Invalidated += Inspector_Invalidated;

            InvalidatedService.Register
                (inspector_PropertyGrid,
                "EntityComponentChange", "CurrentSelectedEntityChange");

            inspector_PropertyGrid.PropertyValueChanged += Inspector_PropertyGrid_PropertyValueChanged;
        }

        private void Inspector_PropertyGrid_PropertyValueChanged(object? s, PropertyValueChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ChangeInspectorSelectingEntity(TransformEntity? e)
        {
            GameApplication.UnregisterDraw(GameApp_Draw);

            if (e != null)
                GameApplication.RegisterDraw(GameApp_Draw);

            PropertyGridInvoke(delegate
            {
                if (e == null) inspector_PropertyGrid.SelectedObject = null;
                else inspector_PropertyGrid.SelectedObject = new EntityWrapper(e);
            });
        }

        private void Inspector_Invalidated(object? sender, InvalidateEventArgs e)
        {
            var wrapper = (EntityWrapper)inspector_PropertyGrid.SelectedObject;
            wrapper?.RefreshComponents();
        }

        private void GameApp_Draw(GameTime time)
        {
            RefreshPropertyGrid();
        }

        private void RefreshPropertyGrid()
        {
            PropertyGridInvoke(delegate
            {
                if (inspector_PropertyGrid.ContainsFocus) return;
                inspector_PropertyGrid.Refresh();
            });
        }

        private void PropertyGridInvoke(Action action)
        {
            SafeInvoke(inspector_PropertyGrid ,action);
        }
    }
}