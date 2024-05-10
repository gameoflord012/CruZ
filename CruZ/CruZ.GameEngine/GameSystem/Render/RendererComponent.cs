using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    public abstract class RendererComponent : Component, IComparable<RendererComponent>
    {
        /// <summary>
        /// Priority value of which SpriteComponent being proccess first
        /// </summary>
        public int SortingLayer { get; set; }

        public float LayerDepth { get; set; }

        public virtual void Render(RenderSystemEventArgs args) { }

        public int CompareTo(RendererComponent? other)
        {
            if (other == null) return 1;

            var sortingLayerCompare = SortingLayer.CompareTo(other.SortingLayer);
            return sortingLayerCompare == 0 ? LayerDepth.CompareTo(other.LayerDepth) : sortingLayerCompare;
        }
    }
}
