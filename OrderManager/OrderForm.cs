using System;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class OrderForm : Form
    {
        private OrderManager orderManager;
        private Label customerNameLabel;
        private TextBox customerNameTextBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label dateLabel;
        private DateTimePicker creationDatePicker;
        private ComboBox statusComboBox;
        private Button addOrderButton;
        private Button removeOrderButton;
        private Button updateStatusButton;
        private Label ordersListLabel;
        private ListBox ordersListBox;
        public OrderForm()
        {
            this.Text = "Управление заказами";
            this.Width = 600;
            this.Height = 500;
            customerNameLabel = new Label
            {
                Location = new System.Drawing.Point(10, 10),
                Text = "Имя клиента"
            };
            customerNameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 150
            };
            descriptionLabel = new Label
            {
                Location = new System.Drawing.Point(170, 10),
                Text = "Описание"
            };
            descriptionTextBox = new TextBox
            {
                Location = new System.Drawing.Point(170, 40),
                Width = 200
            };
            dateLabel = new Label
            {
                Location = new System.Drawing.Point(380, 10),
                Text = "Дата создания заказа:",
                Width = 130,
            };
            creationDatePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(380, 40)
            };
            addOrderButton = new Button
            {
                Location = new System.Drawing.Point(10, 70),
                Text = "Добавить",
                Width = 100
            };
            addOrderButton.Click += AddOrderButton_Click;
            removeOrderButton = new Button
            {
                Location = new System.Drawing.Point(120, 70),
                Text = "Удалить",
                Width = 100
            };
            removeOrderButton.Click += RemoveOrderButton_Click;
            updateStatusButton = new Button
            {
                Location = new System.Drawing.Point(220, 70),
                Text = "Обновить статус",
                Width = 120
            };
            updateStatusButton.Click += UpdateStatusButton_Click;
            statusComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(340, 70),
                Width = 100,
                Items = { "Новый", "В обработке", "Завершён" }
            };
            ordersListLabel = new Label
            {
                Location = new System.Drawing.Point(10, 100),
                Text = "Список заказов:",
            };

            ordersListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 130),
                Width = 560,
                Height = 300
            };
            this.Controls.Add(customerNameTextBox);
            this.Controls.Add(customerNameLabel);
            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(dateLabel);
            this.Controls.Add(creationDatePicker);
            this.Controls.Add(addOrderButton);
            this.Controls.Add(removeOrderButton);
            this.Controls.Add(updateStatusButton);
            this.Controls.Add(statusComboBox);
            this.Controls.Add(ordersListLabel);
            this.Controls.Add(ordersListBox);
            orderManager = new OrderManager();
            UpdateOrdersList();
        }
        private void UpdateOrdersList()
        {
            ordersListBox.Items.Clear();
            foreach (var order in orderManager.Orders)
            {
                ordersListBox.Items.Add($"{order.CustomerName} - {order.Description} ({order.Status})");
            }
        }
        private void AddOrderButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(customerNameTextBox.Text) ||
            string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            DateTime creationDate = creationDatePicker.Value;
            Order newOrder = new Order(customerNameTextBox.Text, descriptionTextBox.Text,
            creationDate);
            try
            {
                orderManager.AddOrder(newOrder);
                customerNameTextBox.Clear();
                descriptionTextBox.Clear();
                UpdateOrdersList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveOrderButton_Click(object sender, EventArgs e)
        {
            if (ordersListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заказ для удаления!");
                return;
            }
            string selectedItem = ordersListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string customerName = parts[0].Trim();
                string description = parts[1].Trim();
                var orderToRemove = orderManager.Orders.Find(o => o.CustomerName ==
                customerName && o.Description == description);
                if (orderToRemove != null)
                {
                    try
                    {
                        orderManager.RemoveOrder(orderToRemove);
                        UpdateOrdersList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void UpdateStatusButton_Click(object sender, EventArgs e)
        {
            if (ordersListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заказ для обновления статуса!");
                return;
            }
            string selectedItem = ordersListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string customerName = parts[0].Trim();
                string description = parts[1].Trim();
                var orderToUpdate = orderManager.Orders.Find(o => o.CustomerName ==
                customerName && o.Description == description);
                if (orderToUpdate != null)
                {
                    OrderStatus newStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus),
                    statusComboBox.SelectedItem.ToString());
                    try
                    {
                        orderManager.UpdateOrderStatus(orderToUpdate, newStatus);
                        UpdateOrdersList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
