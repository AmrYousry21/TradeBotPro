using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.ViewModels;
using TradeBotPro.App.Utilities;

namespace TradeBotPro.App.Controllers
{
    [Authorize]
    public class HistoryController : BaseController
    {
        private readonly DatabaseContext _dbContext;

        public HistoryController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Get Client Names
            var clients = _dbContext.Clients
                .Select(x => x.Name)
                .ToList();

            // Get User Names
            var users = _dbContext.Users
                .Include(x => x.Client)
                .Where(x => User.IsInRole(UserRoles.SystemAdmin) || x.Client.Id == ClientId)
                .Select(x => x.Email)
                .ToList();

            // Get Orders
            var orders = _dbContext.Orders
                .Include(x => x.User)
                .ThenInclude(x => x.Client)
                .Where(x => (User.IsInRole(UserRoles.SystemAdmin) || x.User.Client.Id == ClientId)
                         && (!User.IsInRole(UserRoles.ClientUser) || x.User.Id == UserId))
                .OrderByDescending(x => x.EntryTime)
                .ToList();

            // Get Closures
            var closures = _dbContext.Closures
                .Include(x => x.User)
                .ThenInclude(x => x.Client)
                .Where(x => (User.IsInRole(UserRoles.SystemAdmin) || x.User.Client.Id == ClientId)
                         && (!User.IsInRole(UserRoles.ClientUser) || x.User.Id == UserId))
                .OrderByDescending(x => x.PositionId)
                .ToList();

            return View(new HistoryViewModel
            {
                Clients = clients,
                Users = users,
                Orders = orders,
                Closures = closures
            });
        }
    }
}
