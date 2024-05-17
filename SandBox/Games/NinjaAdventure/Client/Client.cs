using System;
using System.Net;
using System.Net.Sockets;
namespace NinjaAdventure.Client;

internal class Client
{
    public const int Port = 11000;
    public static readonly IPAddress BroadCastAddress = IPAddress.Parse("192.168.1.255");
    public static readonly IPEndPoint BroadCastEP = new(BroadCastAddress, Port);

    public Client()
    {
        _udp = new();
    }

    private UdpClient _udp;

    public static void CreateContext()
    {
        if(_instance != null) throw new InvalidOperationException("Already created");
        _instance = new();
    }

    public static void Send(byte[] bytes)
    {
        Instance._udp.Send(bytes, BroadCastEP);
    }

    public static byte[] Receive()
    {
        IPEndPoint ep = new(IPAddress.Any, Port);
        return Instance._udp.Receive(ref ep);
    }

    private static Client Instance
    {
        get => _instance ?? throw new NullReferenceException();
    }

    private static Client? _instance;
}
