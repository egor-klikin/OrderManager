namespace OrderManager
{
    public class NotificationManager
    {
        public bool NotifyOnCompleted { get; set; } = true;
        public bool NotifyOnInProgress { get; set; } = true;

        public NotificationManager() { }

        public string NotifyStatusChange(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            string message = "";

            if (oldStatus != newStatus)
            {
                if (newStatus == OrderStatus.Завершён && NotifyOnCompleted)
                {
                    message = $"Заказ '{order.CustomerName} - {order.Description} {order.CreationDate}' завершен";
                }
                else if (newStatus == OrderStatus.В_обработке && NotifyOnInProgress)
                {
                    message = $"Заказ '{order.CustomerName} - {order.Description} {order.CreationDate}' взят в обработку";
                }
            }

            return message;
        }
    }
}