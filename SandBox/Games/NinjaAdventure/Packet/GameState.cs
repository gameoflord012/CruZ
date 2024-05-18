using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Packet
{
    [DataContract]
    internal class GameState
    {
        public GameState(NinjaAdventureDecorator gameScene)
        {
            Monsters = gameScene.Monsters;
            Characters = gameScene.Characters;
        }

        [DataMember]
        public IEnumerable<MonsterData> Monsters
        {
            get;
            private set;
        }

        [DataMember]
        public IEnumerable<CharacterData> Characters
        {
            get;
            private set;
        }
    }
}
