using System.ComponentModel.DataAnnotations;

namespace TradeBotPro.App.Models.FormModels;

public class ChangePasswordFormModel
{
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,16}$",
        ErrorMessage = "Password must have minimum 8 and maximum 16 characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
