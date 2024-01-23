using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    using Microsoft.Xna.Framework;

    public class DrawBeginEventArgs : EventArgs
    {
        public Vector2 Position;
        public Rectangle SourceRectangle;
        public Vector2 Origin;
        public Texture2D Texture;
    }

    public class DrawEndEventArgs : EventArgs
    {
        public bool KeepDrawing = false;
    }

    public partial class SpriteComponent : IComponent, IComponentCallback
    {
        public event EventHandler<DrawBeginEventArgs> OnDrawBegin;
        public event EventHandler<DrawEndEventArgs> OnDrawEnd;

        public Type ComponentType => typeof(SpriteComponent);

        [JsonIgnore]
        public Texture2D? Texture { get => _texture; set => _texture = value; }
        public float LayerDepth { get; set; } = 0;
        public bool Flip;

        public SpriteComponent() { }
        public SpriteComponent(string resourceName) { LoadTexture(resourceName); }

        public void LoadTexture(string resourcePath)
        {
            _textureURI = resourcePath;

            if (!string.IsNullOrEmpty(resourcePath))
            {
                Texture = ResourceManager.LoadResource<Texture2D>(resourcePath);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            Trace.Assert(_e != null);

            while (true)
            {
                DrawBeginEventArgs beginArgs = new();

                beginArgs.Position = new Vector2(_e.Transform.Position.X, _e.Transform.Position.Y);

                if (Texture != null)
                {
                    beginArgs.SourceRectangle = Texture.Bounds;
                    beginArgs.Origin = new(Texture.Bounds.Width / 2f, Texture.Bounds.Height / 2f);
                    beginArgs.Texture = Texture;
                }


                OnDrawBegin?.Invoke(this, beginArgs);

                if (beginArgs.Texture != null)
                {
                    spriteBatch.Draw(
                    texture: beginArgs.Texture,
                    position: beginArgs.Position,
                    sourceRectangle: beginArgs.SourceRectangle,
                    color: Color.White,
                    rotation: 0,
                    origin: beginArgs.Origin,
                    scale: new Vector2(_e.Transform.Scale.X, _e.Transform.Scale.Y),
                    effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    layerDepth: LayerDepth);
                }
                else
                {
                    Trace.TraceWarning("Texture is null, can't draw");
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

        private Texture2D? _texture;
        private string _textureURI = "";
        private TransformEntity? _e;
    }
}
