using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.Repositories
{
    public class OrderProductRepository : EntityBaseRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(OnlineStoreContext context) : base(context)
        {
        }
    }
}
