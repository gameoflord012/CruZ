using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

using NinjaAdventure.Server;

class Program
{
    private static readonly IPAddress BroadCastAddress = IPAddress.Parse("192.168.1.255");
    private static int Port = 11000;

    static void Main(string[] args)
    {
        UdpClient udpClient = new();
        IPEndPoint broadCast = new(BroadCastAddress, Port);

        udpClient.Send(Encoding.ASCII.GetBytes("GET_GAME_STATE"), broadCast);

        IPEndPoint serverEP = new(IPAddress.Any, Port);
        var bytes = udpClient.Receive(ref serverEP);

        ProcessServerResponse(bytes);
    }

    private static void ProcessServerResponse(byte[] bytes)
    {
        DataContractSerializer serializer = new(typeof(GameState));

        GameState gameState;

        using(MemoryStream stream = new(bytes))
        {
            gameState = (GameState)serializer.ReadObject(stream)!;
        }
    }
}
