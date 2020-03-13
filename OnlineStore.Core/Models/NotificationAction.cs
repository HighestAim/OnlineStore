using OnlineStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models
{
    public class NotificationAction
    {
        public SocketActionType Type { get; set; }
        public long Id { get; set; }
        public object Action { get; set; }
    }
}
