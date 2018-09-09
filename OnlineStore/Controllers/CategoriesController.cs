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
    public class CategoriesController : Controller
    {
        private readonly ICategoryOperations _categoryOperations;
        private readonly TokenAuthentification _tokenAuthentication;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryOperations categoryOperations, IOptions<TokenAuthentification> tokenAuthentication, IMapper mapper)
        {
            _categoryOperations = categoryOperations;
            _tokenAuthentication = tokenAuthentication.Value;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Response> GetCategories()
        {
            var categories = await _categoryOperations.GetAllAsync();

            var result = _mapper.Map<CategoryViewModel[]>(categories.ToArray());

            return new Response
            {
                Result = categories,
                Status = ResponseStatus.Ok,
            };
        }

        [AllowAnonymous]
        [HttpGet("{id}/Products")]
        public async Task<Response> GetProductByCategoryId(long id)
        {
            var products = await _categoryOperations.GetProductByCategoryIdAsync(id);

            var result = _mapper.Map<ProductViewModel[]>(products.ToArray());

            return new Response
            {
                Status = ResponseStatus.Ok,
                Result = result
            };
        }
    }
}