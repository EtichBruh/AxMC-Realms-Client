using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.Networking
{
    class Connection
    {
        static  Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Connection()
        {
            try
            {
                client.Connect(IPAddress.Parse("127.0.0.1"), 2050);
                if (client.Connected)
                    Console.WriteLine("connected!");
                byte[] bytes = new byte[256];

                if (client.Connected)
                {
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes("Hello");

                    // Send a message.
                    client.Send(msg, SocketFlags.None);
                    Console.WriteLine("Sent: {0}", System.Text.Encoding.UTF8.GetString(msg));
                    int i = client.Receive(bytes, SocketFlags.None);

                    // Translate data bytes to a UTF8 string.
                    string data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                client.Close();
            }
        }
        /// <summary>
        /// Sends player Position to connected server
        /// </summary>
        /// <param name="X">Position X</param>
        /// <param name="Y">Position Y</param>
        public static void SendPosition(byte X, byte Y)
        {
            byte[] msg = {((byte)PacketId.Position), X, Y };

            client.Send(msg, SocketFlags.None);
        }
        /// <summary>
        /// Receives other player position from server.
        /// </summary>
        /// <returns>Received Position in Vector2</returns>
        public static Vector2 ReceivePosition()
        {
            byte[] buffer = new byte[4];
            client.Receive(buffer, SocketFlags.None);
            return new Vector2(buffer[2], buffer[3]);
        }
    }
}
