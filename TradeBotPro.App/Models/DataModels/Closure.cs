using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeBotPro.App.Models.DataModels;

[Table("closures")]
public class Closure
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int PositionId { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; }

    [Required]
    public double VolumeInUnits { get; set; }

    [Required]
    public double GrossProfit { get; set; }

    [Required]
    public double NetProfit { get; set; }

    [Required]
    public double Margin { get; set; }

    [Required]
    public double Commissions { get; set; }

    [Required]
    public double Quantity { get; set; }

    [Required]
    public User User { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}