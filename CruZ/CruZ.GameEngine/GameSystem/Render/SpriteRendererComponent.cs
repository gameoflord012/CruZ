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
    public partial class SpriteRendererComponent : RendererComponent, IUIRectProvider
    {
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<UIRect>? UIRectChanged;

        public SpriteRendererComponent()
        {
            _resource = GameApplication.Resource;
            InitBoundingBoxEventHandlers();
        }

        private void InitBoundingBoxEventHandlers()
        {
            DrawBegin += () =>
            {
                uiRect = new();
            };

            DrawRequestsFetched += (fetchedDrawRequests) =>
            {
                foreach (var request in fetchedDrawRequests
                    .Where(e => e is SpriteDrawRequest)
                    .Select(e => ((SpriteDrawRequest)e).SpriteDrawArgs))
                {
                    if ( // ignore if it is an invalid request
                        request.Texture == null ||
                        request.GetWorldBound().W == 0 && request.GetWorldBound().H == 0)
                        continue;

                    uiRect.WorldOrigins.Add(request.GetWorldOrigin());

                    if (!uiRect.WorldBound.HasValue)
                    {
                        uiRect.WorldBound = request.GetWorldBound(); // Assign if worldbounds is uninitialized
                    }
                    else
                    {
                        var joinBound = request.GetWorldBound();
                        var currentBound = uiRect.WorldBound.Value;

                        // we expand the world joinBound to cover the request draws joinBound
                        WorldRectangle newBound = uiRect.WorldBound.Value;
                        {
                            newBound.X = MathF.Min(currentBound.X, joinBound.X);
                            newBound.Y = MathF.Min(currentBound.Y, joinBound.Y);
                            newBound.W = MathF.Max(currentBound.Right, joinBound.Right) - newBound.X;
                            newBound.H = MathF.Max(currentBound.Top, joinBound.Top) - newBound.Y;
                        }

                        uiRect.WorldBound = newBound;
                    }
                }
            };

            DrawEnd += () => UIRectChanged?.Invoke(uiRect);
        }

        public override void Render(RenderSystemEventArgs e)
        {
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(e.ViewProjectionMatrix);

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

        UIRect uiRect;
    }
}
