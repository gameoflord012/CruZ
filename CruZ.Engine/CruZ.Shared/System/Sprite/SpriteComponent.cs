using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    using Box2D.NetStandard.Dynamics.World;
    using CruZ.Global;
    using Microsoft.Xna.Framework;
    using MonoGame.Extended;
    using System.ComponentModel;
    using System.IO;

    public class DrawBeginEventArgs : EventArgs
    {
        public Rectangle    SourceRectangle;
        public Vector2      Origin;
        public Vector2      Position;
        public Vector2      Scale;
        public Texture2D?   Texture;
        public Matrix       ViewMatrix;
        public float        LayerDepth = 0;
        public bool         Skip = false;

        public RectangleF BoundRect()
        {
            RectangleF rect = new();
            rect.Width = SourceRectangle.Width * Scale.X;
            rect.Height = SourceRectangle.Height * Scale.Y;
            rect.Position = new Vector2(Position.X - rect.Width * Origin.X, Position.Y - rect.Height * Origin.Y);
            return rect;
        }
    }

    public class DrawEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
    }

    public partial class SpriteComponent : IComponent, IComponentCallback
    {
        public event EventHandler<DrawBeginEventArgs>   DrawLoopBegin;
        public event EventHandler<DrawEndEventArgs>     DrawLoopEnd;
        public event Action                             DrawBegin;
        public event Action                             DrawEnd;

        [Browsable(false), JsonIgnore]
        public Type             ComponentType => typeof(SpriteComponent);

        [JsonIgnore, Browsable(false)]
        public Texture2D?       Texture         { get => _texture; set => _texture = value; }
        public float            LayerDepth      { get; set; } = 0;
        public int              SortingLayer    { get; set; } = 0;
        public bool             YLayerDepth     { get; set; } = false;
        public bool             Flip            { get; set; }

        public SpriteComponent() { }
        public SpriteComponent(string resourceName) { LoadTexture(resourceName); }

        public void LoadTexture(string resourcePath)
        {
            if (!string.IsNullOrEmpty(resourcePath))
            {
                Texture = ResourceManager.LoadResource<Texture2D>(resourcePath, out _spriteResInfo);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            Trace.Assert(_e != null);

            DrawBegin?.Invoke();
            while (true)
            {
                DrawBeginEventArgs beginArgs = new();
                beginArgs.Position = new Vector2(_e.Transform.Position.X, _e.Transform.Position.Y);
                beginArgs.ViewMatrix = viewMatrix;
                beginArgs.LayerDepth = CalculateLayerDepth();
                beginArgs.Origin = new(0.5f, 0.5f);
                beginArgs.Scale = new Vector2(_e.Transform.Scale.X, _e.Transform.Scale.Y);

                if (Texture != null)
                {
                    beginArgs.SourceRectangle = Texture.Bounds;
                    beginArgs.Texture = Texture;
                }

                DrawLoopBegin?.Invoke(this, beginArgs);

                if (beginArgs.Skip)
                {

                }
                else if (beginArgs.Texture == null)
                {

                }
                else
                {
                    spriteBatch.Draw(
                    texture: beginArgs.Texture,
                    position: beginArgs.Position,

                    sourceRectangle: beginArgs.SourceRectangle,

                    color: XNA.Color.White,
                    rotation: 0,

                    origin: new(beginArgs.Origin.X * beginArgs.SourceRectangle.Width,
                                            beginArgs.Origin.Y * beginArgs.SourceRectangle.Height),

                    scale: beginArgs.Scale,

                    effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    layerDepth: beginArgs.LayerDepth);
                }

                var endArgs = new DrawEndEventArgs();
                DrawLoopEnd?.Invoke(this, endArgs);
                if (!endArgs.KeepDrawing) break;
            }

            DrawEnd?.Invoke();
        }

        public void OnAttached(TransformEntity entity)
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
            return YLayerDepth ? (_e.Transform.Position.Y / Variables.MAX_WORLD_DISTANCE + 1) / 2 : LayerDepth;
        }

        private Texture2D?          _texture;
        private TransformEntity?    _e;

        [JsonProperty]
        private ResourceInfo?       _spriteResInfo;
    }
}
