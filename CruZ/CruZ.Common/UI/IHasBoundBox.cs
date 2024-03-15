using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace CruZ.Common.UI
{
    public struct UIBoundingBox
    {
        public UIBoundingBox()
        {
            Points = [];
            Bound = new(0, 0, 0, 0);
        }

        /// <summary>
        /// Bound to be drawn in world coord
        /// </summary>
        public DRAW.RectangleF Bound;
        /// <summary>
        /// Points to be drawn in world coord
        /// </summary>
        public List<Vector2> Points;

        public static UIBoundingBox Default => new UIBoundingBox();

        public bool IsEmpty()
        {
            return Bound.IsEmpty;
        }
    }


    public interface IHasBoundBox
    {
        event Action<UIBoundingBox> BoundingBoxChanged;
    }
}
