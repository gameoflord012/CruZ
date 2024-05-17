using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem
{
    internal class AutoResizeRenderTarget : IDisposable
    {
        public AutoResizeRenderTarget(GraphicsDevice gd, GameWindow window)
        {
            _gd = gd;
            _window = window;
            _window.ClientSizeChanged += Window_ClientSizeChanged;

            UpdateResolution();
        }

        private void Window_ClientSizeChanged(object? sender, EventArgs e)
        {
            UpdateResolution();
        }

        private void UpdateResolution()
        {
            _value?.Dispose();
            _value = new RenderTarget2D(_gd, _window.ClientBounds.Width, _window.ClientBounds.Height);
        }

        public void Dispose()
        {
            _window.ClientSizeChanged -= Window_ClientSizeChanged;
            _value?.Dispose();
        }

        GameWindow _window;
        GraphicsDevice _gd;
        RenderTarget2D? _value;

        public RenderTarget2D Value
        {
            get => _value!;
        }
    }
}
