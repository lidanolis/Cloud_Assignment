using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cloud_Assignment.Models
{
    public class DistributionSchedule
    {
        [Key]
        public int DistributionId { get; set; }

        //donation/distribute
        public String? RecordType { get; set; }

        [ForeignKey("Record")]
        public int? RecordId { get; set; }

        public RequestRecord? Record { get; set; }

        public String? DistributionStatus { get; set; }
        public DateTime DOD { get; set; }

        [ForeignKey("Distributor")]
        public String? DistributorId { get; set; }

        public Cloud_AssignmentUser? Distributor { get; set; }
    }
}
