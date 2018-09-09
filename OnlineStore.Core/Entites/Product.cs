using OnlineStore.Core.Abstractions;
using System.Collections.Generic;

namespace OnlineStore.Core.Entites
{
    public class Product: EntityBaseWithId
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public long SeederId { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
