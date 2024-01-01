using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace CruZ.Components
{
    public partial class SpriteComponent : ISpriteBatchDrawable, IComponentAddedCallback
    {
        public SpriteComponent() { }
        public SpriteComponent(string resourceName) { LoadTexture(resourceName); }

        public Texture2D? Texture { get => _texture; set => _texture = value; }

        public void LoadTexture(string resourceName)
        {
            _resourceName = resourceName;
            Texture = ResourceManager.LoadContent<Texture2D>(resourceName);

            //UpdateRectTransform();
        }

        //private void UpdateRectTransform()
        //{
        //    if (_e == null) return;

        //    _e.RectTransform.SetWidth((float)Texture.Width / MGWrapper.Viewport.Width * MGWrapper.VIRTUAL_WIDTH);
        //    _e.RectTransform.SetHeight((float)Texture.Height / MGWrapper.Viewport.Height * MGWrapper.VIRTUAL_HEIGHT);
        //}

        public virtual void Draw(SpriteBatch spriteBatch, Matrix viewMatrix)
        {
            if(Texture == null)
            {
                Trace.TraceWarning("Texture is unloaded, draw will not execute");
                return;
            }

            //var scale = new Vector3(
            //    _e.RectTransform.Size.X / Texture.Width,
            //    _e.RectTransform.Size.Y / Texture.Height);

            Trace.Assert(_e != null);

            spriteBatch.Begin(transformMatrix: 
                _e.Transform.TotalMatrix * viewMatrix);

            spriteBatch.Draw(
                Texture,
                new Vector2(
                    -Texture.Width / 2f * _e.Transform.Scale.X, 
                    -Texture.Height / 2f * _e.Transform.Scale.Y),
                sourceRectangle: null,
                Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                scale: new Vector2(_e.Transform.Scale.X, _e.Transform.Scale.Y),
                effects: SpriteEffects.None,
                layerDepth: 0);

            spriteBatch.End();
        }

        public void OnComponentAdded(TransformEntity entity)
        {
            _e = entity;
            //UpdateRectTransform();
        }

        private Texture2D? _texture;
        private string _resourceName = "";
        private TransformEntity? _e;
    }
}
