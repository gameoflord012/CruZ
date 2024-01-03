using CruZ.Utility;
using CurZ.Editor;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CruZ.Editor
{
    class LoggingView : IViewDrawCallback
    {
        public void DrawView(GameTime gameTime)
        {
            var bound = Core.Instance.GraphicsDevice.Viewport.Bounds;

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(bound.Width, bound.Height));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0));
            ImGui.Begin("##log-window", 
                ImGuiWindowFlags.NoMove | 
                ImGuiWindowFlags.NoTitleBar | 
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoInputs);

            foreach (var msg in Logging.GetMsgs())
            {
                ImGui.Text(msg);
            }
        }
    }
}