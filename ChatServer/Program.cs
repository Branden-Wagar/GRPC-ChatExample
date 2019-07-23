using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Program
    {
        const string Host = "localhost";
        const int Port = 8080;
        public static void Main(string[] args)
        {
            
            var server = new Server
            {
                Services = { ChatServerProto.ChatServerService.BindService(new ChatServerSvc()) },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };
            // Start server
            server.Start();

            Console.WriteLine("ChatService listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
