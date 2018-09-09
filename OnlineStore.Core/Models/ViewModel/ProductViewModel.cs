using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models.ViewModel
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
    }
}
