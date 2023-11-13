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
    }
}
