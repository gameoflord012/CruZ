using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    public class RendererComponent : Component, IComparable<RendererComponent>
    {
        public override Type ComponentType => typeof(RendererComponent);
        
        public int SortingLayer { get; set; }
        
        public float LayerDepth { get; set; }
        
        public virtual void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix) { }

        public int CompareTo(RendererComponent? other)
        {
            var sortingLayerCompare = SortingLayer.CompareTo(other.SortingLayer);
            return sortingLayerCompare == 0 ? LayerDepth.CompareTo(other.LayerDepth) : sortingLayerCompare;
        }
    }
}
