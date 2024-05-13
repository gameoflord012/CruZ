using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using CruZ.GameEngine;

using NinjaAdventure;

public class UDPListener
{
    private const int listenPort = 11000;

    private static void StartListener()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while(true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine($"Received broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
            }
        }
        catch(SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public static void Main()
    {
        //StartListener();

        GameWrapper game = new();
        GameApplication.CreateContext(game, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource"));

        GameApplication.Initialized += GameApplication_Initialized;

        game.Run();

        GameApplication.Initialized -= GameApplication_Initialized;
    }

    private static void GameApplication_Initialized()
    {
        NinjaScene.DemoNinjaScene();
    }
}
