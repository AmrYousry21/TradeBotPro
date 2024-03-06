using System.ComponentModel.DataAnnotations;

namespace TradeBotPro.App.Models.FormModels;

public class RegisterFormModel
{
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(50, ErrorMessage = "First Name must not exceed 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(50, ErrorMessage = "First Name must not exceed 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,16}$",
        ErrorMessage = "Password must have minimum 8 and maximum 16 characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Registration Key is required")]
    [StringLength(32, ErrorMessage = "The Registration Key must be a 32 characters")]
    public string RegistrationKey { get; set; }
}
