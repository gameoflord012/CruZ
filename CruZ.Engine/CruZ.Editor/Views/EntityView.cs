using CruZ.Components;
using CurZ.Editor;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Numerics;

namespace CruZ.Editor
{
    public class EntityView : IViewDrawCallback
    {
        public static readonly string ViewLabel = "Entity View##entity-view";

        public void DrawView(GameTime gameTime)
        {
            #region RIP
            //ImGui.Begin("##rect-view");
            //var drawList = ImGui.GetWindowDrawList();

            //var rectCoord = _binding.GetRectCoord();

            //var pMin = MGWrapper.CoordinateToPoint(rectCoord.TopLeft);
            //var pMax = MGWrapper.CoordinateToPoint(rectCoord.BottomRight);

            //var center = MGWrapper.CoordinateToPoint(rectCoord.Center);
            //var col = ImGui.ColorConvertFloat4ToU32(new Vector4(1, 0, 0, 1));

            //drawList.AddRect(new(pMin.X, pMin.Y), new(pMax.X, pMax.Y), col);
            //drawList.AddCircle(new(center.X, center.Y), 3, col);

            //drawList.AddText(
            //    new(center.X, center.Y), 
            //    col, 
            //    string.Format("({0}, {1})", 
            //        rectCoord.Center.X, rectCoord.Center.Y));

            //ImGui.End();
            #endregion

            ImGui.Begin(ViewLabel);

            DisplayCursorPosition();
            if(IsSelected) DrawDragging();
            DrawSelectButton();

            ImGui.End();
        }

        private void DrawSelectButton()
        {
            ImGui.SetCursorPos(
                GetEntityScreenPoint() + new System.Numerics.Vector2(-10, 10));

            ImGui.PushStyleColor(
                ImGuiCol.Text, 
                new System.Numerics.Vector4(1, 0, 0, 1));

            if(ImGui.RadioButton("select me", IsSelected))
            {
                CurrentSelected = IsSelected ? null :_binding;
            }

            ImGui.PopStyleColor();
        }

        private void DrawDragging()
        {
            ImGui_DragThumb();
            if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(0))
            {
                _isDragging = true;
            }
            else if (!ImGui.IsMouseDown(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                DoDragging();
            }
        }

        private void ImGui_DragThumb()
        {
            ImGui.SetCursorScreenPos(
                GetEntityScreenPoint() - new System.Numerics.Vector2(10, 10)
            );

            ImGui.RadioButton("##thumb", false);
        }

        private System.Numerics.Vector2 GetEntityScreenPoint()
        {
            return Camera.Main.CoordinateToPoint(_binding.Transform.Position);
        }

        private void DoDragging()
        {
            var point = ImGui.GetMousePos();
            var coord = Camera.Main.PointToCoordinate(point);
            _binding.Transform.Position = coord;
        }

        private void DisplayCursorPosition()
        {
            var text = ImGui.GetMousePos().ToString();
            ImGui.SetCursorPos(new(2,
                ImGui.GetWindowSize().Y - ImGui.CalcTextSize(text).Y));
            ImGui.Text(text);
        }

        public static void InitializeWindow()
        {
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable; // new

            ImGui.Begin("Entity View##entity-view",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoBringToFrontOnFocus);

            ImGui.SetWindowPos(new(0, 0));
            ImGui.SetWindowSize(ImGui.GetMainViewport().Size);

            ImGui.End();
        }

        [JsonIgnore]
        private TransformEntity _binding;
        [JsonIgnore]
        public TransformEntity Binding { get => _binding; set => _binding = value; }

        bool _isDragging = false;
        bool IsSelected => CurrentSelected == _binding;

        public static TransformEntity? CurrentSelected;
    }
}