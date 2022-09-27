using AxMC_Realms_Client.Networking;
using System;
using System.Threading;

namespace AxMC_Realms_Client
{
    public static class Program
    {

        [STAThread]
        static void Main()
        {

            new Thread(Connection.Connect).Start();
            using (var game = new Game1())
                game.Run();
        }
    }
}
