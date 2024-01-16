using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    using Microsoft.Xna.Framework;

    public partial class SpriteComponent : IComponent, IComponentCallback
    {
        public SpriteComponent() { }
        public SpriteComponent(string resourceName) { LoadTexture(resourceName); }

        public Type         ComponentType   => typeof(SpriteComponent);
        [JsonIgnore]
        public Texture2D?   Texture         { get => _texture; set => _texture = value; }
        
        public Microsoft.Xna.Framework.Rectangle    SourceRectangle;
        public Vector2      Origin;
        public bool         Flip;
        public float        LayerDepth { get; set; } = 0;

        public void LoadTexture(string resourcePath)
        {
            _textureURI = resourcePath;

            if(!string.IsNullOrEmpty(resourcePath))
            {
                Texture = ResourceManager.LoadResource<Texture2D>(resourcePath);
                SourceRectangle = Texture.Bounds;
                Origin = new(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            if(Texture == null)
            {
                Trace.TraceWarning("Texture is unloaded, draw will not execute");
                return;
            }

            Trace.Assert(_e != null);

            spriteBatch.Draw(
                Texture,
                position:           new Vector2(_e.Transform.Position.X, _e.Transform.Position.Y),
                sourceRectangle:    SourceRectangle,
                color:              Color.White,
                rotation:           0,
                origin:             Origin,
                scale:              new Vector2(_e.Transform.Scale.X, _e.Transform.Scale.Y),
                effects:            Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                layerDepth:         LayerDepth);
        }

        public void OnEntityChanged(TransformEntity entity)
        {
            _e = entity;
        }

        private Texture2D? _texture;
        private string _textureURI = "";
        private TransformEntity? _e;
    }
}
