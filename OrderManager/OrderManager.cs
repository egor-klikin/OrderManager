using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OrderManager
{
    public class OrderManager
    {
        public List<Order> Orders { get; private set; }
        public OrderManager()
        {
            Orders = new List<Order>();
            LoadOrders();
        }
        public void AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            Orders.Add(order);
            SaveOrders();
        }
        public void RemoveOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            Orders.Remove(order);
            SaveOrders();
        }
        public void UpdateOrderStatus(Order order, OrderStatus newStatus)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            order.UpdateStatus(newStatus);
            SaveOrders();
        }
        private void SaveOrders()
        {
            File.WriteAllLines("orders.txt", Orders.Select(o =>
            $"{o.CustomerName}|{o.Description}|{(int)o.Status}|{o.CreationDate.ToString("yyyy-MM-dd HH: mm:ss")}"));
        }
        private void LoadOrders()
        {
            if (File.Exists("orders.txt"))
            {
                var lines = File.ReadAllLines("orders.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        OrderStatus status = (OrderStatus)Enum.Parse(typeof(OrderStatus), parts[2]);
                        DateTime date;
                        if (DateTime.TryParse(parts[3], out date))
                        {
                            Orders.Add(new Order(parts[0], parts[1], date));
                            Orders.Last().Status = status;
                        }
                    }
                }
            }
        }
    }
}
