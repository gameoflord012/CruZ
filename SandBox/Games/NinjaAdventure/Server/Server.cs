﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Scene;

using NinjaAdventure;
using NinjaAdventure.Server;

public class Program
{
    private const int ListenPort = 11000;

    public static void Main()
    {
        GameWrapper game = new();
        GameApplication.CreateContext(game, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource"));

        GameApplication.Initialized += GameApplication_Initialized;

        game.Run();
        CloseListener();

        GameApplication.Initialized -= GameApplication_Initialized;
    }

    private static void GameApplication_Initialized()
    {
        _gameScene = new NinjaAdventureDecorator(GameScene.Create());
        _requestProcessor = new RequestResponser(_gameScene);
        _listenerTask = new(StartListener);
        _listenerTask.Start();
    }

    private static void StartListener()
    {
        _listener = new UdpClient(ListenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);

        try
        {
            while(true)
            {
                byte[] bytes = _listener.Receive(ref groupEP);
                //Console.WriteLine($"=== Received request from {groupEP}");
                _requestProcessor.ProcessRequest(bytes, groupEP, out byte[] output);
                //Console.WriteLine($"... Request processed");
                _listener.Send(output, groupEP);
            }
        }
        catch(SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            CloseListener();
        }
    }

    private static void CloseListener()
    {
        lock(_listener)
        {
            _listener.Close();
        }
    }

    private static Task _listenerTask;
    private static UdpClient _listener;
    private static NinjaAdventureDecorator _gameScene;
    private static RequestResponser _requestProcessor;
}
