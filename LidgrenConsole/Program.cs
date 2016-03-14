using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;

namespace LidgrenConsole
{
    class Program
    {
        static NetClient client;
        static NetPeerConfiguration config;
        
        static void Main(string[] args)
        {
            NetIncomingMessage chat;
            config = new NetPeerConfiguration("Chat");
            Console.WriteLine("Enter a Username:");
            String username = Console.ReadLine();
            client = new NetClient(config);
            client.Start();
            client.Connect("127.0.0.1", 25565);
            Thread.Sleep(500);
            
            while (true)
            {
                NetOutgoingMessage msg = client.CreateMessage();
                String text = Console.ReadLine();
                msg.Write(username + ": " + text);
                client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
               
            }
            
            
            client.Disconnect("bye bye");
        }
    }
}
