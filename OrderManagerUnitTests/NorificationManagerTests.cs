using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OrderManager;

namespace OrderManagerUnitTests
{
    [TestClass]
    public class NotificationManagerTests
    {
        [TestMethod]
        public void NotificationManagerConstructor_Test1() // тест для конструктора без параметров
        {
            NotificationManager manager = new NotificationManager();

            Assert.IsNotNull(manager);
        }

        [TestMethod]
        public void NotificationManagerChangeStatusOnInProgressNotifyOn_Test() // тест на формирование сообщения, когда уведомления для взятия в обработку включены
        {
            NotificationManager manager = new NotificationManager();
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderStatus newStatus = OrderStatus.В_обработке;
            Order order = new Order(name, description, dateTime);

            order.UpdateStatus(newStatus);
            string message = manager.NotifyStatusChange(order, OrderStatus.Новый, newStatus);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Contains(name));
            Assert.IsTrue(message.Contains(description));
            Assert.IsTrue(message.Contains(dateTime.ToString()));
            Assert.IsTrue(message.Contains("взят в обработку"));
        }

        [TestMethod]
        public void NotificationManagerChangeStatusCompletedNotifyOn_Test() // тест на формирование сообщения, когда уведомления для завершения заказа включены
        {
            NotificationManager manager = new NotificationManager();
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderStatus newStatus = OrderStatus.Завершён;
            Order order = new Order(name, description, dateTime);

            order.UpdateStatus(newStatus);
            string message = manager.NotifyStatusChange(order, OrderStatus.Новый, newStatus);

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Contains(name));
            Assert.IsTrue(message.Contains(description));
            Assert.IsTrue(message.Contains(dateTime.ToString()));
            Assert.IsTrue(message.Contains("завершен"));
        }

        [TestMethod]
        public void NotificationManagerChangeStatusOnInProgressNotifyOff_Test() // тест на формирование сообщения, когда уведомления для взятия в обработку выключены
        {
            NotificationManager manager = new NotificationManager();
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderStatus newStatus = OrderStatus.В_обработке;
            Order order = new Order(name, description, dateTime);

            manager.NotifyOnInProgress = false;
            order.UpdateStatus(newStatus);
            string message = manager.NotifyStatusChange(order, OrderStatus.Новый, newStatus);

            Assert.IsNotNull(message);
            Assert.AreEqual(message, "");
        }

        [TestMethod]
        public void NotificationManagerChangeStatusCompletedNotifyOff_Test() // тест на формирование сообщения, когда уведомления для завершения заказа выключены
        {
            NotificationManager manager = new NotificationManager();
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderStatus newStatus = OrderStatus.Завершён;
            Order order = new Order(name, description, dateTime);

            manager.NotifyOnCompleted = false;
            order.UpdateStatus(newStatus);
            string message = manager.NotifyStatusChange(order, OrderStatus.Новый, newStatus);

            Assert.IsNotNull(message);
            Assert.AreEqual(message, "");
        }

        [TestMethod]
        public void NotificationManagerChangeNotifyOnInProgress_Test() // тест для свойства notifyOnInProgress, отвечающее за включение и выключение уведомлений для взятия закказа в обработку
        {
            NotificationManager manager = new NotificationManager();
            manager.NotifyOnInProgress = false;

            Assert.IsFalse(manager.NotifyOnInProgress);
        }

        [TestMethod]
        public void NotificationManagerChangeNotifyOnCompleted_Test() // тест для свойства notifyOnInProgress, отвечающее за включение и выключение уведомлений для завершения заказа
        {
            NotificationManager manager = new NotificationManager();
            manager.NotifyOnCompleted = false;

            Assert.IsFalse(manager.NotifyOnCompleted);
        }

        [TestMethod]
        public void NotificationManagerChangeOnTheSameStatus_Test() // тест для проверки возвращения пустой строки при обновлении статуса на тот же самый
        {
            NotificationManager manager = new NotificationManager();
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderStatus newStatus = OrderStatus.Завершён;
            Order order = new Order(name, description, dateTime);

            order.UpdateStatus(newStatus);
            order.UpdateStatus(newStatus);
            string message = manager.NotifyStatusChange(order, newStatus, newStatus);

            Assert.IsNotNull(message);
            Assert.AreEqual(message, "");
        }
    }
}