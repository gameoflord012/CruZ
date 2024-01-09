using Microsoft.Xna.Framework;
using System;

namespace CruZ.Systems
{
    public interface IECSContextProvider
    {
        event Action<GameTime> DrawEvent;
        event Action<GameTime> UpdateEvent;
        event Action InitializeSystemEvent;
    }
}