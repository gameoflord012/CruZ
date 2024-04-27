using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Drawing.Design;
using System.Text.Json.Serialization;
using CruZ.GameEngine.Resource;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Utility;
using System.Collections.Generic;
using System.Collections.Immutable;

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

        #region Properties
        public bool SortByY { get; set; } = false;
        public bool FlipHorizontally { get; set; }

        [JsonIgnore, Browsable(false)]
        public Texture2D? Texture { get => _texture; set => _texture = value; }
        public Vector2 NormalizedOrigin { get; set; } = new(0.5f, 0.5f);

        //[Editor(typeof(FileUITypeEditor), typeof(UITypeEditor))]
        //public string TexturePath
        //{
        //    get => _spriteResInfo != null ? _spriteResInfo.ResourceName : "";
        //    set => LoadTexture(value);
        //}
        #endregion

        public SpriteRendererComponent()
        {
            _resource = GameContext.GameResource;
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
                foreach (var request in fetchedDrawRequests)
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

        public void LoadTexture(string texturePath)
        {
            Texture = _resource.Load<Texture2D>(texturePath);
        }

        public int CompareLayer(SpriteRendererComponent other)
        {
            return SortingLayer == other.SortingLayer ?
                CalculateLayerDepth().CompareTo(other.CalculateLayerDepth()) :
                SortingLayer.CompareTo(other.SortingLayer);
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

            List<DrawArgs> drawRequest = [];
            FetchDrawRequests(drawRequest, e.ViewProjectionMatrix);

            foreach (var request in drawRequest)
            {
                e.SpriteBatch.DrawWorld(request);
            }

            e.SpriteBatch.End();
            DrawEnd?.Invoke();
        }

        /// <summary>
        /// default draw request and list of draw request
        /// </summary>
        public event Action<FetchingDrawRequestsEventArgs>? DrawRequestsFetching;
        public event Action<IImmutableList<DrawArgs>>? DrawRequestsFetched;

        private void FetchDrawRequests(List<DrawArgs> drawRequests, Matrix viewProjectionMat)
        {
            DrawArgs defaultArgs = new();

            if (Texture != null) defaultArgs.Apply(Texture);
            defaultArgs.Apply(AttachedEntity);

            defaultArgs.LayerDepth = CalculateLayerDepth();
            defaultArgs.NormalizedOrigin = NormalizedOrigin;
            defaultArgs.Color = Color.White;
            defaultArgs.SpriteEffect = FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            DrawRequestsFetching?.Invoke(new FetchingDrawRequestsEventArgs(defaultArgs, drawRequests, viewProjectionMat));
            DrawRequestsFetched?.Invoke(drawRequests.ToImmutableList());
        }

        private float CalculateLayerDepth()
        {
            return SortByY ? AttachedEntity.Transform.Position.Y / 2 : LayerDepth;
        }

        Texture2D? _texture;
        //[JsonInclude]
        //ResourceInfo? _spriteResInfo;
        ResourceManager _resource;
        RectUIInfo _worldBound;
    }
}
