using CruZ.Components;
using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public void UpdatePropertyGrid(TransformEntity? e)
        {
            GameApplication.UnregisterDraw(GameApp_Draw);

            if (e != null)
                GameApplication.RegisterDraw(GameApp_Draw);

            SetPropertyGridSelectedObject(e);
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


        private void SetPropertyGridSelectedObject(TransformEntity? e)
        {
            PropertyGridInvoke(delegate
            {
                if (e == null) inspector_PropertyGrid.SelectedObject = null;
                else inspector_PropertyGrid.SelectedObject = new EntityWrapper(e);
            });
        }

        private void PropertyGridInvoke(Action action)
        {
            SafeInvoke(inspector_PropertyGrid ,action);
        }
    }
}