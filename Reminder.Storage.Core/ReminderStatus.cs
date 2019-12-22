using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Storage.Core
{
    public enum ReminderStatus
    {
        Awaiting=1,
        Sended,
        Failed,
        ReadyToSend
    }
}
