using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReminderStorage.InMemory;
using System;
using System.Threading;

namespace Reminder.Storage.Domain.Tests
{
    [TestClass()]
    public class DomainTests
    {
        [TestMethod()]
        public void SendingSucceded_Event_Test()
        {
            var storage = new InMemoryStorage();
            Domain d = new Domain(storage);
            bool completed = false;
            bool isFailed = false;

            d.OnFailedSend += (o, e) =>
            {
                isFailed = true;
                completed = true;
            };

            d.OnSuccesSend += (o, e) =>
            {
                completed = true;
            };

            d.SendReminder = (model) => { };

            d.Add(new Models.AddReminderItemModel()
            {
                contactId = 123,
                Message = "Test",
                date = DateTime.Now + TimeSpan.FromSeconds(3)
            });

            while (!completed)
            {
                d.CheckAwaitingReminders();
                d.SendReadyRemiders();
                Thread.Sleep(10);
            }

            var list = storage.Get(Core.ReminderStatus.Sended);
            var count = list.Count;

            Assert.AreEqual(isFailed, false);
            Assert.AreEqual(count, 1);
        }

        [TestMethod()]
        public void SendingFailed_Event_Test()
        {
            var storage = new InMemoryStorage();
            Domain d = new Domain(storage);
            bool completed = false;
            bool isFailed = false;

            d.OnFailedSend += (o, e) =>
            {
                isFailed = true;
                completed = true;
                Assert.IsNotNull(e.exception);
            };

            d.OnSuccesSend += (o, e) =>
            {
                Assert.Fail();
            };

            d.SendReminder = (model) => { };

            d.Add(new Models.AddReminderItemModel()
            {
                contactId = 123,
                Message = "Test",
                date = DateTime.Now + TimeSpan.FromSeconds(3)
            });

            while (!completed)
            {
                d.CheckAwaitingReminders();
                d.SendReadyRemiders();
                Thread.Sleep(10);
            }

            var list = storage.Get(Core.ReminderStatus.Failed);
            var count = list.Count;

            Assert.AreEqual(isFailed, true);
            Assert.AreEqual(count, 1);
        }

        [TestMethod()]
        public void ReminderDelegate_Calls_Test()
        {
            var storage = new InMemoryStorage();
            Domain d = new Domain(storage);

            bool completed = false;
            bool isCalled = false;

            d.OnFailedSend += (o, e) =>
            {
                completed = true;
            };

            d.OnSuccesSend += (o, e) =>
            {
                completed = true;
            };

            d.SendReminder = (model) => { isCalled = true; };

            Assert.IsNotNull(d.SendReminder);

            d.Add(new Models.AddReminderItemModel()
            {
                contactId = 123,
                Message = "Test",
                date = DateTime.Now + TimeSpan.FromSeconds(3)
            });

            while (!completed)
            {
                d.CheckAwaitingReminders();
                d.SendReadyRemiders();
                Thread.Sleep(10);
            }

            Assert.AreEqual(isCalled, true);
        }
    }
}