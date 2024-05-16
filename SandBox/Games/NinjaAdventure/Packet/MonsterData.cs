using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Packet
{
    [DataContract]
    internal class MonsterData
    {
        public MonsterData(Vector2 position, int id)
        {
            Position = position;
            Id = id;
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
