using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.Editor
{
    internal class WeakReferenceComparer : IEqualityComparer<WeakReference>
    {
        public bool Equals(WeakReference? x, WeakReference? y)
        {
            if(x == null || y == null || !x.IsAlive || !y.IsAlive)
            {
                return x == y;
            }

            return ReferenceEquals(x.Target, y.Target);
        }

        public int GetHashCode([DisallowNull] WeakReference obj)
        {
            return obj.Target != null ? obj.Target.GetHashCode() : obj.GetHashCode();
        }
    }
}
