namespace TradeBotPro.App.Models.ViewModels;

public class DashboardSummaryViewModel
{
    public int ClientCount { get; set; }
    public int UserCount { get; set; }
    public int TotalClosedOrders { get; set; }
    public double NetProfit { get; set; }
    public string TradeCountByTradeType { get; set; }
    public string TradeVolumeByTradeType { get; set; }
    public string YearlyProfit { get; set; }
}