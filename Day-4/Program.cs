using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderTrackingSystem
{
    class Program
    {
        static void Main()
        {
            List<Order> orders = new List<Order>
            {
                new Order { Id = 1 },
                new Order { Id = 2 },
                new Order { Id = 3 }
            };

            Kitchen kitchen = new Kitchen();
            Delivery delivery = new Delivery();
            CustomerService cskh = new CustomerService();

            Predicate<Order> isDelivering = o => o.Status == "Đang giao";
            Func<Order, string> describeOrder = o => $"[Mô tả] Đơn #{o.Id} hiện tại là '{o.Status}'";
            Action<string> logToConsole = msg => Console.WriteLine($"[LOG] {msg}");

            foreach (var order in orders)
            {
                kitchen.Subscribe(order);
                delivery.Subscribe(order);
                cskh.Subscribe(order);

                order.OrderStatusChanged += (sender, e) =>
                {
                    logToConsole(describeOrder(e.Order));
                };
            }

            orders[0].UpdateStatus("Mới");
            orders[0].UpdateStatus("Đang giao");
            orders[0].UpdateStatus("Hoàn tất");

            orders[1].UpdateStatus("Mới");
            orders[1].UpdateStatus("Hủy");

            orders[2].UpdateStatus("Mới");
            orders[2].UpdateStatus("Đang giao");
            orders[2].UpdateStatus("Giao thất bại");

            string logFile = "order-log.txt";
            System.IO.File.WriteAllText(logFile, ""); // Xóa file cũ nếu có

            foreach (var order in orders)
            {
                order.OrderStatusChanged += (sender, e) =>
                {
                    string logEntry = $"[LOG] {DateTime.Now:HH:mm:ss} - Đơn #{e.Order.Id} => Trạng thái mới: '{e.Order.Status}'";
                    Console.WriteLine(logEntry);
                    System.IO.File.AppendAllText(logFile, logEntry + Environment.NewLine);
                };
            }


            int thanhCong = orders.Count(o => o.Status == "Hoàn tất");
            int huy = orders.Count(o => o.Status == "Hủy");

            Console.WriteLine($"[Tổng kết] Thành công: {thanhCong} đơn, Bị hủy: {huy} đơn.");
        }
    }
}