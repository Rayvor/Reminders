using System;
using System.Collections.Generic;
using System.Text;
using Reminder.Storage.Core;

namespace Reminder.Storage.Domain.EventArgs
{
    public class SendSuccesEventArgs:System.EventArgs
    {
        public ReminderItem item { get; set; }

        public SendSuccesEventArgs(ReminderItem item)
        {
            this.item = item;
        }
    }
}
