using OnlineStore.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Entites
{
    public class OrderProduct: EntityBase
    {
        public long ProductId { get; set; }
        public long OrderId { get; set; }
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
