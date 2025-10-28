using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager
{
    public enum OrderStatus
    {
        Новый,
        В_обработке,
        Завершён
    }
    public class Order
    {
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public Order(string customerName, string description, DateTime creationDate)
        {
            CustomerName = customerName;
            Description = description;
            Status = OrderStatus.Новый;
            CreationDate = creationDate;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
