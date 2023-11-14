using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Controllers
{
	public class RequestController : Controller
	{
		private readonly Cloud_AssignmentContext _context;
		public RequestController(Cloud_AssignmentContext context)
		{
			_context = context;
		}
		public IActionResult RequestForHelp()
		{
			//view havent created yet
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddFoodRequest()
		{	
			try
			{
				List<FoodRequest> foodRequest = await _context.FoodRequest.ToListAsync();
				FoodRequest specificlist;
				var request = new FoodRequest
				{
					UserId = Request.Form["UserId"],
					RequestDate = DateTime.Now,
					RequestStatus = "pending",
					ContactName = Request.Form["ContactName"],
					Phone = Request.Form["Phone"],
					Email = Request.Form["Email"],
					Address = Request.Form["Address"],
					Adults = int.Parse(Request.Form["Adults"]),
					Children = int.Parse(Request.Form["Children"]),
					SpecialNeeds = Request.Form["SpecialNeeds"],
					FoodType = Request.Form["FoodType"],
					DietaryRestrictions = Request.Form["DietaryRestrictions"],
					DeliveryDate = Convert.ToDateTime(Request.Form["DeliveryDate"]).Date,
				};
				_context.Add(request);
				await _context.SaveChangesAsync();
				return RedirectToAction("RequestForHelp");
			}
			catch (Exception ex) { return BadRequest("Error: " + ex.Message); }
		}
	}
}
