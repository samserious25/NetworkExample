using System;
using System.Timers;

namespace Gallows.Server
{
    public class ServerTimer
    {
        private readonly Timer serverTimer;
        public Action OnTick = delegate { };

        public ServerTimer(double updateInterval)
        {
            serverTimer = new Timer(updateInterval);
            serverTimer.Elapsed += Tick;
            serverTimer.AutoReset = true;
            serverTimer.Enabled = true;
        }

        private void Tick(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("Tick");
            OnTick();
        }

        public void Stop()
        {
            serverTimer.Stop();
        }
    }
}
