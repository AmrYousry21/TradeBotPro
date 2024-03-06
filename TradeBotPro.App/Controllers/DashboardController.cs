using TradeBotPro.App;
using TradeBotPro.App.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TradeBotPro.App.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public DashboardController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(new DashboardSummaryViewModel());
        }
    }
}
