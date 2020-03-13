using OnlineStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Abstractions.OperationInterfaces
{
    public interface ILiveUpdateOperations
    {
        Task SendNotificationToAllUsers(long actionId, SocketActionType actionType, object action = null);
        Task SendNotificationToAuthorizedUsers(long actionId, SocketActionType actionType, object action = null);
        Task SendNotificationToSpecificUsers(long actionId, SocketActionType actionType, object action = null, params long[] clientIds);
        Task SendNotificationToUnAuthorizedUsers(long actionId, SocketActionType actionType, object action = null);
    }
}
