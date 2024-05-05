using System.ComponentModel.DataAnnotations;

namespace Tutorial6.Models;

public class ProductWarehouse
{
    public int IdProductWarehouse { get;}
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int IdProduct { get; set; }
    public int IdOrder { get; }
    [Required]
    public int Amount { get; set; }
    public double Price { get; }
    [Required]
    public DateTime CreatedAt { get; set; }
}