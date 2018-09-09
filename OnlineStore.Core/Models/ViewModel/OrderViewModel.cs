using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models.ViewModel
{
    public class OrderViewModel
    {
        public IEnumerable<OrderProductViewModel> OrderProducts { get; set; }
    }
}
