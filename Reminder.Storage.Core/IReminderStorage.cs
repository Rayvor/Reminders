using System;
using System.Collections.Generic;
using System.Text;

namespace Reminder.Storage.Core
{
    public interface IReminderStorage
    {
        void Add(Guid guid, ReminderItem item);
        ReminderItem Get(Guid id);
        int Count();
        void Remove(Guid id);
        void Clear();
        List<ReminderItem> Get(int count, int startPosition);
        List<ReminderItem> Get(ReminderStatus status, int count, int startPosition);
        void UpdateStatus(Guid id, ReminderStatus status);
        void UpdateStatus(IEnumerable<Guid> ids, ReminderStatus status);
    }
}
