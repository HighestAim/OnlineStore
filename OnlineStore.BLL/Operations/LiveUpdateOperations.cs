using OnlineStore.BLL.Managers;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Operations
{
    public class LiveUpdateOperations : ILiveUpdateOperations
    {
        private readonly SocketManager _socketManager;

        public LiveUpdateOperations(SocketManager socketManager)
        {
            _socketManager = socketManager;
        }

        public async Task SendNotificationToSpecificUsers(long actionId, SocketActionType actionType, object action = null, params long[] clientIds)
        {
            await Task.Factory.StartNew(() =>
            {
                var notificationAction = new NotificationAction
                {
                    Action = action,
                    Id = actionId,
                    Type = actionType
                };
                _socketManager.SendMessageToClientsAsync(clientIds, notificationAction);
            });
        }

        public async Task SendNotificationToAuthorizedUsers(long actionId, SocketActionType actionType, object action = null)
        {
            var signedClientIds = _socketManager.GetAllAuthorizedSockets().Select(x => x.Value.UserId.Value);

            await SendNotificationToSpecificUsers(actionId, actionType, action, signedClientIds.ToArray());
        }

        public async Task SendNotificationToUnAuthorizedUsers(long actionId, SocketActionType actionType, object action = null)
        {
            var anonymousConnections = _socketManager.GetAllUnAuthorizedSockets().Select(x => x.Value.Socket);

            await Task.Factory.StartNew(() =>
            {
                var notificationAction = new NotificationAction
                {
                    Action = action,
                    Id = actionId,
                    Type = actionType
                };
                _socketManager.SendMessageToSocketsAsync(anonymousConnections, notificationAction);
            });
        }

        public async Task SendNotificationToAllUsers(long actionId, SocketActionType actionType, object action = null)
        {
            await Task.Factory.StartNew(() =>
            {
                var notificationAction = new NotificationAction
                {
                    Action = action,
                    Id = actionId,
                    Type = actionType
                };
                _socketManager.SendMessageToSocketsAsync(_socketManager.GetAll().Select(x => x.Value.Socket), notificationAction);
            });
        }
    }
}
