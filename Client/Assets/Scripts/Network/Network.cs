using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Gallows.Client
{
    public class Network: MonoBehaviour
    {
        public InputField inputField;
        public Text players;
        private ServerTimer serverTimer;

        public void Connect()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyTypeResolveEventHandler);

            StartClient();
        }

        private void StartClient()
        {
            serverTimer = new ServerTimer(256);
            GameClient.Initialize("localhost", 6667, inputField.text);
            serverTimer.OnTick += GameClient.Update;    
        }

        private static Assembly MyTypeResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return typeof(NetworkMessage).Assembly;
        }

        private void OnApplicationQuit()
        {
            GameClient.Stop();

            Debug.Log("Client closing");
        }
    }
}
