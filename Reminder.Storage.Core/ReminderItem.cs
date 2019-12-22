using System;

namespace Reminder.Storage.Core
{
    public class ReminderItem
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
        public long ContactId { get; set; }
        public ReminderStatus Status { get; set; } = ReminderStatus.Awaiting;
        public bool  IsTimeToAlarm => Date<DateTime.UtcNow;
    }
}
