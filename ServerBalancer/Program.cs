using System;
using System.Reflection;

namespace Gallows.Server
{
    class Program
    {
        private static GameServer gameServer;
        private static ServerTimer serverTimer;

        public static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth / 3, Console.WindowHeight);

            Start();

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyTypeResolveEventHandler);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnCloseApp);

            Console.ReadKey();
        }

        private static Assembly MyTypeResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return typeof(NetworkMessage).Assembly;
        }

        public static void Start()
        {
            serverTimer = new ServerTimer(256);
            gameServer = new GameServer(6667);

            serverTimer.OnTick += gameServer.Update;
        }

        private static void OnCloseApp(object sender, EventArgs e)
        {
            serverTimer.Stop();
            gameServer.Stop();

            Console.WriteLine("Server closing");
        }
    }
}
