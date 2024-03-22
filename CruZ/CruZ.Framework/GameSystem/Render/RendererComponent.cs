using System;

using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    public abstract class RendererComponent : Component, IComparable<RendererComponent>
    {
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
