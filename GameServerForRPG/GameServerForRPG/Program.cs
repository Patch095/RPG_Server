using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerForRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            GameTransportIPv4 transport = new GameTransportIPv4();
            ServerClock clock = new ServerClock();
            GameServer gameServer = new GameServer(transport, clock);

            transport.Bind("127.0.0.1", 9999);

            gameServer.Start();
        }
    }
}