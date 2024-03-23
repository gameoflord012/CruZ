using System;
using System.Collections.Generic;
using System.Drawing;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.UI
{
    public struct UIBoundingBox
    {
        public UIBoundingBox() { }

        public UIBoundingBox(RectangleF rect, Vector2[] origins)
        {
            WorldOrigins.AddRange(origins);
            WorldBounds = rect;
        }

        /// <summary>
        /// WorldBounds to be drawn in world coord
        /// </summary>
        public RectangleF WorldBounds;
        /// <summary>
        /// WorldOrigins to be drawn in world coord
        /// </summary>
        public List<Vector2> WorldOrigins = [];

        public static UIBoundingBox Default => new UIBoundingBox();

        public bool IsEmpty()
        {
            return WorldBounds.IsEmpty;
        }
    }


    public interface IHasBoundBox
    {
        event Action<UIBoundingBox> BoundingBoxChanged;
    }
}
