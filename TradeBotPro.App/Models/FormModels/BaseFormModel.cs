namespace TradeBotPro.App.Models.FormModels;

public class BaseFormModel
{
    public Guid Id { get; set; }
    public bool Edit => Id != Guid.Empty;
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
}
