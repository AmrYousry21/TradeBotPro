namespace TradeBotPro.App.Models.ViewModels;

public class UserListViewModel
{
    public Guid ClientId { get; set; }
    public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
}
