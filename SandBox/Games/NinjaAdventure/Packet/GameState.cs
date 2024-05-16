using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Packet
{
    [DataContract]
    internal class GameState
    {
        public GameState(IEnumerable<MonsterData> monsterDatas)
        {
            this.MonsterDatas = monsterDatas;
        }

        [DataMember]
        public IEnumerable<MonsterData> MonsterDatas
        {
            get;
            private set;
        }
    }
}
