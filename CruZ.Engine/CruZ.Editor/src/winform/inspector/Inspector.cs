using CruZ.Components;
using CruZ.Editor.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class Inspector
    {
        public static PropertyGrid PropertyGrid => EditorForm.GetPropertyGrid();

        public static void DisplayEntity(TransformEntity? e)
        {
            GameApplication.UnregisterDraw(GameApp_Draw);

            if (e != null)
                GameApplication.RegisterDraw(GameApp_Draw);

            SetPropertyGridSelectedObject(e);
        }

        private static void GameApp_Draw(GameTime time)
        {
            RefreshPropertyGrid();
        }

        private static void RefreshPropertyGrid()
        {
            PropertyGridInvoke(delegate
            {
                if (PropertyGrid.ContainsFocus) return;
                PropertyGrid.Refresh();
            });   
        }


        private static void SetPropertyGridSelectedObject(TransformEntity? e)
        {
            PropertyGridInvoke(delegate
            {
                if (e == null) PropertyGrid.SelectedObject = null;
                else PropertyGrid.SelectedObject = new EntityWrapper(e);
            });
        }

        private static void PropertyGridInvoke(Action action)
        {
            if(PropertyGrid.IsDisposed) return;

            if(PropertyGrid.InvokeRequired)
            {
                PropertyGrid.Invoke(action);
            }
            else
            {
                if(PropertyGrid.ContainsFocus) return;
                PropertyGrid.Refresh();
            }
        }
    }
}