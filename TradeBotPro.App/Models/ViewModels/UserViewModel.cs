using TradeBotPro.App.Models.DataModels;

namespace TradeBotPro.App.Models.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserRole { get; set; }
    public UserStatusEnum Status { get; set; }
    public Guid? ClientId { get; set; }

    public static explicit operator UserViewModel(User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserRole = user.UserRole.ToString(),
            Status = user.Status,
            ClientId = user?.Client?.Id
        };
    }
}
