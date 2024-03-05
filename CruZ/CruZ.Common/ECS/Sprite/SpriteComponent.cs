using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using CruZ.Common.GameSystem.Resource;
using CruZ.Common.Resource;



#if CRUZ_EDITOR
using CruZ.Editor.Utility;
using CruZ.Editor.Winform.Ultility;
using System.Drawing.Design;
#endif

namespace CruZ.Common.ECS
{
    #region EventArgs
    public class DrawLoopBeginEventArgs : EventArgs
    {
        public Rectangle SourceRectangle;
        public NUM.Vector2 Origin;
        public NUM.Vector2 Position;
        public NUM.Vector2 Scale;
        public Texture2D? Texture;
        public Matrix ViewMatrix;
        public float LayerDepth = 0;
        public bool Skip = false;

        public DRAW.RectangleF GetWorldBounds() // in World Coordinate
        {
            DRAW.RectangleF rect = new();
            rect.Width = SourceRectangle.Width * Scale.X;
            rect.Height = SourceRectangle.Height * Scale.Y;
            rect.Location = new(
                Position.X - rect.Width * Origin.X,
                Position.Y - rect.Height * Origin.Y);

            return rect;
        }

        public DataType.Vector3 GetWorldOrigin()
        {
            var worldBounds = GetWorldBounds();
            return new(
                worldBounds.X + worldBounds.Width * Origin.X,
                worldBounds.Y + worldBounds.Height * Origin.Y,
                0
            );
        }
    }

    public class DrawLoopEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
        public DrawLoopBeginEventArgs BeginArgs;
    }

    public class DrawEndEventArgs : EventArgs
    {
        public DRAW.RectangleF RenderBounds;
        /// <summary>
        /// To mark the RenderBounds property is valid to use in <see cref="DrawEndEventArgs"/>
        /// </summary>
        public bool HasRenderBounds = false;
    }
    #endregion

    /// <summary>
    /// Game component loaded from specify resource
    /// </summary>
    public partial class SpriteComponent : Component
    {
        public event EventHandler<DrawLoopBeginEventArgs>? DrawLoopBegin;
        public event EventHandler<DrawLoopEndEventArgs>? DrawLoopEnd;
        public event Action? DrawBegin;
        public event Action<DrawEndEventArgs>? DrawEnd;

        #region Properties
        public float LayerDepth { get; set; } = 0;
        public int SortingLayer { get; set; } = 0;
        public bool YLayerDepth { get; set; } = false;
        public bool Flip { get; set; }

        [Browsable(false), JsonIgnore]
        public override Type ComponentType => typeof(SpriteComponent);

        [JsonIgnore, Browsable(false)]
        public Texture2D? Texture { get => _texture; set => _texture = value; }

#if CRUZ_EDITOR
        [TypeConverter(typeof(Vector2TypeConverter))]
#endif
        public NUM.Vector2 Origin { get; set; } = new(0.5f, 0.5f);

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
                catch(global::System.Exception e)
                {
                    throw new ArgumentException($"Failed to load texture with path \"{texturePath}\"", e);
                }
            }
        }

        internal virtual void InternalDraw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            Trace.Assert(_e != null);

            DrawBegin?.Invoke();

            DrawEndEventArgs drawEnd = new();

            while (true)
            {
                DrawLoopBeginEventArgs beginLoop = new();
                beginLoop.Position = new(_e.Transform.Position.X, _e.Transform.Position.Y);
                beginLoop.ViewMatrix = viewMatrix;
                beginLoop.LayerDepth = CalculateLayerDepth();
                beginLoop.Origin = Origin;
                beginLoop.Scale = new(_e.Transform.Scale.X, _e.Transform.Scale.Y);

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

                    origin: new(beginLoop.Origin.X * beginLoop.SourceRectangle.Width,
                                beginLoop.Origin.Y * beginLoop.SourceRectangle.Height),

                    scale: beginLoop.Scale,

                    effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    layerDepth: beginLoop.LayerDepth);

                    if (!drawEnd.HasRenderBounds)
                    {
                        drawEnd.RenderBounds = beginLoop.GetWorldBounds();
                        drawEnd.HasRenderBounds = true;
                    }
                    else
                    {
                        var bounds = beginLoop.GetWorldBounds();

                        drawEnd.RenderBounds.X = MathF.Min(drawEnd.RenderBounds.X, bounds.X);
                        drawEnd.RenderBounds.Y = MathF.Min(drawEnd.RenderBounds.Y, bounds.Y);
                        drawEnd.RenderBounds.Width = drawEnd.RenderBounds.Right < bounds.Right ? bounds.Right - drawEnd.RenderBounds.X : drawEnd.RenderBounds.Width;
                        drawEnd.RenderBounds.Height = drawEnd.RenderBounds.Bottom < bounds.Bottom ? bounds.Bottom - drawEnd.RenderBounds.Y : drawEnd.RenderBounds.Height;
                    }
                }

                var endLoop = new DrawLoopEndEventArgs();

                endLoop.BeginArgs = beginLoop;
                DrawLoopEnd?.Invoke(this, endLoop);
                if (!endLoop.KeepDrawing) break;
            }

            DrawEnd?.Invoke(drawEnd);
        }

        protected override void OnAttached(TransformEntity entity)
        {
            _e = entity;
        }

        public int CompareLayer(SpriteComponent other)
        {
            return SortingLayer == other.SortingLayer ?
                CalculateLayerDepth().CompareTo(other.CalculateLayerDepth()) :
                SortingLayer.CompareTo(other.SortingLayer);
        }

        private float CalculateLayerDepth()
        {
            return YLayerDepth ? (_e.Transform.Position.Y / GameConstants.MAX_WORLD_DISTANCE + 1) / 2 : LayerDepth;
        }

        Texture2D? _texture;
        TransformEntity? _e;
        [JsonProperty]
        ResourceInfo? _spriteResInfo;
        ResourceManager _resource;

    }
}
