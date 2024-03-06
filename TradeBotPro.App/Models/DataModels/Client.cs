using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeBotPro.App.Models.FormModels;

namespace TradeBotPro.App.Models.DataModels;

[Table("clients")]
public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [StringLength(32)]
    public string Registrationkey { get; set; }

    [Required]
    public ClientStatusEnum Status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<User> Users { get; set; }


    public static explicit operator Client(ClientEditFormModel clientEditFormModel)
    {
        return new Client
        {
            Id = clientEditFormModel.Id,
            Name = clientEditFormModel.Name,
            Status = Enum.Parse<ClientStatusEnum>(clientEditFormModel.Status),
            Registrationkey = clientEditFormModel.RegistrationKey,
        };
    }
}

public enum ClientStatusEnum
{
    Active = 1,
    Inactive = 2,
    Suspended = 3
}