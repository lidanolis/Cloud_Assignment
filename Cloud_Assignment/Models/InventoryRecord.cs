using System.ComponentModel.DataAnnotations;

namespace Cloud_Assignment.Models
{
    public class InventoryRecord
    {
        [Key]
        public int FoodId { get; set; }
        public string? FoodName { get; set; }
        public int? FoodTotal { get; set; }
        public string? FoodDesc { get; set; }
    }
}
