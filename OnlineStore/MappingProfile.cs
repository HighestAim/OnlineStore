using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using OnlineStore.Core.Entites;
using OnlineStore.Core.Models.ViewModel;

namespace OnlineStore.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<RegistrationViewModel, User>();
            CreateMap<User, UserViewModel>();
        }
    }
}
