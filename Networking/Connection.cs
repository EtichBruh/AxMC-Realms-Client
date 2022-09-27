using Microsoft.Xna.Framework;
using nekoT;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.Networking
{
    public static class Connection
    {
        static Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static byte[] Buffer = new byte[256];
        public static int players = 0;
        public static void Connect()
        {
            try
            {
                client.Connect(IPAddress.Parse("127.0.0.1"), 2050);
                if (client.Connected)
                    Begin();
                /*                  Console.WriteLine("connected!");
                int bytesread = await client.ReceiveAsync(Buffer, SocketFlags.None);
                Array.Resize(ref Buffer, bytesread);
                if(bytesread > 0 && Buffer[0] == 0)
                {
                    _connectedplayers = Buffer[1];
                    Buffer = new byte[256];
                }*/
            }
            catch (Exception e)
            {
                client.Close();
            }
        }
        static async void Begin()
        {
            while (client.Connected)
            {
                //if (!await Read(1)) break;

                int bytesRead = await client.ReceiveAsync(Buffer, 0);
                Array.Resize(ref Buffer, bytesRead);
                var id = Buffer[0];

                //var size = Buffer.Length - 1;//BinaryPrimitives.ReadInt32BigEndian(_receiveBuffer) - 5;

                //if (!await Read(size))
                //    break;
                //_receiveCipher.Crypt(_receiveBuffer, 0, size);
                if (!ProcessPacket(id))
                    break;
                Buffer = new byte[256];
            }
            client.Close();
        }

        static bool ProcessPacket(int id)
        {
            PacketId index = (PacketId)id;
            if (index == PacketId.Hello)
            {
                players = Buffer[1];
                Game1.NetworkPlayers = new Vector2[players - 1];
                return true;
            }
            if (index == PacketId.Position)
            {
                byte[] rawPositions = ReadAfterId(Buffer);
                byte[] temp = new byte[4]; // 2 is sizeof ushort in bytes
                for (int i = 0; i < players - 1; i++)
                {
                    Array.Copy(rawPositions, i * 4, temp, 0, 4); // take a chunk of 4 bytes and paste into temp
                    Game1.NetworkPlayers[i] = temp.AsVector2();
                    Array.Clear(temp, 0, temp.Length);
                }
                return true;
            }
            return false;
        }

        static async ValueTask<bool> Read(int numBytes)
        {
            int bytesRead = 0;

            if (numBytes > 1024) return false;

            try
            {
                while (bytesRead < numBytes)
                {

                    var buff = new Memory<byte>(Buffer, bytesRead, numBytes - bytesRead);
                    bytesRead = await client.ReceiveAsync(buff, 0);

                    if (bytesRead == 0) return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static byte[] ReadAfterId(byte[] buffer)
        {
            byte[] Data = Array.Empty<byte>();
            Array.Resize(ref Data, buffer.Length - 1);
            Array.Copy(buffer, 1, Data, 0, Data.Length);
            return Data;
        }

        /// <summary>
        /// Sends player Position to connected server
        /// </summary>
        /// <param name="pos">Position Array</param>
        public static void SendPosition(byte[] pos)
        {
            if (client.Connected)
            {
                client.Send(new byte[5] { (byte)PacketId.Position, pos[0], pos[1], pos[2], pos[3] });
            }
        }
    }
}
