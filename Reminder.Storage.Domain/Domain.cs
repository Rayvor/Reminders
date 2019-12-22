using System;
using System.Linq;
using Reminder.Storage.Core;
using ReminderStorage;
using Reminder.Storage.Domain.Models;
using Reminder.Storage.Domain.EventArgs;
using System.Collections.Generic;
using System.Threading;

namespace Reminder.Storage.Domain
{
    public class Domain
    {
        private IReminderStorage storage;
        private int timeToUpdate;
        public Action<SendReminderItemModel> SendReminder { get; set; }
        public event EventHandler<SendSuccesEventArgs> OnSuccesSend;
        public event EventHandler<SendFailedEventArgs> OnFailedSend;

        /// <summary>
        /// Создание домен контроллера
        /// </summary>
        /// <param name="_storage">Хранилище напоминаний</param>
        public Domain(IReminderStorage _storage) {
            storage = _storage;
            timeToUpdate = 5;
        }


        /// <summary>
        /// Создание домен контроллера
        /// </summary>
        /// <param name="_storage">Хранилище напоминаний</param>
        /// <param name="_timeToUpdate">Промежуток обновленния данных</param>
        public Domain(IReminderStorage _storage, int _timeToUpdate)
        {
            storage = _storage;
            timeToUpdate = _timeToUpdate;
        }


        public void Run()
        {
            while (true)
            {
                Thread.Sleep(timeToUpdate);
            }
        }


        public void Add(AddReminderItemModel model)
        {
            ReminderItem item = new ReminderItem()
            {
                Id = Guid.NewGuid(),
                Date = model.date,
                Message = model.Message,
                ContactId = model.contactId,
                Status = ReminderStatus.Awaiting
            };
            storage.Add(item.Id, item);
        }


        public void CheckAwaitingReminders()
        {
            var ids = storage.Get(ReminderStatus.Awaiting, 0, 0).Where(r => r.IsTimeToAlarm).Select(r=>r.Id);
            storage.UpdateStatus(ids, ReminderStatus.ReadyToSend);
        }

        public void SendReadyRemiders()
        {
            var reminders = storage.Get(ReminderStatus.ReadyToSend, 0, 0).Where(r => r.IsTimeToAlarm).ToList();
            foreach(ReminderItem r_item in reminders)
            {
                SendReminderItemModel sendModel = new SendReminderItemModel()
                {
                    Id = r_item.Id,
                    ContactId = r_item.ContactId,
                    Message = r_item.Message
                };

                try
                {
                    SendReminder?.Invoke(sendModel);
                    storage.UpdateStatus(sendModel.Id, ReminderStatus.Sended);
                    OnSuccesSend?.Invoke(this, new SendSuccesEventArgs(r_item));

                }
                catch (Exception e)
                {
                    storage.UpdateStatus(sendModel.Id, ReminderStatus.Failed);
                    OnFailedSend?.Invoke(this, new SendFailedEventArgs(e, r_item));
                }
            }
        }

        public void Display()
        {
            foreach(ReminderItem item in storage.Get(0, 0))
            {
                Console.WriteLine($"ID:{item.Id};Contact:{item.ContactId};Message:{item.Message}");
            }
        }
    }
}
