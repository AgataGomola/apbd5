using System.ComponentModel.DataAnnotations;

namespace Tutorial6.Models;

public class Warehouse
{
    [Required]
    public int idWarehouse { get; set; }
    [Required]
    public string name { get; set; }
    [Required]
    public string address { get; set; }

    public Order Order { get; set; }
}