using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework.Graphics;

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
        public event Action<List<DrawRequestBase>>? DrawRequestsFetching;
        public event Action<IImmutableList<DrawRequestBase>>? DrawRequestsFetched;

        public SpriteRendererComponent()
        {
            InitBoundingBoxEventHandlers();
        }

        private void InitBoundingBoxEventHandlers()
        {
            DrawBegin += () =>
            {
                _uiRect = new();
            };

            DrawRequestsFetched += (fetchedDrawRequests) =>
            {
                foreach(var request in fetchedDrawRequests
                    .Where(e => e is SpriteDrawRequest)
                    .Select(e => ((SpriteDrawRequest)e).SpriteDrawArgs))
                {
                    if( // ignore if it is an invalid request
                        request.Texture == null ||
                        request.GetWorldBound().W == 0 && request.GetWorldBound().H == 0)
                        continue;

                    _uiRect.WorldOrigins.Add(request.GetWorldOrigin());

                    if(!_uiRect.WorldBound.HasValue)
                    {
                        _uiRect.WorldBound = request.GetWorldBound(); // Assign if worldbounds is uninitialized
                    }
                    else
                    {
                        var joinBound = request.GetWorldBound();
                        var currentBound = _uiRect.WorldBound.Value;

                        // we expand the world joinBound to cover the request draws joinBound
                        WorldRectangle newBound = _uiRect.WorldBound.Value;
                        {
                            newBound.X = MathF.Min(currentBound.X, joinBound.X);
                            newBound.Y = MathF.Min(currentBound.Y, joinBound.Y);
                            newBound.W = MathF.Max(currentBound.Right, joinBound.Right) - newBound.X;
                            newBound.H = MathF.Max(currentBound.Top, joinBound.Top) - newBound.Y;
                        }

                        _uiRect.WorldBound = newBound;
                    }
                }
            };

            DrawEnd += () => UIRectChanged?.Invoke(_uiRect);
        }

        public override void Render(RenderSystemEventArgs e)
        {
            DrawBegin?.Invoke();
            {
                e.SpriteBatch.Begin(effect: GetSetupEffect(e), sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
                {
                    DrawRequests(e.SpriteBatch);
                }
                e.SpriteBatch.End();
            }
            DrawEnd?.Invoke();
        }

        private void DrawRequests(SpriteBatch spriteBatch)
        {
            List<DrawRequestBase> drawRequest = [];
            FetchDrawRequests(drawRequest);

            foreach(var request in drawRequest)
            {
                request.DrawRequest(spriteBatch);
            }
        }

        protected virtual Effect GetSetupEffect(RenderSystemEventArgs args)
        {
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(args.ViewProjectionMatrix);
            return fx;
        }

        private void FetchDrawRequests(List<DrawRequestBase> drawRequests)
        {
            DrawRequestsFetching?.Invoke(drawRequests);
            DrawRequestsFetched?.Invoke(drawRequests.ToImmutableList());
        }

        private UIRect _uiRect;
    }
}
