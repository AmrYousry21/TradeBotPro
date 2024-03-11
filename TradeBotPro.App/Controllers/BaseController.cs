using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TradeBotPro.App.Controllers
{
    public class BaseController : Controller
    {
        protected Guid? UserId
        {
            get
            {
                var userId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrWhiteSpace(userId) ? null : new Guid(userId);
            }
        }

        protected Guid? ClientId
        {
            get
            {
                var clientId = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
                return string.IsNullOrWhiteSpace(clientId) ? null : new Guid(clientId);
            }
        }
    }
}
