using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CruZ.Components
{
    public partial class SpriteComponent : IComponent, IComponentCallback
    {
        public SpriteComponent() { }
        public SpriteComponent(string resourceName) { LoadTexture(resourceName); }

        public Type         ComponentType   => typeof(SpriteComponent);
        [JsonIgnore]
        public Texture2D?   Texture         { get => _texture; set => _texture = value; }
        
        public Rectangle    SourceRectangle;
        public Vector2      Origin;
        public bool         Flip;

        public void LoadTexture(string TextureURI)
        {
            _textureURI = TextureURI;

            if(!string.IsNullOrEmpty(TextureURI))
            {
                SourceRectangle = Texture.Bounds;
                Texture = ResourceManager.LoadContent<Texture2D>(TextureURI);
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

            spriteBatch.Begin(
                transformMatrix: _e.Transform.TotalMatrix * viewMatrix,
                samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(
                Texture,
                Vector2.Zero,
                sourceRectangle: SourceRectangle,
                Color.White,
                rotation: 0,
                origin: Origin,
                scale: new Vector2(_e.Transform.Scale.X, _e.Transform.Scale.Y),
                effects: Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                layerDepth: 0);

            spriteBatch.End();
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
