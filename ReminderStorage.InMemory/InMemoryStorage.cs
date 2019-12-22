using Reminder.Storage.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReminderStorage.InMemory
{
    public class InMemoryStorage : IReminderStorage
    {
        internal readonly Dictionary<Guid, ReminderItem> reminders = new Dictionary<Guid, ReminderItem>();

        public void Add(Guid guid, ReminderItem item)
        {
            reminders.Add(guid, item);
        }

        public ReminderItem Get(Guid id)
        {
            if (reminders[id] == null) throw new Exception("НЕ элемента с таким ID");
            return reminders[id];
        }

        public void Display()
        {

            if (reminders.Count > 0) { 
                string msg = "";

                foreach (KeyValuePair<Guid, ReminderItem> pair in reminders)
                    msg += $"{pair.Key.ToString()}. Отправить \"{pair.Value.Message}\"\n";
                Console.WriteLine(msg);
            }
            else
            {
                Console.WriteLine("Нет задач для выполнения");
            }

        }

        public int Count() => reminders.Count;

        public void Remove(Guid id)
        {
            reminders.Remove(id);
        }

        public void Clear()
        {
            reminders.Clear();
        }

        public List<ReminderItem> Get(int count, int startPosition)
        {
            var items = reminders.Values.ToList();
            if (startPosition != 0) items = items.Skip(startPosition).ToList();
            return items = count != 0 ? items.Take(count).ToList() : items;
        }

        public List<ReminderItem> Get(ReminderStatus status, int count=0, int startPosition=0)
        {
            var items = reminders.Values.ToList().Where(r => r.Status == status).ToList();
            if (startPosition != 0) items = items.Skip(startPosition).ToList();
            return items = count != 0 ? items.Take(count).ToList() : items;
        }

        public void UpdateStatus(Guid id, ReminderStatus status)
        {
            reminders[id].Status = status;
        }

        public void UpdateStatus(IEnumerable<Guid> ids, ReminderStatus status)
        {
            foreach(Guid id in ids)
            {
                if (reminders.ContainsKey(id)) reminders[id].Status = status;
            }
           
        }
    }
}
