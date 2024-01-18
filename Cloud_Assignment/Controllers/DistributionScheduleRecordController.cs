using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;

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

        public class DistributionViewModel
        {
            public List<RequestRecord>? RequestList { get; set; }
            public DistributionSchedule? DistributionRecord { get; set; }
        }

        public async Task<IActionResult> AddDistributionScheduleRecord()
        {
            var viewModel = new DistributionViewModel();
            viewModel.RequestList = await _context.RequestRecord.ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> ViewScheduleRecord()
        {
            List<DistributionSchedule> record = await _context.DistributionSchedule.ToListAsync();
            return View(record);
        }

        public async Task<IActionResult> EditDistributionScheduleRecord(int? DistributionId)
        {
            if (DistributionId == null)
            {
                return NotFound();
            }
            var distributionRecord = await _context.DistributionSchedule.FindAsync(DistributionId);

            if (distributionRecord == null)
            {
                return BadRequest(DistributionId + "is not found in the table!");
            }
            return View(distributionRecord);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistributionRecord(DistributionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.DistributionSchedule.Add(viewModel.DistributionRecord!);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
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
                    return RedirectToAction("ViewScheduleRecord", "DistributionScheduleRecord");
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
            return RedirectToAction("ViewScheduleRecord", "DistributionScheduleRecord");
        }

        public void ViewProof(string? imageId)
        {
            string cloudfrontUrl = "https://d3h8frk5rv7b5p.cloudfront.net/";
            string imageUrl = cloudfrontUrl + imageId;
            Response.Redirect(imageUrl);
        }
    }

    
}
