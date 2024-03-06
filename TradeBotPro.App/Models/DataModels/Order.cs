using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeBotPro.App.Models.DataModels;

[Table("orders")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    [Required]
    public int BarIndex { get; set; }

    [Required]
    public int PositionId { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ZoneId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    [Required]
    [MaxLength(50)]
    public string Symbol { get; set; }

    [Required]
    public double EntryPrice { get; set; }

    [Required]
    public DateTime EntryTime { get; set; }

    [Required]
    public double StopLoss { get; set; }

    [Required]
    public double TakeProfit { get; set; }

    [Required]
    public double Pips { get; set; }

    [Required]
    public User User { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}