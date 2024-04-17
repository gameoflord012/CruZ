using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    public abstract class RendererComponent : Component, IComparable<RendererComponent>
    {
        public int SortingLayer { get; set; }

        public float LayerDepth { get; set; }

        public virtual void Render(RendererEventArgs args) { }

        public int CompareTo(RendererComponent? other)
        {
            var sortingLayerCompare = SortingLayer.CompareTo(other.SortingLayer);
            return sortingLayerCompare == 0 ? LayerDepth.CompareTo(other.LayerDepth) : sortingLayerCompare;
        }
    }
}
