using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NinjaAdventure.Client
{
    internal abstract class Communicator
    {
        public abstract byte[] GetRequest();
        public abstract void ProcessResponse(byte[] bytes);

        public void Communicate()
        {
            Client.Send(GetRequest());
            //Console.WriteLine($"=== Request sent: {Encoding.ASCII.GetString(GetRequest())}");
            ProcessResponse(Client.Receive());
            //Console.WriteLine($"... Response processed");
        }
    }
}
