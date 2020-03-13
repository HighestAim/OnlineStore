using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.BLL.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.API.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SocketManager _socketManager;
        private const string ACCESS_TOKEN = "access_token=";

        public WebSocketMiddleware(RequestDelegate next, SocketManager socketManager)
        {
            _next = next;
            _socketManager = socketManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var socketId = _socketManager.AddSocket(socket);

            try
            {
                await Receive(socket, async (result, buffer) =>
                {
                    SocketAuthorization(result, buffer, socketId);

                    await HandleSocketDisconnecting(result, socket, socketId);
                });
            }
            catch (Exception ex)
            {
                if (socket.State == WebSocketState.Open)
                    await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                File.AppendAllLines($"C:\\FantasyServices\\Logs.txt", new List<string> { $"{DateTime.Now} {ex.ToString()}" });
            }

        }

        private void SocketAuthorization(WebSocketReceiveResult result, byte[] buffer, string socketId)
        {
            var data = Encoding.UTF8.GetString(buffer, 0, result.Count);

            if (data.Contains(ACCESS_TOKEN))
            {
                var token = data.Substring(ACCESS_TOKEN.Length);

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var user = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secretkey_secretkey123")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out _);

                var hasUserId = long.TryParse(user.Identity.Name, out long userId);
                if (hasUserId)
                {
                    _socketManager.AddUserIdToSocket(socketId, userId);
                }
            }
        }

        private async Task HandleSocketDisconnecting(WebSocketReceiveResult result, WebSocket socket, string socketId)
        {
            if (result.MessageType == WebSocketMessageType.Close && !string.IsNullOrEmpty(socketId))
            {
                await _socketManager.RemoveSocket(socketId);
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived ||
                    socket.State == WebSocketState.CloseSent)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

                    if (socket.State != WebSocketState.CloseSent)
                    {
                        await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    }
                }
            }
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                        cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
