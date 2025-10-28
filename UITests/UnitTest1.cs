using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;

namespace UITests
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }
        private Application _app;
        private UIA3Automation _automation;
        private Window _mainWindow;
        private string filePath = @"orders.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            _app = Application.Launch(@"..\..\..\OrderManager\bin\Debug\OrderManager.exe");
            _automation = new UIA3Automation();
            _mainWindow = _app.GetMainWindow(_automation);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _automation?.Dispose();
            _app?.Close();

            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }
        }

        private void CreateOrder(string customerName, string description, DateTime dateTime)
        {
            var customerNameTextBox = _mainWindow.FindFirstDescendant(x => x.ByName("customerNameTextBox"))?.AsTextBox();
            var descriptionTextBox = _mainWindow.FindFirstDescendant(x => x.ByName("descriptionTextBox"))?.AsTextBox();
            var creationDatePicker = _mainWindow.FindFirstDescendant(x => x.ByName("creationDatePicker"))?.AsDateTimePicker();
            var addOrderButton = _mainWindow.FindFirstDescendant(x => x.ByName("addOrderButton"))?.AsButton();

            customerNameTextBox.Enter(customerName);
            descriptionTextBox.Enter(description);
            creationDatePicker.SelectedDate = dateTime;
            addOrderButton.Click();
        }

        [TestMethod]
        public void TestMethod001()
        {
            var customerName = "Анатолий";
            var description = "заказ 2";
            DateTime dateTime = new DateTime(2025, 10, 15);

            CreateOrder(customerName, description, dateTime);

            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var lastOrder = orderList?.Items.Last()?.Text;
            Assert.IsTrue(lastOrder?.Contains(customerName));
            Assert.IsTrue(lastOrder?.Contains(description));
            Assert.IsTrue(lastOrder?.Contains(dateTime.Year.ToString()));
            Assert.IsTrue(lastOrder?.Contains(dateTime.Day.ToString()));
            Assert.IsTrue(lastOrder?.Contains(dateTime.Month.ToString()));

            TestCleanup();
        }

        [TestMethod]
        public void TestMethod002()
        {
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var lengthBefore = orderList?.Items.Count();
            var description = "Срочный заказ";
            DateTime dateTime = new DateTime(2025, 10, 15);

            var descriptionTextBox = _mainWindow.FindFirstDescendant(x => x.ByName("descriptionTextBox"))?.AsTextBox();
            var creationDatePicker = _mainWindow.FindFirstDescendant(x => x.ByName("creationDatePicker"))?.AsDateTimePicker();
            var addOrderButton = _mainWindow.FindFirstDescendant(x => x.ByName("addOrderButton"))?.AsButton();

            descriptionTextBox.Enter(description);
            creationDatePicker.SelectedDate = dateTime;
            addOrderButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            string message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();
            var lengthAfter = orderList?.Items.Count();
            Assert.AreEqual("Заполните все поля!", message);
            Assert.AreEqual(lengthBefore, lengthAfter);

            TestCleanup();
        }

        [TestMethod]
        public void TestMethod003()
        {
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var lengthBefore = orderList?.Items.Count();
            var customerName = "Анатолий";
            DateTime dateTime = new DateTime(2025, 10, 15);

            var customerNameTextBox = _mainWindow.FindFirstDescendant(x => x.ByName("customerNameTextBox"))?.AsTextBox();
            var creationDatePicker = _mainWindow.FindFirstDescendant(x => x.ByName("creationDatePicker"))?.AsDateTimePicker();
            var addOrderButton = _mainWindow.FindFirstDescendant(x => x.ByName("addOrderButton"))?.AsButton();

            customerNameTextBox?.Enter(customerName);
            creationDatePicker.SelectedDate = dateTime;
            addOrderButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            string message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();
            var lengthAfter = orderList?.Items.Count();
            Assert.AreEqual("Заполните все поля!", message);
            Assert.AreEqual(lengthBefore, lengthAfter);

            TestCleanup();
        }

        [TestMethod]
        public void TestMethod004()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var lengthBefore = orderList?.Items.Count();
            var removeOrderButton = _mainWindow.FindFirstDescendant(x => x.ByName("removeOrderButton"))?.AsButton();

            orderList?.Select(0);
            removeOrderButton.Click();

            var lengthAfter = orderList?.Items.Count();
            Assert.AreEqual(lengthBefore - 1, lengthAfter);

            TestCleanup();
        }

        [TestMethod]
        public void TestMethod005()
        {
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var lengthBefore = orderList?.Items.Count();
            var removeOrderButton = _mainWindow.FindFirstDescendant(x => x.ByName("removeOrderButton"))?.AsButton();

            removeOrderButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            string message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();
            var lengthAfter = orderList?.Items.Count();
            Assert.AreEqual("Выберите заказ для удаления!", message);
            Assert.AreEqual(lengthBefore, lengthAfter);

            TestCleanup();
        }

        [TestMethod]
        public void TestMethod006()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();

            orderList.Select(0);
            statusComboBox.Click();
            statusComboBox.Select("В обработке");
            updateStatusButton.Focus();
            updateStatusButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();
            orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            Assert.IsTrue(orderList?.Items[0]?.Text.Contains("В_обработке"));
        }

        [TestMethod]
        public void TestMethod007()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var orderBefore = orderList?.Items[0]?.Text;
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();

            orderList.Select(0);
            updateStatusButton.Focus();
            updateStatusButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            string message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();
            orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            Assert.IsTrue(message.Contains("Выберите статус для обновления статуса заказа"));
            Assert.IsTrue(orderList?.Items[0]?.Text.Contains(orderBefore));
        }


        [TestMethod]
        public void TestMethod008()
        {
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();

            statusComboBox.Click();
            statusComboBox.Select("В обработке");
            updateStatusButton.Focus();
            updateStatusButton.Click();

            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            string message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();

            Assert.IsTrue(message.Contains("Выберите заказ для обновления статуса!"));
        }

        [TestMethod]
        public void TestMethod012()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var order = orderList?.Items[0]?.Text.Trim();
            string[] parts = order.Split(new[] { '-', '(', ')' }, StringSplitOptions.None);
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();

            orderList.Select(0);
            statusComboBox.Click();
            statusComboBox.Select("Завершён");
            updateStatusButton.Focus();
            updateStatusButton.Click();
            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();

            Assert.IsTrue(message.Contains($"Заказ '{parts[0].Trim()} - {parts[1].Trim()} {parts[3].Trim()}' завершен"));
        }

        [TestMethod]
        public void TestMethod013()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var order = orderList?.Items[0]?.Text.Trim();
            string[] parts = order.Split(new[] { '-', '(', ')' }, StringSplitOptions.None);
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();

            orderList.Select(0);
            statusComboBox.Click();
            statusComboBox.Select("В обработке");
            updateStatusButton.Focus();
            updateStatusButton.Click();
            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();
            var messageText = messageBox?.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var message = messageText?.Name;
            var okButton = messageBox?.FindFirstDescendant(x => x.ByAutomationId("2")).AsButton();
            okButton?.Click();

            Assert.IsTrue(message.Contains($"Заказ '{parts[0].Trim()} - {parts[1].Trim()} {parts[3].Trim()}' взят в обработку"));
        }

        [TestMethod]
        public void TestMethod014()
        {
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();
            var notifyOnInProgressCheckBox = _mainWindow?.FindFirstDescendant(x => x.ByName("notifyOnInProgressCheckBox"))?.AsCheckBox();

            notifyOnInProgressCheckBox.IsChecked = false;
            orderList.Select(0);
            statusComboBox.Click();
            statusComboBox.Select("В обработке");
            updateStatusButton.Focus();
            updateStatusButton.Click();
            var messageBox = _mainWindow?.ModalWindows.FirstOrDefault();

            Assert.IsNull(messageBox);
        }

        [TestMethod]
        public void TestMethod015()
        {
            // Подготовка заказа со статусом "В обработке"
            CreateOrder("Анатолий", "Срочный заказ", new DateTime(2025, 10, 15));
            var orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            var updateStatusButton = _mainWindow?.FindFirstDescendant(x => x.ByName("updateStatusButton"))?.AsButton();
            var statusComboBox = _mainWindow?.FindFirstDescendant(x => x.ByName("statusComboBox"))?.AsComboBox();
            orderList.Select(0);
            statusComboBox.Click();
            statusComboBox.Select("В обработке");
            Console.WriteLine($"Выбран элемент: {statusComboBox.Items[1].Text}");
            statusComboBox.Collapse();
            System.Threading.Thread.Sleep(300);

            updateStatusButton.Focus();
            updateStatusButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Проверяем, что появилось уведомление об изменении статуса
            var messageBox1 = _mainWindow?.ModalWindows.FirstOrDefault();
            Assert.IsNotNull(messageBox1, "Должно появиться уведомление при первом изменении статуса");

            // Закрываем уведомление
            var okButton = messageBox1?.FindFirstDescendant(x => x.ByAutomationId("2"))?.AsButton();
            okButton?.Click();
            System.Threading.Thread.Sleep(500);

            // ПРОВЕРКА: убеждаемся, что статус действительно изменился
            orderList = _mainWindow?.FindFirstDescendant(x => x.ByName("ordersListBox"))?.AsListBox();
            string statusAfterFirstChange = orderList?.Items[0]?.Text ?? "";
            Console.WriteLine($"Статус после первого изменения: {statusAfterFirstChange}");
            Assert.IsTrue(statusAfterFirstChange.Contains("В_обработке"),
                "Статус должен быть 'В_обработке' после первого изменения");

            // Теперь пробуем изменить на тот же статус
            orderList.Select(0);
            System.Threading.Thread.Sleep(500);

            // Снова проверяем элементы ComboBox
            statusComboBox.Click();
            System.Threading.Thread.Sleep(300);

            Console.WriteLine($"Количество элементов в ComboBox (второй раз): {statusComboBox.Items.Length}");

            // Снова проверяем границы
            if (statusComboBox.Items.Length < 2)
            {
                Assert.Fail($"В ComboBox недостаточно элементов при второй попытке. Найдено: {statusComboBox.Items.Length}");
                return;
            }

            // Выбираем снова "В обработке"
            statusComboBox.Items[1].Select();
            statusComboBox.Collapse();
            System.Threading.Thread.Sleep(300);

            updateStatusButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Проверяем, что уведомление НЕ появилось
            var messageBox2 = _mainWindow?.ModalWindows.FirstOrDefault();

            Assert.IsNull(messageBox2,
                "Уведомление не должно появляться при изменении на тот же статус");
        }
    }
}