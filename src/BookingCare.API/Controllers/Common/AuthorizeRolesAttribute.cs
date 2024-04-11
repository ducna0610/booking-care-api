using Microsoft.AspNetCore.Authorization;

namespace BookingCare.API.Controllers.Common
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            if (roles != null)
                Roles = string.Join(",", roles);
        }
    }
}
