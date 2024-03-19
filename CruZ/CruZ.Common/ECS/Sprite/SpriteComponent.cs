using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.ComponentModel;
using CruZ.Common.GameSystem.Resource;
using CruZ.Common.Resource;
using CruZ.Common.UI;
using Microsoft.Xna.Framework;



#if CRUZ_EDITOR
using CruZ.Editor.Utility;
using CruZ.Editor.Winform.Ultility;
using System.Drawing.Design;
#endif

namespace CruZ.Common.ECS
{
    /// <summary>
    /// Game component loaded from specify resource
    /// </summary>
    public partial class SpriteComponent : Component, IHasBoundBox
    {
        public event EventHandler<DrawLoopBeginEventArgs>? DrawLoopBegin;
        public event EventHandler<DrawLoopEndEventArgs>? DrawLoopEnd;
        public event Action? DrawBegin;
        public event Action? DrawEnd;
        public event Action<UIBoundingBox> BoundingBoxChanged;

        #region Properties
        public float LayerDepth { get; set; } = 0;
        public int SortingLayer { get; set; } = 0;
        public bool SortByY { get; set; } = false;
        public bool Flip { get; set; }

        [Browsable(false), JsonIgnore]
        public override Type ComponentType => typeof(SpriteComponent);

        [JsonIgnore, Browsable(false)]
        public Texture2D? Texture { get => _texture; set => _texture = value; }

#if CRUZ_EDITOR
        [TypeConverter(typeof(Vector2TypeConverter))]
#endif
        public Vector2 NormalizedOrigin { get; set; } = new(0.5f, 0.5f);

#if CRUZ_EDITOR
        [Editor(typeof(FileUITypeEditor), typeof(UITypeEditor))]
#endif
        public string TexturePath
        {
            get => _spriteResInfo != null ? _spriteResInfo.ResourceName : "";
            set => LoadTexture(value);
        }
        #endregion

        public SpriteComponent()
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
                catch(Exception e)
                {
                    throw new ArgumentException($"Failed to load texture with path \"{texturePath}\"", e);
                }
            }
        }

        public int CompareLayer(SpriteComponent other)
        {
            return SortingLayer == other.SortingLayer ?
                CalculateLayerDepth().CompareTo(other.CalculateLayerDepth()) :
                SortingLayer.CompareTo(other.SortingLayer);
        }
        
        internal virtual void InternalDraw(SpriteBatch spriteBatch)
        {
            DrawBegin?.Invoke();

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
        UI.UIBoundingBox _boundingBox = new();
        bool _hasBoundingBox;
    }
}
