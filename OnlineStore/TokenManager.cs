using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineStore.API
{
    public static class TokenManager
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;

            return Convert.ToInt32(identity.Name);
        }
    }
}
