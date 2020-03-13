using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnlineStore.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Managers
{
    public class SocketManager
    {
        /// <summary>
        /// ConnectionId - {Socket, UserId}
        /// </summary>
        private static ConcurrentDictionary<string, SocketValue> _sockets = new ConcurrentDictionary<string, SocketValue>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value?.Socket;
        }

        public ConcurrentDictionary<string, SocketValue> GetAll()
        {
            return _sockets;
        }

        public Dictionary<string, SocketValue> GetAllAuthorizedSockets()
        {
            return _sockets.Where(x => x.Value.UserId.HasValue)
                           .ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, SocketValue> GetAllUnAuthorizedSockets()
        {
            return _sockets.Where(x => !x.Value.UserId.HasValue)
                           .ToDictionary(x => x.Key, x => x.Value);
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value?.Socket == socket).Key;
        }

        public string AddSocket(WebSocket socket)
        {
            var id = CreateConnectionId();
            _sockets.TryAdd(id, new SocketValue { Socket = socket});

            return id;
        }

        public bool AddUserIdToSocket(string socketId, long userId)
        {
            if (string.IsNullOrEmpty(socketId) || !_sockets.ContainsKey(socketId))
            {
                return false;
            }

            _sockets[socketId].UserId = userId;

            return true;
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out SocketValue socketValue);

            await socketValue.Socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public async void SendMessageToSocketsAsync(IEnumerable<WebSocket> sockets, NotificationAction action)
        {
            var settingForCamelCase = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            var message = JsonConvert.SerializeObject(action, settingForCamelCase);
            foreach (var socket in sockets)
            {
                await SendMessageAsync(socket, message);
            }
        }

        public async void SendMessageToClientsAsync(IEnumerable<long> clientIds, NotificationAction action)
        {
            var settingForCamelCase = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            var message = JsonConvert.SerializeObject(action, settingForCamelCase);
            foreach (var pair in _sockets)
            {
                if (pair.Value.Socket.State == WebSocketState.Open &&
                    (clientIds == null || 
                    (pair.Value.UserId.HasValue && clientIds.Contains(pair.Value.UserId.Value))))
                {
                    await SendMessageAsync(pair.Value.Socket, message);
                }
            }
        }

        private async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public class SocketValue
    {
        public long? UserId { get; set; }
        public WebSocket Socket { get; set; }
    }

}
