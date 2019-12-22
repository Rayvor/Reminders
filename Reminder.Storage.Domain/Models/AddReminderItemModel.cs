using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Storage.Domain.Models
{
    public class AddReminderItemModel
    {
        public DateTimeOffset date { get; set; }
        public long contactId { get; set; }
        public string Message { get; set; }
    }
}
