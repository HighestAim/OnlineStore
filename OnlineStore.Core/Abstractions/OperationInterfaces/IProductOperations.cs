using OnlineStore.Core.Entites;
using OnlineStore.Core.Models;
using OnlineStore.Core.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Abstractions.OperationInterfaces
{
    public interface IProductOperations
    {
        Task<IList<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(long id);
    }
}
