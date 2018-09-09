using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OnlineStoreContext context) : base(context)
        {
        }
    }
}
