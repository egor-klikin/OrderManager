using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OrderManager;

namespace OrderManagerUnitTests
{
    [TestClass]
    public class OrderManagerTests
    {
        [TestMethod]
        public void OrderManagerConstructor_Test1() // тест для конструктора
        {
            OrderManager.OrderManager manager = new OrderManager.OrderManager();

            Assert.IsNotNull(manager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderManagerAddNullOrder_Test1() // тест для проверки выкидывания исключения при попытки добавления null-заказа
        {
            OrderManager.OrderManager manager = new OrderManager.OrderManager();

            manager.AddOrder(null);
        }

        [TestMethod]
        public void OrderManagerAddOrder_Test2() // тест для проверки успешного добавления заказа
        {
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderManager.OrderManager manager = new OrderManager.OrderManager();
            Order order = new Order(name, description, dateTime);

            manager.AddOrder(order);

            Assert.IsNotNull(manager.Orders.Find(x => x.CreationDate == dateTime));
            Assert.AreEqual(name, manager.Orders.Find(x => x.CreationDate == dateTime).CustomerName);
            Assert.AreEqual(description, manager.Orders.Find(x => x.CreationDate == dateTime).Description);
            Assert.AreEqual(dateTime, manager.Orders.Find(x => x.CreationDate == dateTime).CreationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderManagerRemoveNullOrder_Test1() // тест для проверки выкидывания исключения при попытки удаления null-заказа
        {

            OrderManager.OrderManager manager = new OrderManager.OrderManager();

            manager.RemoveOrder(null);
        }

        [TestMethod]
        public void OrderManagerRemoveOrder_Test2() // тест для проверки успешного удаления заказа
        {
            string name1 = "Роман";
            string description1 = "2 пачки кофе";
            DateTime dateTime1 = new DateTime(2025, 10, 12);
            string name2 = "Марк";
            string description2 = "2 пачки конфет";
            DateTime dateTime2 = new DateTime(2022, 12, 12);
            Order order1 = new Order(name1, description1, dateTime1);
            Order order2 = new Order(name2, description2, dateTime2);
            OrderManager.OrderManager manager = new OrderManager.OrderManager();

            manager.AddOrder(order1);
            manager.AddOrder(order2);
            manager.RemoveOrder(order1);

            Assert.AreEqual(-1, manager.Orders.FindIndex(x => x.CreationDate == dateTime1));
            Assert.AreEqual(-1, manager.Orders.FindIndex(x => x.CreationDate == dateTime1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderManagerUpdateNullOrder_Test1() // тест для проверки выкидывания исключения при попытки обноления статуса у null-заказа
        {
            OrderManager.OrderManager manager = new OrderManager.OrderManager();

            manager.UpdateOrderStatus(null, OrderStatus.Завершён);
        }

        [TestMethod]
        public void OrderManagerUpdateOrder_Test2() // тест для проверки успешного обновления статуса заказа
        {
            string name = "Роман";
            string description = "2 пачки кофе";
            DateTime dateTime = DateTime.Now;
            OrderManager.OrderManager manager = new OrderManager.OrderManager();
            Order order = new Order(name, description, dateTime);
            OrderStatus newStatus = OrderStatus.В_обработке;

            manager.AddOrder(order);
            manager.UpdateOrderStatus(order, newStatus);

            Assert.AreEqual(newStatus, manager.Orders.Find(x => x.CreationDate == dateTime).Status);
        }
    }
}