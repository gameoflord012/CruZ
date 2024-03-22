using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using CruZ.Common.GameSystem.Resource;
using CruZ.Common.Resource;
using CruZ.Common.UI;
using Microsoft.Xna.Framework;
using CruZ.Common.Utility;
using System.Drawing.Design;

namespace CruZ.Common.ECS
{
    /// <summary>
    /// Game component loaded from specify resource
    /// </summary>
    public partial class SpriteRendererComponent : RendererComponent, IHasBoundBox
    {
        public event EventHandler<DrawLoopBeginEventArgs>? DrawLoopBegin;
        public event EventHandler<DrawLoopEndEventArgs>? DrawLoopEnd;
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<UIBoundingBox> BoundingBoxChanged;

        #region Properties
        public bool SortByY { get; set; } = false;
        public bool Flip { get; set; }

        [JsonIgnore, Browsable(false)]
        public Texture2D? Texture { get => _texture; set => _texture = value; }

        [TypeConverter(typeof(Vector2TypeConverter))]
        public Vector2 NormalizedOrigin { get; set; } = new(0.5f, 0.5f);

        [Editor(typeof(FileUITypeEditor), typeof(UITypeEditor))]
        public string TexturePath
        {
            get => _spriteResInfo != null ? _spriteResInfo.ResourceName : "";
            set => LoadTexture(value);
        }
        #endregion

        public SpriteRendererComponent()
        {
            _resource = GameContext.GameResource;

            DrawBegin += () =>
            {
                _boundingBox.Points.Clear();
                _hasBoundingBox = false;
            };

            DrawLoopEnd += (sender, args) =>
            {
                _boundingBox.Points.Add(args.BeginArgs.GetWorldOrigin());

                if (!_hasBoundingBox)
                {
                    _boundingBox.Bound = args.BeginArgs.GetWorldBounds();
                    _hasBoundingBox = true;
                }
                else
                {
                    var bounds = args.BeginArgs.GetWorldBounds();

                    _boundingBox.Bound.X = MathF.Min(_boundingBox.Bound.X, bounds.X);
                    _boundingBox.Bound.Y = MathF.Min(_boundingBox.Bound.Y, bounds.Y);
                    _boundingBox.Bound.Width = _boundingBox.Bound.Right < bounds.Right ? bounds.Right - _boundingBox.Bound.X : _boundingBox.Bound.Width;
                    _boundingBox.Bound.Height = _boundingBox.Bound.Bottom < bounds.Bottom ? bounds.Bottom - _boundingBox.Bound.Y : _boundingBox.Bound.Height;
                }
            };

            DrawEnd += () => BoundingBoxChanged.Invoke(_hasBoundingBox ? _boundingBox : UIBoundingBox.Default);
        }

        public void LoadTexture(string texturePath)
        {
            if (!string.IsNullOrEmpty(texturePath))
            {
                _spriteResInfo = _resource.RetriveResourceInfo(texturePath);

                try
                {
                    Texture = _resource.Load<Texture2D>(_spriteResInfo);
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Failed to load texture with path \"{texturePath}\"", e);
                }
            }
        }

        public int CompareLayer(SpriteRendererComponent other)
        {
            return SortingLayer == other.SortingLayer ?
                CalculateLayerDepth().CompareTo(other.CalculateLayerDepth()) :
                SortingLayer.CompareTo(other.SortingLayer);
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            var fx = EffectManager.NormalSpriteRenderer;
            fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);

            DrawBegin?.Invoke();
            spriteBatch.Begin(
                effect: EffectManager.NormalSpriteRenderer,
                sortMode: SpriteSortMode.FrontToBack,
                samplerState: SamplerState.PointClamp);

            while (true)
            {
                DrawLoopBeginEventArgs beginLoop = new();
                beginLoop.Position = new(AttachedEntity.Transform.Position.X, AttachedEntity.Transform.Position.Y);
                beginLoop.LayerDepth = CalculateLayerDepth();
                beginLoop.NormalizedOrigin = NormalizedOrigin;
                beginLoop.Scale = new(AttachedEntity.Transform.Scale.X, AttachedEntity.Transform.Scale.Y);

                if (Texture != null)
                {
                    beginLoop.SourceRectangle = Texture.Bounds;
                    beginLoop.Texture = Texture;
                }

                DrawLoopBegin?.Invoke(this, beginLoop);

                if (beginLoop.Skip)
                {

                }
                else if (beginLoop.Texture == null)
                {

                }
                else
                {
                    spriteBatch.Draw(
                    texture: beginLoop.Texture,
                    position: beginLoop.Position,

                    sourceRectangle: beginLoop.SourceRectangle,

                    color: XNA.Color.White,
                    rotation: 0,

                    origin: new(beginLoop.NormalizedOrigin.X * beginLoop.SourceRectangle.Width,
                                beginLoop.NormalizedOrigin.Y * beginLoop.SourceRectangle.Height),

                    scale: beginLoop.Scale,

                    effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    layerDepth: beginLoop.LayerDepth);
                }

                var endLoop = new DrawLoopEndEventArgs();

                endLoop.BeginArgs = beginLoop;
                DrawLoopEnd?.Invoke(this, endLoop);
                if (!endLoop.KeepDrawing) break;
            }

            spriteBatch.End();
            DrawEnd?.Invoke();
        }

        private float CalculateLayerDepth()
        {
            return SortByY ? AttachedEntity.Transform.Position.Y / 2 : LayerDepth;
        }

        public override string ToString()
        {
            return _spriteResInfo != null ? _spriteResInfo.ResourceName : "<None>";
        }

        Texture2D? _texture;
        [JsonProperty]
        ResourceInfo? _spriteResInfo;
        ResourceManager _resource;
        UIBoundingBox _boundingBox = new();
        bool _hasBoundingBox;
    }
}
