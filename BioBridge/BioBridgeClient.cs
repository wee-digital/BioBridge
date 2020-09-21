using System;
using System.Text;
using System.IO;
using System.Net.WebSockets;
using System.Threading;

namespace BioBridge
{
    class BioBridgeClient
    {

        private ClientWebSocket socketClient = new ClientWebSocket();

        private WebSocketReceiveResult result;

        public BioBridgeClient()
        {
        }

        public async void Connect(string url)
        {
            try
            {
                socketClient = new ClientWebSocket();
                await socketClient.ConnectAsync(new Uri(url), CancellationToken.None);
                listener?.OnStateChanged(socketClient.State);
                Receive();
            }
            catch (Exception e)
            {
                listener?.OnError(e);
            }
        }

        public async void Close()
        {
            try
            {
                await socketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                listener?.OnStateChanged(socketClient.State);
            }
            catch (Exception e)
            {
                listener?.OnError(e);
            }
        }

        public void Send(string message)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                socketClient.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                listener?.OnError(e);
            }

        }

        public WebSocketState State() {
            return socketClient.State;
        }
        
        public async void Receive()
        {
            while (socketClient.State == WebSocketState.Open)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {

                        do
                        {
                            var receiveBuffer = ClientWebSocket.CreateClientBuffer(1024, 1024);
                            result = await socketClient.ReceiveAsync(receiveBuffer, CancellationToken.None);
                            ms.Write(receiveBuffer.Array, 0, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var data = Encoding.UTF8.GetString(ms.ToArray());
                            listener?.OnMessage(data);
                        }
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;
                    }

                }
                catch (Exception e)
                {
                    listener?.OnError(e);
                }
            }


        }

        public interface IListener
        {
            void OnStateChanged(WebSocketState state);

            void OnMessage(string data);

            void OnError(Exception e);
        }

        public IListener listener;

    }

}
