using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Core.Models;
using OnlineStore.Core.Enums;
using Microsoft.Extensions.Options;
using OnlineStore.Core.Models.ViewModel;

namespace OnlineStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderOperations _orderOperations;
        private readonly TokenAuthentification _tokenAuthentication;

        public OrdersController(IOrderOperations orderOperations, IOptions<TokenAuthentification> tokenAuthentication)
        {
            _orderOperations = orderOperations;
            _tokenAuthentication = tokenAuthentication.Value;
        }

        [HttpPost]
        public async Task<Response> AddOrder([FromBody] OrderViewModel order)
        {
            var userId = TokenManager.GetUserId(User);

            await _orderOperations.AddOrder(order, userId);

            return new Response
            {
                Status = ResponseStatus.Ok,
            };
        }
    }
}