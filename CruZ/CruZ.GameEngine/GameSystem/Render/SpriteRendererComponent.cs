using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using CruZ.GameEngine.Resource;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Utility;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CruZ.GameEngine.GameSystem.ECS
{
    /// <summary>
    /// Game component loaded from specify resource
    /// </summary>
    public partial class SpriteRendererComponent : RendererComponent, IRectUIProvider
    {
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<RectUIInfo>? UIRectChanged;

        public SpriteRendererComponent()
        {
            _resource = GameApplication.Resource;
            InitBoundingBoxEventHandlers();
        }

        private void InitBoundingBoxEventHandlers()
        {
            DrawBegin += () =>
            {
                _worldBound = new();
            };

            DrawRequestsFetched += (fetchedDrawRequests) =>
            {
                foreach (var request in fetchedDrawRequests
                    .Where(e => e is SpriteDrawRequest)
                    .Select(e => ((SpriteDrawRequest)e).SpriteDrawArgs))
                {
                    // add origin

                    if ( // ignore if it is an invalid request
                        request.Texture == null ||
                        request.GetWorldBounds().W == 0 && request.GetWorldBounds().H == 0)
                        continue;

                    _worldBound.WorldOrigins.Add(request.GetWorldOrigin());

                    if (_worldBound.WorldBound == null)
                    {
                        _worldBound.WorldBound = request.GetWorldBounds(); // Assign if worldbounds is uninitialized
                    }
                    else
                    {
                        var bounds = request.GetWorldBounds();

                        //
                        // we expand the world bound to cover the request's world bound
                        //
                        WorldRectangle updatingBound = _worldBound.WorldBound.Value;
                        {
                            updatingBound.X = MathF.Min(updatingBound.X, bounds.X);
                            updatingBound.Y = MathF.Min(updatingBound.Y, bounds.Y);
                            updatingBound.Top = MathF.Max(updatingBound.Top, bounds.Top);
                            updatingBound.Right = MathF.Max(updatingBound.Right, bounds.Right);
                        }
                        _worldBound.WorldBound = updatingBound;
                    }
                }
            };

            DrawEnd += () => UIRectChanged?.Invoke(_worldBound);
        }

        public override void Render(RenderSystemEventArgs e)
        {
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);
            fx.Parameters["hdrColor"].SetValue(new Vector4(1, 1, 1, 1));

            DrawBegin?.Invoke();
            e.SpriteBatch.Begin(
                effect: fx,
                sortMode: SpriteSortMode.FrontToBack,
                samplerState: SamplerState.PointClamp);

            List<DrawRequestBase> drawRequest = [];
            FetchDrawRequests(drawRequest);

            foreach (var request in drawRequest)
            {
                request.DoRequest(e.SpriteBatch);
            }

            e.SpriteBatch.End();
            DrawEnd?.Invoke();
        }

        public event Action<List<DrawRequestBase>>? DrawRequestsFetching;
        public event Action<IImmutableList<DrawRequestBase>>? DrawRequestsFetched;

        private void FetchDrawRequests(List<DrawRequestBase> drawRequests)
        {
            DrawRequestsFetching?.Invoke(drawRequests);
            DrawRequestsFetched?.Invoke(drawRequests.ToImmutableList());
        }

        Texture2D? _texture;
        ResourceManager _resource;

        RectUIInfo _worldBound;
    }
}
