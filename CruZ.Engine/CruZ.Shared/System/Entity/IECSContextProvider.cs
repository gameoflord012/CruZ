using Microsoft.Xna.Framework;
using System;

namespace CruZ.Systems
{
    public interface IECSContextProvider
    {
        public event Action<GameTime> DrawEvent;
        public event Action<GameTime> UpdateEvent;
        public event Action InitializeSystemEvent;
    }
}