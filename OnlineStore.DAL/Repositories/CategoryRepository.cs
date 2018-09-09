using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.Repositories
{
    public class CategoryRepository : EntityBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(OnlineStoreContext context) : base(context)
        {
        }
    }
}
