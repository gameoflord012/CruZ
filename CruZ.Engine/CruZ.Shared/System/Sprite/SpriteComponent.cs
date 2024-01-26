using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    using Box2D.NetStandard.Dynamics.World;
    using Microsoft.Xna.Framework;
    using MonoGame.Extended;
    using System.ComponentModel;
    using System.IO;

    public class DrawBeginEventArgs : EventArgs
    {
        public Rectangle    SourceRectangle;
        public Vector2      Origin;
        public Vector2      Position;
        public Texture2D?   Texture;
        public Matrix       ViewMatrix;
        public float        LayerDepth = 0;
        public bool         Skip = false;
    }

    public class DrawEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
    }

    public partial class SpriteComponent : IComponent, IComponentCallback
    {
        public event EventHandler<DrawBeginEventArgs>   OnDrawBegin;
        public event EventHandler<DrawEndEventArgs>     OnDrawEnd;

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

            while (true)
            {
                DrawBeginEventArgs beginArgs = new();
                beginArgs.Position          = new Vector2(_e.Transform.Position.X, _e.Transform.Position.Y);
                beginArgs.ViewMatrix        = viewMatrix;
                beginArgs.LayerDepth        = YLayerDepth ? (beginArgs.Position.Y / 1000 + 1) / 2 : LayerDepth;
                beginArgs.Origin            = new(0.5f, 0.5f);
                
                if(Texture != null)
                {
                    beginArgs.SourceRectangle   = Texture.Bounds;
                    beginArgs.Texture           = Texture;
                }
                
                OnDrawBegin?.Invoke(this, beginArgs);

                if (beginArgs.Skip)
                {

                }
                else if(beginArgs.Texture == null)
                {
                    
                }
                else
                {
                    spriteBatch.Draw(
                    texture:            beginArgs.Texture,
                    position:           beginArgs.Position,

                    sourceRectangle:    beginArgs.SourceRectangle,

                    color: XNA.Color.White,
                    rotation: 0,

                    origin:             new(beginArgs.Origin.X * beginArgs.SourceRectangle.Width, 
                                            beginArgs.Origin.Y * beginArgs.SourceRectangle.Height),

                    scale: new Vector2(
                        _e.Transform.Scale.X,
                        _e.Transform.Scale.Y),

                    effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    layerDepth:         beginArgs.LayerDepth);
                }

                var endArgs = new DrawEndEventArgs();
                OnDrawEnd?.Invoke(this, endArgs);
                if (!endArgs.KeepDrawing) break;
            }
        }

        public void OnAttached(TransformEntity entity)
        {
            _e = entity;
        }

        private Texture2D?          _texture;
        private TransformEntity?    _e;

        [JsonProperty]
        private ResourceInfo?       _spriteResInfo;
    }
}
