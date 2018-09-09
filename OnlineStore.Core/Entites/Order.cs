using OnlineStore.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Entites
{
    public class Order: EntityBaseWithId
    {
        public Order()
        {
            OrderDate = DateTime.UtcNow;
        }

        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public long UserId { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
        public User User { get; set; }
    }
}
