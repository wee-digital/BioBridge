using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Threading;

namespace BioBridge
{

    class Program 
    {

        private static string URL = "ws://192.168.42.129:8887";
        public static string currentCommand = "";
        public static BioBridgeClient client;
        private static string cameraStatus = "";
        private static string fingerprintStatus = "";

       

        static void Main(string[] args)
        {
            client = new BioBridgeClient
            {
                listener = new BioBridgeClientListener()
            };

            while (!currentCommand.Equals("3"))
            {
                currentCommand = client.State() != WebSocketState.Open ? ShowConnectivity() : ShowMainMenu();
            }
        }

        public static string ShowConnectivity() 
        {
            Console.Clear();
            Console.WriteLine("{0}::Try to connect {1} ...", Utils.Now(), URL);
            client.Connect(URL);
            return Console.ReadLine();
        }

        public static string ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}: Bio bridge socket: {1} connected, type to command: ", Utils.Now(), URL);
            Console.ResetColor();
            Console.WriteLine("1) Start camera");
            Console.WriteLine("2) Start fingerprint");
            Console.WriteLine("3) Exit");
            var s = Console.ReadLine();
            switch (s)
            {
                case "1":
                    var command1 = cameraStatus.Equals("OPEN") ? "CAMERA_STOP" : "CAMERA";
                    client.Send(command1);
                    break;
                case "2":
                    var command2 = fingerprintStatus.Equals("OPEN") ? "FINGERPRINT_STOP" : "FINGERPRINT";
                    client.Send(command2);
                    break;
                case "3":
                    client.Close();
                    break;
                default:
                    break;
            }
            return s;
        }

        public static void OnCameraData(CameraData data)
        {
            cameraStatus = data.Status;

            // handle data callback here
        }

        public static void OnFingerprintData(FingerprintData data)
        {
            fingerprintStatus = data.Status;

            // handle data callback here 

        }
    }

    
    class BioBridgeClientListener : BioBridgeClient.IListener
    {
        public void OnError(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message.ToString());
            Console.ResetColor();
            if (Program.currentCommand == "0" && Program.client.State() != WebSocketState.Open)
            {
                Thread.Sleep(5000);
                Program.ShowConnectivity();
            }  Program.ShowConnectivity();
            
        }

        public void OnStateChanged(WebSocketState state)
        {
            if (Program.client.State() == WebSocketState.Open)
            {
                Program.ShowMainMenu();
            }
        }

        public void OnMessage(string data)
        {
            if (data == null || data.Equals("")) return;

            try
            {
                SocketData socketData = JsonConvert.DeserializeObject<SocketData>(data);
                if (socketData.CameraData != null)
                {
                    Program.OnCameraData(socketData.CameraData);
                    return;
                }
                if (socketData.FingerprintData != null)
                {
                    Program.OnFingerprintData(socketData.FingerprintData);
                }
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }
 
    }


}
