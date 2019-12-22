using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Storage.Domain.Models
{
    public class SendReminderItemModel
    {
        public Guid Id { get; set; }
        public long ContactId { get; set; }
        public string Message { get; set; }
    }
}
