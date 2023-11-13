using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Models
{
    public class FinancialRecord
    {
        [Key]
        public int RecordId { get; set; }

        //donation/distribute
        public String? RecordType { get; set; }

        [ForeignKey("User")]
        public String? UserId { get; set; }

        public Cloud_AssignmentUser? User { get; set; }

        public String? CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime DOR { get; set; }

        public String? Description {  get; set; }
    }
}
