using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CruZ.Editor
{
    public class CameraView : IViewDrawCallback
    {
        public void DrawView(GameTime gameTime)
        {
            var io = ImGui.GetIO();
            io.WantCaptureMouse = true;

            ImGui.Begin(EntityView.ViewLabel);

            if(ImGui.IsMouseDragging(ImGuiMouseButton.Right))
            {
                var delt = ImGui.GetMouseDragDelta(ImGuiMouseButton.Right);

                Camera.Main.Position += 
                    Vector2.Normalize(delt) * 
                    1000 *
                    (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            ImGui.End();
        }
    }
}