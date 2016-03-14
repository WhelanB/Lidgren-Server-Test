using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace LidgrenConsoleServer
{
    class Program
    {
        static NetServer server;
        static NetPeerConfiguration config;
        static List<String> chatMessages;
        static void Main(string[] args)
        {
            chatMessages = new List<String>();
            config = new NetPeerConfiguration("Chat");
            config.Port = 25565;
            config.MaximumConnections = 10;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("Server has started");
            NetIncomingMessage chat;
            
            while (true)
            {
                
                if ((chat = server.ReadMessage()) != null)
                {
                    switch (chat.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            chat.SenderConnection.Approve();
                           // NetOutgoingMessage updateChat = server.CreateMessage();
                           // updateChat.WriteAllProperties(chatMessages);
                            // server.SendMessage(updateChat, chat.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine("User" + chat.SenderConnection.Status);
                            break;
                        case NetIncomingMessageType.Data:
                            String chatMsg = chat.ReadString();
                            Console.WriteLine(chatMsg);
                            NetOutgoingMessage relayChat = server.CreateMessage();
                            relayChat.Write(chatMsg);
                            foreach(NetConnection c in server.Connections.Where(connection => connection != chat.SenderConnection ).ToList())
                            {
                                server.SendMessage(relayChat, c, NetDeliveryMethod.ReliableOrdered);
                            }
                          
                            break;
                      

                    }
                   

                }
            }
            
            
        }
    }
}
