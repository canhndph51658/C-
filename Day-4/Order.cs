using System;

namespace OrderTrackingSystem
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; private set; }

        public event EventHandler<OrderEventArgs> OrderStatusChanged;

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
            OnOrderStatusChanged(new OrderEventArgs(this, newStatus));
        }

        protected virtual void OnOrderStatusChanged(OrderEventArgs e)
        {
            OrderStatusChanged?.Invoke(this, e);
        }

        public override string ToString()
        {
            return $"Order #{Id} - Trạng thái: {Status}";
        }
    }
}