using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Models
{
    public class FoodRecord
    {
        [Key]
        public int RecordId { get; set; }

        //donation/distribute
        public String? RecordType { get; set; }

        [ForeignKey("User")]
        public String? UserId { get; set; }

        public Cloud_AssignmentUser? User { get; set; }

        [ForeignKey("InventoryRecord")]
        public int? FoodId { get; set; }

        public InventoryRecord? InventoryRecord { get; set; }
        public int? FoodQuantity { get; set; }
        public DateTime DOR { get; set; }
    }
}
