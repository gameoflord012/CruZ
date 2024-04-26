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
    public partial class SpriteRendererComponent : RendererComponent, IHasBoundBox
    {
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<UIBoundingBox>? BoundingBoxChanged;

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
                _boundingBox.WorldOrigins.Clear();
                _boundingBox.WorldBounds = default;
            };

            DrawRequestsFetched += (fetchedDrawRequests) =>            
            {
                foreach (var request in fetchedDrawRequests)
                {
                    // add origin

                    if( // ignore if it is an invalid request
                        request.Texture == null ||
                        request.GetWorldBounds().Width == 0 && request.GetWorldBounds().Height == 0)
                        continue;

                    _boundingBox.WorldOrigins.Add(request.GetWorldOrigin());

                    if (_boundingBox.WorldBounds.IsEmpty)
                    {
                        _boundingBox.WorldBounds = request.GetWorldBounds(); // Assign if worldbounds is uninitialized
                    }
                    else
                    {
                        var bounds = request.GetWorldBounds();

                        //
                        // we expand the bounding box accroding to requests
                        //
                        _boundingBox.WorldBounds.X = MathF.Min(_boundingBox.WorldBounds.X, bounds.X);
                        _boundingBox.WorldBounds.Y = MathF.Min(_boundingBox.WorldBounds.Y, bounds.Y);
                        _boundingBox.WorldBounds.Width = _boundingBox.WorldBounds.Right < bounds.Right ? bounds.Right - _boundingBox.WorldBounds.X : _boundingBox.WorldBounds.Width;
                        _boundingBox.WorldBounds.Height = _boundingBox.WorldBounds.Bottom < bounds.Bottom ? bounds.Bottom - _boundingBox.WorldBounds.Y : _boundingBox.WorldBounds.Height;
                    }
                }
            };

            DrawEnd += () => BoundingBoxChanged?.Invoke(_boundingBox);
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
                e.SpriteBatch.Draw(request);
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
        UIBoundingBox _boundingBox = new();
    }
}
