using TradeBotPro.App.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Utilities;
using Newtonsoft.Json;

namespace TradeBotPro.App.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DatabaseContext _dbContext;

        public DashboardController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Get Client Count
            var clientCount = _dbContext.Clients
                .Where(x => User.IsInRole(UserRoles.SystemAdmin) || x.Id == ClientId)
                .Count();

            // Get User Count
            var userCount = _dbContext.Users
                .Include(x => x.Client)
                .Where(x => User.IsInRole(UserRoles.SystemAdmin) || x.Client.Id == ClientId)
                .Count();

            // Get Trades
            var trades = _dbContext.Orders
                .Include(x => x.User)
                .ThenInclude(x => x.Client)
                .Where(x => (User.IsInRole(UserRoles.SystemAdmin) || x.User.Client.Id == ClientId)
                         && (!User.IsInRole(UserRoles.ClientUser) || x.User.Id == UserId))
                .ToList();

            // Get Closed Orders
            var closedOrders = _dbContext.Closures
                .Include(x => x.User)
                .ThenInclude(x => x.Client)
                .Where(x => (User.IsInRole(UserRoles.SystemAdmin) || x.User.Client.Id == ClientId)
                         && (!User.IsInRole(UserRoles.ClientUser) || x.User.Id == UserId))
                .ToList();

            var totalClosedOrders = closedOrders.Count();
            var netProfit = closedOrders.Sum(x => x.NetProfit);
            var years = trades.Select(x => x.EntryTime.Year).Distinct().OrderBy(x => x);
            var yearlyprofitData = new List<DataPoint>();
            foreach (var year in years)
            {
                yearlyprofitData.Add(new DataPoint
                {
                    label = year.ToString(),
                    y = closedOrders
                        .Where(x => trades
                            .Any(y => y.OrderId == x.OrderId && y.EntryTime.Year == year))
                        .Sum(x => x.NetProfit)
                });
            }

            return View(new DashboardSummaryViewModel
            {
                ClientCount = clientCount,
                UserCount = userCount,
                TotalClosedOrders = totalClosedOrders,
                NetProfit = Math.Round(netProfit, 2),
                TradeCountByTradeType = JsonConvert.SerializeObject(new List<DataPoint>
                {
                    new DataPoint("Buy", trades.Count(x => x.Type == "BUY")),
                    new DataPoint("Sell", trades.Count(x => x.Type == "SELL"))
                }),
                TradeVolumeByTradeType = JsonConvert.SerializeObject(new List<DataPoint>
                {
                    new DataPoint("Buy", closedOrders
                        .Where(x => trades
                            .Any(y => y.OrderId == x.OrderId && y.Type == "BUY"))
                        .Sum(x => x.VolumeInUnits)),
                    new DataPoint("Sell", closedOrders
                        .Where(x => trades
                            .Any(y => y.OrderId == x.OrderId && y.Type == "SELL"))
                        .Sum(x => x.VolumeInUnits))
                }),
                YearlyProfit = JsonConvert.SerializeObject(yearlyprofitData)
            });
        }
    }
}
