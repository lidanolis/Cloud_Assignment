using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Models
{
    public class NewsletterEmail
    {
        [Key]
        public int NewsletterEmailId { get; set; }
        public String? Email { get; set; }
        [ForeignKey("User")]
        public String? UserId { get; set; }
    }
}
