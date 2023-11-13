using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cloud_Assignment.Models
{
    public class RequestRecord
    {
        [Key]
        public int RecordId { get; set; }

        //food/money
        public String? requestType { get; set; }

        [ForeignKey("User")]
        public String? UserId { get; set; }

        public Cloud_AssignmentUser? User { get; set; }

        [ForeignKey("Staff")]
        public String? StaffId { get; set; }

        public Cloud_AssignmentUser? Staff { get; set; }

        [ForeignKey("InventoryRecord")]
        public int? FoodId { get; set; }

        public InventoryRecord? InventoryRecord { get; set; }
        public int? FoodQuantity { get; set; }

        public Decimal? Amount { get; set; }

        public String? RequestStatus { get; set; }

        public String? RequestDesc {  get; set; }
        public DateTime DOR { get; set; }
    }
}
