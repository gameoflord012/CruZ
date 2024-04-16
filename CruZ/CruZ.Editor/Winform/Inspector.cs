﻿using CruZ.Editor.Controls;
using CruZ.Editor.Service;
using CruZ.Editor.Winform.Ultility;
using CruZ.Editor.Winform.Utility;
using CruZ.Framework;
using CruZ.Framework.GameSystem;
using CruZ.Framework.GameSystem.Scene;

using System;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class Inspector : UserControl
    {
        public Inspector()
        {
            InitializeComponent();

            inspector_PropertyGrid.PropertySort = PropertySort.Categorized;
            entities_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            InvalidateService.Register
                (inspector_PropertyGrid,
                InvalidatedEvents.EntityNameChanged,
                InvalidatedEvents.EntityComponentChanged,
                InvalidatedEvents.SelectingEntityChanged);

            entities_ComboBox.SelectedIndexChanged += Entities_ComboBox_SelectedIndexChanged;
            inspector_PropertyGrid.Invalidated += Inspector_Invalidated;
        }

        #region Event_Handlers
        private void Editor_CurrentSceneChanged(GameScene? scene)
        {
            UpdateEntityComboBox(scene);
        }

        private void Editor_SelectingEntityChanged(TransformEntity? e)
        {
            UpdatePropertyGrid(e);
        }

        private void Entities_ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Editor.SelectedEntity = (TransformEntity)entities_ComboBox.SelectedItem;
        }

        private void Inspector_Invalidated(object? sender, InvalidateEventArgs e)
        {
            var wrapper = (EntityWrapper)inspector_PropertyGrid.SelectedObject;
            wrapper?.RefreshComponents();
            //RefreshPropertyGrid();
        }

        private void GameApp_Drawing()
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

                foreach (var e in scene.Entities)
                {
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

            GameApplication.AfterDrawn -= GameApp_Drawing;

            if (e != null)
                GameApplication.AfterDrawn += GameApp_Drawing;

            PropertyGridInvoke(delegate
            {
                if (e == null) inspector_PropertyGrid.SelectedObject = null;
                else inspector_PropertyGrid.SelectedObject = new EntityWrapper(e);
            });
        }

        private void RefreshPropertyGrid()
        {
            PropertyGridInvoke(delegate
            {
                var focusedControl = ControlHelper.FindFocusedControl(inspector_PropertyGrid);
                bool isEditingPropertyGrid = focusedControl != null && focusedControl.GetType().ToString() != "GridViewEdit";
                if (!isEditingPropertyGrid) inspector_PropertyGrid.Refresh();
            });
        }

        private void PropertyGridInvoke(Action action)
        {
            inspector_PropertyGrid.SafeInvoke(action);
        }

        public GameEditor Editor 
        { 
            get => _editor ?? throw new NullReferenceException(); 
            set 
            {
                if(_editor == value) return;

                if(_editor != null)
                {
                    _editor.SelectingEntityChanged -= Editor_SelectingEntityChanged;
                    _editor.CurrentSceneChanged -= Editor_CurrentSceneChanged;
                }
                
                _editor = value;

                _editor.SelectingEntityChanged += Editor_SelectingEntityChanged;
                _editor.CurrentSceneChanged += Editor_CurrentSceneChanged;
            }
        }

        GameEditor? _editor;
        #endregion
    }
}
