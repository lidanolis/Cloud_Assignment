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

        public async Task<IActionResult> RequestFood()
        {
            List<InventoryRecord> foodList = await _context.InventoryRecord.ToListAsync();
            return View(foodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodRequest()
        {
            try
            {
                List<RequestRecord> foodRequest = await _context.RequestRecord.ToListAsync();
                RequestRecord specificlist;
                var request = new RequestRecord
                {
                    UserId = Request.Form["UserId"],
                    requestType = "food",
                    FoodId = int.Parse(Request.Form["FoodId"]),
					FoodQuantity = int.Parse(Request.Form["FoodQuantity"]),
                    RequestStatus = "pending",
					RequestDesc = Request.Form["RequestDesc"],
					DOR = DateTime.Now,
                };
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("RequestFood");
            }
            catch (Exception ex) { return BadRequest("Error: " + ex.Message); }
        }
	}
}
