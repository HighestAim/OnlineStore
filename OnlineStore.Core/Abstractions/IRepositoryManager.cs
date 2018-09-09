using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Abstractions
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IOrderProductRepository OrderProducts { get; }
        IOrderRepository Orders { get; }

        Task<int> CompleteAsync();
        int Complete();
        IDbTransaction BeginTransaction();
    }
}
