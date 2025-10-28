using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManager;
using System;

namespace OrderManagerUnitTests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void OrderConstuctor_Test1()
        {
            string name = "Василий";
            string description = "2 банки колы";
            DateTime dateTime = DateTime.Now;
            Order order = new Order(name, description, dateTime);

            Assert.IsNotNull(order);
            Assert.AreEqual(name, order.CustomerName);
            Assert.AreEqual(description, order.Description);
            Assert.AreEqual(dateTime, order.CreationDate);
            Assert.AreEqual(OrderStatus.Новый, order.Status);
        }

        [TestMethod]
        public void UpdateStatus_Test1()
        {
            OrderStatus newStatus = OrderStatus.Завершён;
            Order order = new Order("order", "description", DateTime.Now);

            order.UpdateStatus(newStatus);

            Assert.AreEqual(newStatus, order.Status);
        }
    }
}
