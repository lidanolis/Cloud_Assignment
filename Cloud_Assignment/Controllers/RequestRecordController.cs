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
			List<FoodRequest> foodRequestList = await _context.FoodRequest.ToListAsync();
			return View(foodRequestList);
		}

		public async Task<IActionResult> EditRequest(int? RequestID) 
		{ 
			if (RequestID == null) 
			{ 
				return NotFound(); 
			} 
			var request = await _context.FoodRequest.FindAsync(RequestID); 
			
			if (request == null)
			{ 
				return BadRequest(RequestID + " is not found in the table!"); 
			} 
			return View(request); 
		}

		[HttpPost][ValidateAntiForgeryToken] 
		public async Task<IActionResult> UpdateRequest(FoodRequest foodRequest) { 
			try 
			{ 
				if (ModelState.IsValid) 
				{ 
					_context.FoodRequest.Update(foodRequest); 
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

		public async Task<IActionResult> DeleteRequest(int? RequestID) { 
			if (RequestID == null) { 
				return NotFound(); 
			} 
			var request = await _context.FoodRequest.FindAsync(RequestID); 
			if (request == null) { 
				return BadRequest(RequestID + " is not found in the list!"); 
			} 
			_context.FoodRequest.Remove(request); 
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(ViewRequest));
		}
	}
}
