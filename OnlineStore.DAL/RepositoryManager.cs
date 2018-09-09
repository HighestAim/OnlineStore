using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineStore.DAL
{
    public class RepositoryManager:IRepositoryManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OnlineStoreContext _context;

        public RepositoryManager(IServiceProvider serviceProvider, OnlineStoreContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        private IUserRepository _users;
        public IUserRepository Users => _users ?? (_users = _serviceProvider.GetService<IUserRepository>());

        private IProductRepository _product;
        public IProductRepository Products => _product ?? (_product = _serviceProvider.GetService<IProductRepository>());

        private IOrderRepository _order;
        public IOrderRepository Orders => _order ?? (_order = _serviceProvider.GetService<IOrderRepository>());

        private IOrderProductRepository _orderProduct;
        public IOrderProductRepository OrderProducts => _orderProduct ?? (_orderProduct = _serviceProvider.GetService<IOrderProductRepository>());

        private ICategoryRepository _category;
        public ICategoryRepository Categories => _category ?? (_category = _serviceProvider.GetService<ICategoryRepository>());

        public IDbTransaction BeginTransaction() => new EntityDbTransaction(_context);
        public int Complete() => _context.SaveChanges();
        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
    }
}
