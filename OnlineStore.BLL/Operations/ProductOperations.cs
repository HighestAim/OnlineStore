using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Operations
{
    public class ProductOperations : IProductOperations
    {
        private readonly IRepositoryManager _repositoryManager;

        public ProductOperations(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IList<Product>> GetAllAsync()
        {
            var products = await _repositoryManager.Products.FindAsync();


            return products;
        }

        public async Task<Product> GetByIdAsync(long id)
        {
            var product = await _repositoryManager.Products.GetSingleAsync(x => x.Id == id);


            return product;
        }
    }
}
