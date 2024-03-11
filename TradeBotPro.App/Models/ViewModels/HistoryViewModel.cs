using TradeBotPro.App.Models.DataModels;

namespace TradeBotPro.App.Models.ViewModels;

public class HistoryViewModel
{
    public List<string> Clients { get; set; }
    public List<string> Users { get; set; }
    public List<Order> Orders { get; set; }
    public List<Closure> Closures { get; set; }
}