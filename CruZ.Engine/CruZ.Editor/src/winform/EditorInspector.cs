using Assimp;
using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Services;
using CruZ.Editor.Utility;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor.src.winform
{
    public partial class EditorInspector : UserControl
    {
        public EditorInspector()
        {
            InitializeComponent();
        }

        public void Init(EditorApplication editor)
        {
            _editor = editor;

            entities_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            InvalidatedService.Register
                (inspector_PropertyGrid,
                InvalidatedEvents.EntityComponentChanged,
                InvalidatedEvents.SelectingEntityChanged);

            RegisterEvents();
        }

        #region Event_Handlers
        private void Editor_CurrentSceneChanged(GameScene? scene)
        {
            UpdateEntityComboBox(scene);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void Editor_SelectingEntityChanged(TransformEntity? e)
        {
            UpdatePropertyGrid(e);
        }

        //private void PropertyGrid_PropertyValueChanged(object? s, PropertyValueChangedEventArgs e)
        //{
        //    e.ChangedItem.
        //}

        private void EntityComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _editor.SelectEntity((TransformEntity)entities_ComboBox.SelectedItem);
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
        #endregion

        #region Privates
        private void UpdateEntityComboBox(GameScene? scene)
        {
            entities_ComboBox.SafeInvoke(delegate
            {
                entities_ComboBox.Items.Clear();

                if (scene == null) return;

                for (int i = 0; i < scene.Entities.Count(); i++)
                {
                    var e = scene.Entities[i];
                    entities_ComboBox.Items.Add(e);
                }
            });
        }
 
        private void UpdatePropertyGrid(TransformEntity? e)
        {
            entities_ComboBox.SafeInvoke(delegate
            {
                entities_ComboBox.SelectedItem = e;
            });

            GameApplication.UnregisterDraw(GameApp_Draw);

            if (e != null)
                GameApplication.RegisterDraw(GameApp_Draw);

            PropertyGridInvoke(delegate
            {
                if (e == null) inspector_PropertyGrid.SelectedObject = null;
                else inspector_PropertyGrid.SelectedObject = new EntityWrapper(e);
            });
        }

        private void RefreshPropertyGrid()
        {
            if (!GameApplication.IsActive()) return;

            PropertyGridInvoke(delegate
            {
                inspector_PropertyGrid.Refresh();
            });
        }

        private void PropertyGridInvoke(Action action)
        {
            inspector_PropertyGrid.SafeInvoke(action);
        }

        private void RegisterEvents()
        {
            entities_ComboBox.SelectedIndexChanged += EntityComboBox_SelectedIndexChanged;
            inspector_PropertyGrid.Invalidated += Inspector_Invalidated;

            _editor.SelectingEntityChanged += Editor_SelectingEntityChanged;

            _editor.CurrentSceneChanged += Editor_CurrentSceneChanged;
            if (_editor.CurrentGameScene != null)
            {
                UpdateEntityComboBox(_editor.CurrentGameScene);
            }
        }

        EditorApplication _editor;
        #endregion
    }
}
