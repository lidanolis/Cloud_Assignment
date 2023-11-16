using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Controllers
{
	public class RequestRecordController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
		public RequestRecordController(Cloud_AssignmentContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            //havent create view yet (refer tutorial)
            return View();
        }

		public async Task<IActionResult> ViewRequest() {
			List<RequestRecord> foodRequestList = await _context.RequestRecord.ToListAsync();
			return View(foodRequestList);
		}

		public async Task<IActionResult> EditRequest(int? RecordId) 
		{ 
			if (RecordId == null) 
			{ 
				return NotFound(); 
			} 
			var request = await _context.RequestRecord.FindAsync(RecordId); 
			
			if (request == null)
			{ 
				return BadRequest(RecordId + " is not found in the table!"); 
			} 
			return View(request); 
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateRequest(RequestRecord foodRequest)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (foodRequest.RequestStatus == "deny" && foodRequest.FoodId != null)
					{
						InventoryRecord specificInventory = await _context.InventoryRecord.FindAsync(foodRequest.FoodId);
						if (specificInventory != null)
						{
							int newFoodTotal = specificInventory.FoodTotal.GetValueOrDefault() + foodRequest.FoodQuantity.GetValueOrDefault();
							specificInventory.FoodTotal = newFoodTotal;
							_context.InventoryRecord.Update(specificInventory);
						}
					}
					else
					{
                        var Donation = new FoodRecord
                        {
                            RecordType = "distribution",
                            UserId = Request.Form["StaffId"],
                            FoodId = foodRequest.FoodId,
                            FoodQuantity = foodRequest.FoodQuantity,
                            DOR = DateTime.Now,
                            Description = "Food Distribution"
                        };

                        _context.Add(Donation);
                        await _context.SaveChangesAsync();
                    }
					_context.RequestRecord.Update(foodRequest);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(ViewRequest));
				}
				return View("EditRequest", foodRequest);
			}
			catch (Exception ex)
			{
				return BadRequest("Error: " + ex.Message);
			}
		}

		public async Task<IActionResult> DeleteRequest(int? RecordId) { 
			if (RecordId == null) { 
				return NotFound(); 
			} 
			var request = await _context.RequestRecord.FindAsync(RecordId); 
			if (request == null) { 
				return BadRequest(RecordId + " is not found in the list!"); 
			} 
			_context.RequestRecord.Remove(request); 
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(ViewRequest));
		}
	}
}
