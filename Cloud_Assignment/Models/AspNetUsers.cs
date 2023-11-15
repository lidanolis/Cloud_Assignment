using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Models
{
    public class AspNetUsers
    {
        [Key]
        public String Id { get;  set; }
        public String? UserName { get;  set; }
        public String? NormalizedUserName { get;  set; }
        public String? Email { get;  set; }
        public String? NormalizedEmail { get;  set; }
        public Boolean EmailConfirmed { get;  set; }
        public String? PasswordHash { get;  set; }
        public String? SecurityStamp { get;  set; }
        public String? ConcurrencyStampityStamp { get;  set; }
        public String? PhoneNumber { get;  set; }
        public Boolean PhoneNumberConfirmed { get;  set; }
        public Boolean TwoFactorEnabled { get;  set; }
        public DateTimeOffset LockoutEnd { get;  set; }
        public Boolean LockoutEnabled { get;  set; }
        public int AccessFailedCount { get;  set; }
        public String? UserAdress { get;  set; }
        public int UserAge { get;  set; }
        public DateTime UserDOB{ get;  set; }
        public String? UserFullName{ get;  set; }
        public String? UserRole { get;  set; }
    }
}
