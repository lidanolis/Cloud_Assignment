using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Assignment.Controllers
{
    public class DonationController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
        public DonationController(Cloud_AssignmentContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateFoodDonation()
        {
            List<InventoryRecord> foodList;
            if (_context != null)
            {
                foodList = await _context.InventoryRecord.ToListAsync();
            }
            else
            {
                foodList = new List<InventoryRecord>();
            }
            return View(foodList);
        }
        public IActionResult CreateMoneyDonation()
        {
            return View();
        }
        //---------------------------------------------------------------------------------------
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
        //---------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodDonation()
        {
            int foodID;
            String optionExisting = Request.Form["selectedOption"];
            bool isEqual = String.Equals("showOther", optionExisting);
            if (isEqual)
            {
                String FoodName = Request.Form["FoodType"];
                String FoodDes = Request.Form["FoodDesc"];
                var Food = new InventoryRecord
                {
                    FoodName = FoodName,
                    FoodTotal = int.Parse(Request.Form["FoodQuantity"]),
                    FoodDesc = FoodDes
                };
                _context.Add(Food);
                await _context.SaveChangesAsync();

                List<InventoryRecord> inventoryList;
                if (_context != null)
                {
                    inventoryList = await _context.InventoryRecord.ToListAsync();
                }
                else
                {
                    inventoryList = new List<InventoryRecord>();
                }
                InventoryRecord latestInventory = inventoryList.Last();
                foodID = latestInventory.FoodId;

            }
            else
            {
                foodID = int.Parse(Request.Form["FoodId"]);
                InventoryRecord SpecificInventory = await _context.InventoryRecord.FindAsync(foodID);
                SpecificInventory.FoodTotal = SpecificInventory.FoodTotal + int.Parse(Request.Form["FoodQuantity"]);
                await _context.SaveChangesAsync();
            }

            var Donation = new FoodRecord
            {
                RecordType = "donation",
                UserId = Request.Form["UserId"],
                FoodId = foodID,
                FoodQuantity = int.Parse(Request.Form["FoodQuantity"]),
                DOR = DateTime.Now,
                Description = "Food Donation"
            };

            _context.Add(Donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMoneyDonation()
        {
            List < FinancialRecord > financialList = await _context.FinancialRecord.ToListAsync();
            FinancialRecord specificlist;
            decimal balance = 0;
            if (financialList.Any())
            {
                specificlist = financialList.Last();
                balance = specificlist.Balance;
            }
            var Donation = new FinancialRecord
            {
                RecordType = "donation",
                UserId = Request.Form["UserId"],
                CurrencyType = "RM",
                Amount = Decimal.Parse(Request.Form["Amount"]),
                Balance = balance + Decimal.Parse(Request.Form["Amount"]),
                DOR = DateTime.Now,
                Description = "Money Donation"
            };
            _context.Add(Donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
