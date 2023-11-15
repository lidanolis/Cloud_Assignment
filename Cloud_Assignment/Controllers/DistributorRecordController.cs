using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Assignment.Controllers
{
    public class DistributorRecordController : Controller
    {
        private readonly Cloud_AssignmentContext _context;

        public DistributorRecordController(Cloud_AssignmentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<DistributionSchedule> schedule = await _context.DistributionSchedule.ToListAsync();
            return View(schedule);
        }

        public async Task<IActionResult> ViewDetail(int? RecordID) //View
        {
            if(RecordID == null)
            {
                return NotFound();
            }
            var recordID = await _context.RequestRecord.FindAsync(RecordID);
            if(recordID == null)
            {
                return BadRequest(recordID + " is not found in the table!");
            }
			return View(recordID);
        }

        public async Task<IActionResult> DeliverOrder(int? DistributionID) //Edit
        {
            if (DistributionID == null)
            {
                return NotFound();
            }
            var distribute_ID = await _context.DistributionSchedule.FindAsync(DistributionID);

            if (distribute_ID == null)
            {
                return BadRequest(distribute_ID + " is not found in the table!");
            }
            return View(distribute_ID);
        }

        public async Task<IActionResult> ViewRecord() //View
        {
			List<DistributionSchedule> record = await _context.DistributionSchedule.ToListAsync();
			return View(record);
		}

		public async Task<IActionResult> DeleteOrder(int? DistributionID) //Delete
		{
			if (DistributionID == null)
			{
				return NotFound();
			}
			var selected_Id = await _context.DistributionSchedule.FindAsync(DistributionID);
			if (selected_Id == null)
			{
				return BadRequest(DistributionID + " is not found in the list!");
			}
			_context.DistributionSchedule.Remove(selected_Id);
			await _context.SaveChangesAsync();
			return RedirectToAction("ViewRecord");
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder(DistributionSchedule distributionSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.DistributionSchedule.Update(distributionSchedule);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View("DeliverOrder", distributionSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }


        }

		
	}
}
