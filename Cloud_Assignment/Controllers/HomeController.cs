using Cloud_Assignment.Areas.Identity.Data;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cloud_Assignment.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> NA;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly Cloud_AssignmentContext _context;

        public HomeController(Cloud_AssignmentContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //----------------------Below is all mk functions---------------------------------
        public async Task<IActionResult> viewFoodDonationRecord()
        {
            List<FoodRecord> foodDonationRecord = await _context.FoodRecord.ToListAsync();
            return View(foodDonationRecord);
        }

        public async Task<IActionResult> viewMoneyDonationRecord()
        {
            List<FinancialRecord> financialDonationRecord = await _context.FinancialRecord.ToListAsync();
            return View(financialDonationRecord);
        }

        public async Task<IActionResult> viewUserDetail()
        {
            List<Cloud_AssignmentUser> AllUserDetail = await _context.AspNetUsers.ToListAsync();
            return View(AllUserDetail);
        }

        //----------------------------------------------------------------------------
        public async Task<IActionResult> DeleteFoodDonationRecord(int? recordID)
        {
            if (recordID == null)
            {
                return NotFound();
            }
            var record = await _context.FoodRecord.FindAsync(recordID);
            if (record == null)
            {
                return BadRequest(recordID + " is not found in the list!");
            }
            _context.FoodRecord.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditFoodDonationRecord(int? recordID)
        {
            if (recordID == null)
            {
                return NotFound();
            }
            var record = await _context.FoodRecord.FindAsync(recordID);
            if (record == null)
            {
                return BadRequest(recordID + " is not found in the table!");
            }
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFoodDonationRecord(FoodRecord foodrecord)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.FoodRecord.Update(foodrecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View("EditData", foodrecord);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<IActionResult> DeleteMoneyDonationRecord(int? recordID)
        {
            if (recordID == null)
            {
                return NotFound();
            }
            var record = await _context.FinancialRecord.FindAsync(recordID);
            if (record == null)
            {
                return BadRequest(recordID + " is not found in the list!");
            }
            _context.FinancialRecord.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditMoneyDonationRecord(int? recordID)
        {
            if (recordID == null)
            {
                return NotFound();
            }
            var record = await _context.FinancialRecord.FindAsync(recordID);
            if (record == null)
            {
                return BadRequest(recordID + " is not found in the table!");
            }
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMoneyDonationRecord(FinancialRecord moneyrecord)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.FinancialRecord.Update(moneyrecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View("EditData", moneyrecord);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

    }
}