using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Enums
{
    public enum SocketActionType
    {
        ContestCreated,
        LineupCreated,
        ContestFinished,
        ContestCancelled,
        MatchEventReceived,
        Notification
    }
}
