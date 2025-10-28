using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OrderManager
{
    public partial class OrderForm : Form
    {
        private OrderManager orderManager;
        private NotificationManager notificationManager;
        private Label customerNameLabel;
        private TextBox customerNameTextBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label dateLabel;
        private DateTimePicker creationDatePicker;
        private Label statusLabel;
        private ComboBox statusComboBox;
        private Button addOrderButton;
        private Button removeOrderButton;
        private Button updateStatusButton;
        private Label ordersListLabel;
        private ListBox ordersListBox; 
        private Label notificationLabel;
        private CheckBox notifyOnInProgressCheckbox;
        private CheckBox notifyOnCompletedCheckbox;
        public OrderForm()
        {
            this.Text = "Управление заказами";
            this.Width = 750;
            this.Height = 500;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            customerNameLabel = new Label
            {
                Location = new System.Drawing.Point(10, 10),
                Text = "Имя клиента",
                AccessibleName = "customerNameLabel"
            };
            customerNameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 150,
                AccessibleName = "customerNameTextBox",
            };
            descriptionLabel = new Label
            {
                Location = new System.Drawing.Point(170, 10),
                Text = "Описание",
                AccessibleName = "descriptionLabel"
            };
            descriptionTextBox = new TextBox
            {
                Location = new System.Drawing.Point(170, 40),
                Width = 200,
                AccessibleName = "descriptionTextBox"
            };
            dateLabel = new Label
            {
                Location = new System.Drawing.Point(380, 10),
                Text = "Дата создания заказа:",
                Width = 130,
                AccessibleName = "dateLabel"
            };
            creationDatePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(380, 40),
                AccessibleName = "creationDatePicker"
            };
            addOrderButton = new Button
            {
                Location = new System.Drawing.Point(10, 70),
                Text = "Добавить",
                Width = 100,
                AccessibleName = "addOrderButton"
            };
            addOrderButton.Click += AddOrderButton_Click;
            removeOrderButton = new Button
            {
                Location = new System.Drawing.Point(120, 70),
                Text = "Удалить",
                Width = 100,
                AccessibleName = "removeOrderButton"
            };
            removeOrderButton.Click += RemoveOrderButton_Click;
            updateStatusButton = new Button
            {
                Location = new System.Drawing.Point(220, 70),
                Text = "Обновить статус",
                Width = 120,
                AccessibleName = "updateStatusButton"
            };
            updateStatusButton.Click += UpdateStatusButton_Click;
            statusLabel = new Label
            {
                Location = new System.Drawing.Point(370, 75),
                Text = "Статус",
                AccessibleName = "statusLabel"
            };
            statusComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(370, 100),
                Width = 100,
                Items = { "Новый", "В обработке", "Завершён" },
                DropDownStyle = ComboBoxStyle.DropDownList,
                AccessibleName = "statusComboBox"
            };
            ordersListLabel = new Label
            {
                Location = new System.Drawing.Point(10, 100),
                Text = "Список заказов:",
                AccessibleName = "ordersListLabel"
            };
            ordersListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 130),
                Width = 660,
                Height = 300,
                AccessibleName = "ordersListBox"
            };
            notificationLabel = new Label
            {
                Text = "Включить уведомления для статусов: ",
                Location = new System.Drawing.Point(500, 70),
                Width = 300,
                AccessibleName = "notificationLabel"
            };
            notifyOnInProgressCheckbox = new CheckBox
            {
                Location = new System.Drawing.Point(500, 90),
                Text = "В обработке",
                Width = 100,
                Checked = true,
                AccessibleName = "notifyOnInProgressCheckBox"
            };
            notifyOnInProgressCheckbox.Click += notifyOnInProgressCheckbox_Click;

            notifyOnCompletedCheckbox = new CheckBox
            {
                Location = new System.Drawing.Point(600, 90),
                Text = "Завершен",
                Checked = true,
                AccessibleName = "notifyOnCompletedCheckBox"
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
            this.Controls.Add(statusLabel);
            this.Controls.Add(statusComboBox);
            this.Controls.Add(ordersListLabel);
            this.Controls.Add(ordersListBox);
            this.Controls.Add(ordersListBox);
            this.Controls.Add(notificationLabel);
            this.Controls.Add(notifyOnInProgressCheckbox);
            this.Controls.Add(notifyOnCompletedCheckbox);
            orderManager = new OrderManager();
            notificationManager = new NotificationManager();
            UpdateOrdersList();
        }
        private void UpdateOrdersList()
        {
            ordersListBox.Items.Clear();
            foreach (var order in orderManager.Orders)
            {
                ordersListBox.Items.Add($"{order.CustomerName} - {order.Description} ({order.Status}) {order.CreationDate}");
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
            string[] parts = selectedItem.Split(new[] { '-', '(', ')' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string customerName = parts[0].Trim();
                string description = parts[1].Trim();
                try
                {
                    DateTime creationDate;
                    if (!DateTime.TryParse(parts[3].Trim(), out creationDate)) throw new Exception("Ошибка формата даты!");
                    var orderToRemove = orderManager.Orders.Find(o => o.CustomerName ==
                    customerName && o.Description == description && o.CreationDate == creationDate);
                    if (orderToRemove != null)
                    {
                        orderManager.RemoveOrder(orderToRemove);
                        UpdateOrdersList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
            if (statusComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите статус для обновления статуса заказа!");
                return;
            }
            string selectedItem = ordersListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-', '(', ')' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string customerName = parts[0].Trim();
                string description = parts[1].Trim();
                try
                {
                    DateTime creationDate;
                    if (!DateTime.TryParse(parts[3].Trim(), out creationDate)) throw new Exception("Ошибка формата даты!");
                    var orderToUpdate = orderManager.Orders.Find(o => o.CustomerName == customerName && o.Description == description && o.CreationDate == creationDate);
                    if (orderToUpdate != null)
                    {
                        OrderStatus oldStatus = orderToUpdate.Status;
                        string str_status = statusComboBox.SelectedItem.ToString().Trim().Replace(' ', '_');
                        OrderStatus newStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), str_status);
                        if (oldStatus != newStatus)
                        {
                            orderManager.UpdateOrderStatus(orderToUpdate, newStatus);
                            UpdateOrdersList();

                            string message = notificationManager.NotifyStatusChange(orderToUpdate, oldStatus, newStatus);

                            if (message != string.Empty)
                            {
                                MessageBox.Show(message, "Изменение в статусе заказа");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void notifyOnInProgressCheckbox_Click(object sender, EventArgs e)
        {
            notificationManager.NotifyOnInProgress = !notificationManager.NotifyOnInProgress;
        }

        private void notifyOnCompletedCheckbox_Click(object sender, EventArgs e)
        {
            notificationManager.NotifyOnCompleted = !notificationManager.NotifyOnCompleted;
        }
    }
}
