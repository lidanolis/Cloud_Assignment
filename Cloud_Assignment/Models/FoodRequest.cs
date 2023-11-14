using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Models
{
	public class FoodRequest
	{
		[Key] public int RequestID { get; set; }
		public string  ContactName { get; set; } 
		public string  Phone { get; set; } 
		public string Email { get; set; } 
		public string Address { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
		public string ? SpecialNeeds { get; set; }
		public string ? FoodType { get; set; }
		public string ? DietaryRestrictions { get; set; }
		public DateTime DeliveryDate { get; set; }
		public DateTime RequestDate { get; set; }
		public string RequestStatus { get; set; }
		[ForeignKey("User")]
		public String UserId { get; set; }
	}
}
