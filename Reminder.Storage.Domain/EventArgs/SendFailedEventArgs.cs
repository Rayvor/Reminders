using System;
using System.Collections.Generic;
using System.Text;
using Reminder.Storage.Core;

namespace Reminder.Storage.Domain.EventArgs
{
    public class SendFailedEventArgs: System.EventArgs
    {
        public Exception exception { get; set; }
        public ReminderItem  reminderItem { get; set; }


        public SendFailedEventArgs(Exception exception, ReminderItem item)
        {
            this.exception = exception;
            reminderItem = item;
        }
    }
}
