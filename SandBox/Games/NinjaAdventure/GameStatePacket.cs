using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Server
{
    [DataContract]
    internal class GameState
    {
        public GameState(IEnumerable<Vector2> monsterPosition)
        {
            MonsterPositions = monsterPosition;
        }

        [DataMember]
        public IEnumerable<Vector2> MonsterPositions
        {
            get;
            private set;
        }
    }
}
