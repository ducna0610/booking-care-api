using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookingCare.Application.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string IpAddress { get; }
        List<KeyValuePair<string, string>> Claims { get; set; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IpAddress = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
        }

        public string UserId { get; }
        public string IpAddress { get; }
        public List<KeyValuePair<string, string>> Claims { get; set; }
    }
}
