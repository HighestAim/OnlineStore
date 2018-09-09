using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Core.Models.ViewModel;
using OnlineStore.Core.Models;
using OnlineStore.Core.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using OnlineStore.Core.Entites;
using AutoMapper;

namespace OnlineStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserOperations _userOperations;
        private readonly TokenAuthentification _tokenAuthentication;
        private readonly IMapper _mapper;
        public UsersController(IUserOperations userOperations, IOptions<TokenAuthentification> tokenAuthentication, IMapper mapper)
        {
            _userOperations = userOperations;
            _tokenAuthentication = tokenAuthentication.Value;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<Response> Authenticate([FromBody]LoginViewModel userModel)
        {
            var user = await _userOperations.AuthenticateAsync(userModel.Username, userModel.Password);

            if (user == null)
                return new Response
                {
                    ErrorMessage = "Username or password is incorrect",
                    Status = ResponseStatus.Error,
                };

            var userToken = GenerateUserToken(user);

            var result = new UserAuthenticationModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = userToken
            };

            return new Response
            {
                Result = result,
                Status = ResponseStatus.Ok,
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Response> Register([FromBody]RegistrationViewModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            
            await _userOperations.CreateAsync(user, userModel.Password);

            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet]
        public async Task<Response> GetAll()
        {
            var users = await _userOperations.GetAllAsync();
            var result = _mapper.Map<UserViewModel[]>(users.ToArray());
            
            return new Response
            {
                Status = ResponseStatus.Ok,
                Result = result
            };
        }

        [HttpGet("{id}")]
        public async Task<Response> GetById(int id)
        {
            var user = await _userOperations.GetByIdAsync(id);
            var result = _mapper.Map<UserViewModel>(user);

            return new Response
            {
                Result = result,
                Status = ResponseStatus.Ok
            };
        }

        [HttpPut]
        public async Task<Response> Update([FromBody]RegistrationViewModel userModel)
        {
            var userId = TokenManager.GetUserId(User);
            var user = _mapper.Map<User>(userModel);
            user.Id = 4;

            await _userOperations.UpdateAsync(user, userModel.Password);

            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id)
        {
            await _userOperations.DeleteAsync(id);
            return new Response
            {
                Status = ResponseStatus.Ok
            };
        }

        #region -- helper functions --
        private string GenerateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenAuthentication.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        #endregion
    }
}