using OnlineStore.Core.Abstractions;
using System.Collections.Generic;

namespace OnlineStore.Core.Entites
{
    public class Category: EntityBaseWithId
    {
        public string Name { get; set; }
        public long SeederId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
