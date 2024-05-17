using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Packet
{
    [DataContract]
    internal class CharacterData
    {
        public CharacterData(NinjaCharacter ninjaCharacter)
        {
            Position = ninjaCharacter.Position;
            CharacterId = ninjaCharacter.Id;
        }

        [DataMember]
        public Vector2 Position
        {
            get;
            private set;
        }

        [DataMember]
        public int CharacterId
        {
            get;
            private set;
        }
    }
}
