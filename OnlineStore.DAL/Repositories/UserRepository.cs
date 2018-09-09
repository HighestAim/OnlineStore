using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.DAL.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(OnlineStoreContext context) : base(context)
        {
        }
    }
}
