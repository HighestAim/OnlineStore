using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Operations
{
    public class CategoryOperations : ICategoryOperations
    {
        private readonly IRepositoryManager _repositoryManager;

        public CategoryOperations(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            var categories = await _repositoryManager.Categories.FindAsync();


            return categories;
        }

        public Task<IList<Product>> GetProductByCategoryIdAsync(long id)
        {
            return _repositoryManager.Products.FindAsync(x => x.CategoryId == id);
        }
    }
}
