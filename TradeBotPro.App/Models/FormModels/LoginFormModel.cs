using System.ComponentModel.DataAnnotations;

namespace TradeBotPro.App.Models.FormModels;

public class LoginFormModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
