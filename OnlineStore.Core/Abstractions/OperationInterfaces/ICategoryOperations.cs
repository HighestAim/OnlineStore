using OnlineStore.Core.Entites;
using OnlineStore.Core.Models;
using OnlineStore.Core.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Abstractions.OperationInterfaces
{
    public interface ICategoryOperations
    {
        Task<IList<Category>> GetAllAsync();
        Task<IList<Product>> GetProductByCategoryIdAsync(long id);
    }
}
