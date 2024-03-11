using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TradeBotPro.App.Models.Attributes;
using TradeBotPro.App.Models.DataModels;
using TradeBotPro.App.Utilities;

namespace TradeBotPro.App.Models.FormModels;

public class UserEditFormModel : BaseFormModel
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

    [RequiredIf(nameof(Edit), false, ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,16}$",
        ErrorMessage = "Password must have minimum 8 and maximum 16 characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
    public string Password { get; set; }
    public string Status { get; set; }
    public string UserRole { get; set; }

    [Required]
    public Guid ClientId { get; set; }

    public IEnumerable<SelectListItem> StatusList
    {
        get
        {
            return Enum.GetNames(typeof(UserStatusEnum))
                .Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                });
        }
    }

    public IEnumerable<SelectListItem> RoleList
    {
        get
        {
            return Enum.GetNames(typeof(UserRoleEnum))
                .Where(x => x != UserRoles.SystemAdmin)
                .Select(x => new SelectListItem
                {
                    Text = Regex.Split(x, @"(?<!^)(?=[A-Z])")[1],
                    Value = x
                });
        }
    }

    public static explicit operator UserEditFormModel(User user)
    {
        return new UserEditFormModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Status = user.Status.ToString(),
        };
    }
}
