using System.ComponentModel.DataAnnotations;

namespace Tutorial6.Models;

public class Order
{
    public int idOrder { get; set; }
    public int idProduct { get; set; }
    [Required]
    public int amount { get; set; }
    [Required]
    public DateTime createdAt { get; set; }
    public DateTime fulfielledAt { get; set; }
    public Product Product { get; set; }
}