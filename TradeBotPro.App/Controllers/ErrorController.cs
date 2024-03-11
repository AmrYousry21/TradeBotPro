using Microsoft.AspNetCore.Mvc;

namespace TradeBotPro.App.Controllers
{
    public class ErrorController : BaseController
    {
        public IActionResult Index(string error)
        {
            return View("Error", new ErrorViewModel().AddError(error));
        }
    }
}
