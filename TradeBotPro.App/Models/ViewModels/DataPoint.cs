namespace TradeBotPro.App.Models.ViewModels;

public class DataPoint
{
    public string label { get; set; }
    public double y { get; set; }

    public DataPoint() { }
    public DataPoint(string label, double y)
    {
        this.label = label;
        this.y = y;
    }
}