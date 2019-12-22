using System;
using System.Threading;
using Reminder.Storage.Core;
using ReminderStorage.InMemory;
using Reminder.Storage.Domain;
using Hors;

namespace Reinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Domain d = new Domain(new InMemoryStorage());
            d.OnFailedSend += D_OnFailedSend;
            d.OnSuccesSend += D_OnSuccesSend;
            d.SendReminder = SendReminder;
            //var hors = new HorsTextParser();
            //var result = hors.Parse("через 30 секунд начать отладку", DateTime.Now);
            //
            //var text = result.Text;
            //var date = result.Dates[0].DateFrom; // ->

            d.Add(new Reminder.Storage.Domain.Models.AddReminderItemModel()
            {
                contactId = 123231243,
                Message = "Позвонить другу",
                date = DateTime.Now+TimeSpan.FromSeconds(3)
            });

            d.Display();


            while (true)
            {
                d.CheckAwaitingReminders();
                d.SendReadyRemiders();
                Thread.Sleep(10);
            }
        }

        private static void D_OnSuccesSend(object sender, Reminder.Storage.Domain.EventArgs.SendSuccesEventArgs e)
        {
            Console.WriteLine($"Message to {e.item.ContactId} wass succes send");
        }

        private static void D_OnFailedSend(object sender, Reminder.Storage.Domain.EventArgs.SendFailedEventArgs e)
        {
            Console.WriteLine($"{e.exception.Message}, {e.reminderItem.Message} to {e.reminderItem.ContactId}");
        }

        private static void SendReminder(Reminder.Storage.Domain.Models.SendReminderItemModel model)
        {
            Console.WriteLine($"{model.Id}:{ model.ContactId}:{model.Message}");
        }
    }
}
