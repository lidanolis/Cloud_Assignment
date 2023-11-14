using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cloud_Assignment.Controllers
{
    public class DistributionScheduleRecordController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
        public DistributionScheduleRecordController(Cloud_AssignmentContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<RequestRecord> records = await _context.RequestRecord.ToListAsync();
            return View(records);
        }

        public async Task<IActionResult> AddDistributionScheduleRecord()
        {
            var recordId = await _context.RequestRecord.Select(r=>r.RecordId).ToListAsync();
            ViewBag.RecordId = new SelectList(recordId);
            return View();
        }

        public async Task<IActionResult> ViewScheduleRecord()
        {
            List<DistributionSchedule> record = await _context.DistributionSchedule.ToListAsync();
            return View(record);
        }

        public async Task<IActionResult> EditDistributionScheduleRecord(int ? DistributionId)
        {
            if(DistributionId == null)
            {
                return NotFound();
            }
            var distributionRecord = await _context.DistributionSchedule.FindAsync(DistributionId);

            if(distributionRecord == null)
            {
                return BadRequest(DistributionId + "is not found in the table!");
            }
            return View(distributionRecord);
        }

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistributionRecord(DistributionSchedule distributionSchedule)
        {
            if(ModelState.IsValid)
            {
                _context.DistributionSchedule.Add(distributionSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(distributionSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDistributionRecord(DistributionSchedule distributionSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.DistributionSchedule.Update(distributionSchedule);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "DistributionScheduleRecord");
                }
                return View("EditDistributionScheduleRecord", distributionSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<IActionResult> DeleteDistributionRecord(int? DistributionId)
        {
            if (DistributionId == null)
            {
                return NotFound();
            }
            var record = await _context.DistributionSchedule.FindAsync(DistributionId);
            if (record == null)
            {
                return BadRequest(DistributionId + " is not found!!");
            }
            _context.DistributionSchedule.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "DistributionScheduleRecord");
        }
    }
}
