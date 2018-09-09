using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Threading.Tasks;
using OnlineStore.Core.Models.ViewModel;
using System.Linq;

namespace OnlineStore.BLL.Operations
{
    public class OrderOperations : IOrderOperations
    {
        private readonly IRepositoryManager _repositoryManager;

        public OrderOperations(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task AddOrder(OrderViewModel order, long userId)
        {
            var productIds = order.OrderProducts.Select(x => x.ProductId);

            var products = await _repositoryManager.Products.FindAsync(x => productIds.Contains(x.Id));

            var orderProducts = from product in products
                                join orderProduct in order.OrderProducts on product.Id equals orderProduct.ProductId
                                select new OrderProduct
                                {
                                    UnitPrice = product.Price,
                                    ProductId = product.Id,
                                    Quantity = orderProduct.Quantity
                                };

            var orderTotalSum = orderProducts.Sum(x => x.Quantity * x.UnitPrice);

            var orderEntity = new Order
            {
                OrderProducts = orderProducts.ToList(),
                Total = orderTotalSum,
                UserId = userId,
            };

            _repositoryManager.Orders.Add(orderEntity);

            await _repositoryManager.CompleteAsync();
        }
    }
}
