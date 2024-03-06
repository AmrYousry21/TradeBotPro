using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeBotPro.App.Models.FormModels;

namespace TradeBotPro.App.Models.DataModels;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public UserStatusEnum Status { get; set; }

    [Required]
    public UserRoleEnum UserRole { get; set; }

    public Client Client { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public static explicit operator User(RegisterFormModel registerFormModel)
    {
        return new User
        {
            FirstName = registerFormModel.FirstName,
            LastName = registerFormModel.LastName,
            Email = registerFormModel.Email,
            UserRole = UserRoleEnum.User,
        };
    }
}

public enum UserRoleEnum
{
    Admin = 1,
    User = 2
}

public enum UserStatusEnum
{
    Active = 1,
    Inactive = 2,
    Suspended = 3
}