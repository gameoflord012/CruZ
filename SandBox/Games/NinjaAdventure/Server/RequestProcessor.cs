using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

using CruZ.GameEngine;

using NinjaAdventure.Packet;

namespace NinjaAdventure.Server
{
    internal class RequestResponser
    {
        public RequestResponser(NinjaAdventureDecorator gameScene)
        {
            _gameScene = gameScene;
            _fromEPToCharacter = [];
        }

        public void ProcessRequest(byte[] bytes, IPEndPoint requestEP, out byte[] outResponse)
        {
            string request = Encoding.ASCII.GetString(bytes);

            byte[] response = [];

            GameApplication.MarshalInvoke(() =>
            {
                if(request.StartsWith("GET_GAME_STATE"))
                {
                    response = GetGameStatePacket();
                }
                else
                if(request.StartsWith("INIT_CHARACTER"))
                {
                    if(_fromEPToCharacter.ContainsKey(requestEP.ToString()))
                    {
                        // false if EP character already initialized
                        response = Encoding.ASCII.GetBytes("FALSE");
                    }
                    else
                    {
                        _fromEPToCharacter.Add(request.ToString(), _gameScene.CreateCharacter());
                        response = Encoding.ASCII.GetBytes("SUCCESS");
                    }
                }
            });

            outResponse = response;
        }

        private byte[] GetGameStatePacket()
        {
            var gameState = new GameState(_gameScene);

            using(var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(GameState));
                serializer.WriteObject(stream, gameState);
                return stream.ToArray();
            }
        }

        NinjaAdventureDecorator _gameScene;
        Dictionary<string, NinjaCharacter> _fromEPToCharacter;
    }
}
