using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaAdventure.Client
{
    internal class CharacterCreationCommunicator : Communicator
    {
        public override byte[] GetRequest()
        {
            return Encoding.ASCII.GetBytes("INIT_CHARACTER");
        }

        public override void ProcessResponse(byte[] bytes)
        {
            string response = Encoding.ASCII.GetString(bytes);

            if(response == "FALSE")
            {
                throw new InvalidOperationException();
            }
            else
            {
                Console.WriteLine("Character created successfully");
            }
        }
    }
}
