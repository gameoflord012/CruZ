using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NinjaAdventure.Server
{
    internal class RequestProcessor
    {
        public RequestProcessor(MonsterSpawnerScene gameScene)
        {
            _gameScene = gameScene;
        }

        public bool ProcessRequest(byte[] bytes, out byte[] output)
        {
            string request = Encoding.ASCII.GetString(bytes);

            if(request.StartsWith("GET_GAME_STATE"))
            {
                output = GetGameStatePacket();
                return true;
            }

            output = [];
            return false;
        }

        private byte[] GetGameStatePacket()
        {
            var gameState = new GameState(_gameScene.MonsterPositions);

            using(var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(GameState));
                serializer.WriteObject(stream, gameState);
                return stream.ToArray();
            }
        }

        MonsterSpawnerScene _gameScene;
    }
}
