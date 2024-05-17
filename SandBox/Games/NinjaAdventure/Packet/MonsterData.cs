using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Packet
{
    [DataContract]
    internal class MonsterData
    {
        public MonsterData(LarvaMonster monster)
        {
            Position = monster.Postition;
            Id = monster.Id;
        }

        [DataMember]
        public Vector2 Position
        {
            get;
            private set;
        }

        [DataMember]
        public int Id
        {
            get;
            private set;
        }
    }
}
