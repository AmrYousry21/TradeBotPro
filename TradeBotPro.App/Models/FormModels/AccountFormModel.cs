using System.ComponentModel.DataAnnotations;
using TradeBotPro.App.Models.Attributes;
using TradeBotPro.App.Models.DataModels;

namespace TradeBotPro.App.Models.FormModels;

public class AccountFormModel : BaseFormModel
{
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(50, ErrorMessage = "First Name must not exceed 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(50, ErrorMessage = "Last Name must not exceed 50 characters")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public static explicit operator AccountFormModel(User user)
    {
        return new AccountFormModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}