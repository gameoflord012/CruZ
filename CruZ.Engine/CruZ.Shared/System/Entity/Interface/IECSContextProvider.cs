using Microsoft.Xna.Framework;
using System;

namespace CruZ.Systems
{
    public interface IECSContextProvider
    {
        event Action<GameTime> ECSDraw;
        event Action<GameTime> ECSUpdate;
        event Action InitializeECSSystem;
    }
}