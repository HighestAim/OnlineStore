using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models.JsonModels
{
    public class ProductJsonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public long CategorySeederId { get; set; }
        public long SeederId { get; set; }
    }
}
