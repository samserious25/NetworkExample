using System;
using System.Reflection;

namespace Gallows.Server
{
    class Program
    {
        public static ServerTimer serverTimer;

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth / 3, Console.WindowHeight);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyTypeResolveEventHandler);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnCloseApp);

            if (args.Length > 0)
                StartRoom(args[0]);
            else
                Console.WriteLine("No port assigned for connection");

            Console.ReadKey();
        }

        private static void StartRoom(string port)
        {
            serverTimer = new ServerTimer(256);
            serverTimer.OnTick += GameRoom.Update;

            GameRoom.Start(int.Parse(port));
        }

        private static Assembly MyTypeResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return typeof(NetworkMessage).Assembly;
        }

        private static void OnCloseApp(object sender, EventArgs e)
        {
            serverTimer.Stop();
            GameRoom.Stop();

            Console.WriteLine("Room closing");
        }
    }
}
