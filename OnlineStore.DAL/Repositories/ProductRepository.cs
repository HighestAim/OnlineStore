using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.Repositories
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {
        public ProductRepository(OnlineStoreContext context) : base(context)
        {
        }
    }
}
