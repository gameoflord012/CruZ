using System;
using System.Collections.Generic;
using System.Linq;

namespace CruZ.Common.UI
{
    public struct BoundingBox
    {
        public BoundingBox()
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
        public List<DataType.Vector3> Points;

        public static BoundingBox Default => new BoundingBox();

        public bool IsEmpty()
        {
            return Bound.IsEmpty;
        }
    }


    public interface IHasBoundBox
    {
        event Action<BoundingBox> BoundingBoxChanged;
    }
}
