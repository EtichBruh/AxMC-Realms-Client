using Microsoft.Xna.Framework;
using nekoT;
using System;
using System.Net;
using System.Net.Sockets;

namespace AxMC_Realms_Client.Networking
{
    public static class Connection
    {
        static Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static byte[] Buffer = new byte[256];
        public static int _connectedplayers = 1;
        public static async void Connect()
        {
            try
            {
                client.Connect(IPAddress.Parse("127.0.0.1"), 2050);
                if (client.Connected)
                {
                    Console.WriteLine("connected!");
                    int bytesread = await client.ReceiveAsync(Buffer, SocketFlags.None);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                client.Close();
            }
        }
        static void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    OnReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    OnSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        static void OnReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                e.SetBuffer(e.Offset, e.BytesTransferred);
                if (e.Buffer[0] == 0)
                {
                    _connectedplayers = e.Buffer[1];
                }
                else if (e.Buffer[0] == 1)
                {
                    e.SetBuffer(ReadDataAfterHeader(e.Buffer));
                    Game1.NetworkPlayers = new Vector2[_connectedplayers];
                    byte[] temp = new byte[4];
                    for (int i = 0; i < _connectedplayers; i++)
                        {
                            Array.Copy(e.Buffer, i*4, temp, 0, 4);
                            Game1.NetworkPlayers[i] = temp.AsVector2();
                        }
                }
                if (!(e.UserToken as Socket).ReceiveAsync(e))
                {
                    OnSend(e);
                }
            }
            else
            {
                Disconnect();
            }
        }
        static void OnSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                e.SetBuffer(new byte[256],0,256);
                if (!(e.UserToken as Socket).ReceiveAsync(e))
                {
                    OnReceive(e);
                }
            }
            else
            {
                Disconnect();
            }
        }
        public static void Disconnect() {

            client.Close();
        }

        private static byte[] ReadDataAfterHeader(byte[] buffer)
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
