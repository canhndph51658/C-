using System;

namespace OrderTrackingSystem
{
    public class Kitchen
    {
        public void Subscribe(Order order)
        {
            order.OrderStatusChanged += (sender, e) =>
            {
                if (e.NewStatus == "Mới")
                    Console.WriteLine($"[Kitchen] Chuẩn bị đơn hàng #{e.Order.Id}");
            };
        }
    }

    public class Delivery
    {
        public void Subscribe(Order order)
        {
            order.OrderStatusChanged += (sender, e) =>
            {
                if (e.NewStatus == "Đang giao")
                    Console.WriteLine($"[Delivery] Lấy đơn hàng #{e.Order.Id} để giao.");
            };
        }
    }

    public class CustomerService
    {
        public void Subscribe(Order order)
        {
            order.OrderStatusChanged += (sender, e) =>
            {
                if (e.NewStatus == "Hủy" || e.NewStatus == "Giao thất bại")
                    Console.WriteLine($"[CSKH] Xử lý đơn hàng #{e.Order.Id} bị {e.NewStatus.ToLower()}.");
            };
        }
    }
}