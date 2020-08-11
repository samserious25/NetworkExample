using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Gallows.Server
{
    public class Network: MonoBehaviour
    {
        public InputField inputField;
        public Text players;
        private ServerTimer serverTimer;

        private void Awake()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyTypeResolveEventHandler);
        }

        public void Connect()
        {
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
