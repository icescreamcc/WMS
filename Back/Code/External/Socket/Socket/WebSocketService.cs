using External.Socket.TCP;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

namespace External.Socket.Socket
{
    public class WebSocketService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public event EventHandler<WebSocketArgs>? OnDataReceived;

        public event EventHandler<WebSocketArgs>? OnConnectFailed;

        public async Task HandleWebSocketAsync(HttpContext httpContext, WebSocket webSocket)
        {
            var clientId = Guid.NewGuid().ToString();
            _sockets.TryAdd(clientId, webSocket);
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string clientHost = httpContext.Connection.RemoteIpAddress.ToString();
            string clientPort = httpContext.Connection.RemotePort.ToString();
            while (!result.CloseStatus.HasValue)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (OnDataReceived != null && !string.IsNullOrEmpty(message))
                {
                    var socketArgs = new WebSocketArgs
                    {
                        ClientAddress = clientHost,
                        ClientPort = clientPort,
                        Message = message
                    };
                    OnDataReceived(this, socketArgs);
                }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            _sockets.TryRemove(clientId, out _);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public async Task SendMessage<TSocketMessageModel>(TSocketMessageModel data, string clientId) where TSocketMessageModel : class
        {
            if (_sockets.TryGetValue(clientId, out var webSocket) && webSocket?.State == WebSocketState.Open)
            {
                var dataStr = JsonConvert.SerializeObject(data);
                byte[] buffer = Encoding.UTF8.GetBytes(dataStr);
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMassage<TSocketMessageModel>(TSocketMessageModel data) where TSocketMessageModel : class
        {
            foreach (var dict in _sockets)
            {
                if (dict.Value != null)
                {
                    if (dict.Value.State == WebSocketState.Open)
                    {
                        var dataStr = JsonConvert.SerializeObject(data);
                        byte[] _byte = Encoding.UTF8.GetBytes(dataStr);
                        await dict.Value.SendAsync(_byte, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }

        public async Task CloseWebSocketAsync(WebSocket webSocket, WebSocketCloseStatus closeStatus, string closeStatusDescription = "")
        {
            if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
            {
                await webSocket.CloseAsync(closeStatus, closeStatusDescription, CancellationToken.None);
            }
        }
    }
}
