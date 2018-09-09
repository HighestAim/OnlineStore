using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Core.Models;
using OnlineStore.Core.Enums;
using Microsoft.Extensions.Options;
using OnlineStore.Core.Models.ViewModel;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductOperations _productOperations;
        private readonly TokenAuthentification _tokenAuthentication;
        private readonly IMapper _mapper;

        public ProductsController(IProductOperations productOperations, IOptions<TokenAuthentification> tokenAuthentication, IMapper mapper)
        {
            _productOperations = productOperations;
            _tokenAuthentication = tokenAuthentication.Value;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Response> GetProducts()
        {
            var products = await _productOperations.GetAllAsync();

            var result = _mapper.Map<ProductViewModel[]>(products.ToArray());

            return new Response
            {
                Result = products,
                Status = ResponseStatus.Ok,
            };
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<Response> GetProductById(long id)
        {
            var product = await _productOperations.GetByIdAsync(id);

            var result = _mapper.Map<ProductViewModel>(product);

            return new Response
            {
                Status = ResponseStatus.Ok,
                Result = result
            };
        }
    }
}