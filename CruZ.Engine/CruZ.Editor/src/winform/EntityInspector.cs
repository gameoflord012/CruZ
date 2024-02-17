using Assimp;
using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Services;
using CruZ.Editor.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor.src.winform
{
    public partial class EntityInspector : UserControl
    {
        public EntityInspector()
        {
            InitializeComponent();
        }

        public void Init(EditorApplication editor)
        {
            _editorApp = editor;

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

        private void PropertyGrid_PropertyValueChanged(object? s, PropertyValueChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EntityList_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _editorApp.SelectEntity((TransformEntity)entities_ComboBox.SelectedItem);
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
            PropertyGridInvoke(delegate
            {
                if (inspector_PropertyGrid.ContainsFocus) return;
                inspector_PropertyGrid.Refresh();
            });
        }

        private void PropertyGridInvoke(Action action)
        {
            inspector_PropertyGrid.SafeInvoke(action);
        }

        private void RegisterEvents()
        {
            entities_ComboBox.SelectedIndexChanged += EntityList_SelectedIndexChanged;
            inspector_PropertyGrid.Invalidated += Inspector_Invalidated;

            _editorApp.SelectingEntityChanged += Editor_SelectingEntityChanged;

            _editorApp.CurrentSceneChanged += Editor_CurrentSceneChanged;
            if (_editorApp.CurrentGameScene != null)
            {
                UpdateEntityComboBox(_editorApp.CurrentGameScene);
            }

            inspector_PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
        }

        EditorApplication _editorApp; 
        #endregion
    }
}
