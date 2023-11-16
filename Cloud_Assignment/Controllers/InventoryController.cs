using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;
using static Cloud_Assignment.Controllers.DonationController;
using static Cloud_Assignment.Controllers.InventoryController;

namespace Cloud_Assignment.Controllers
{
    public class InventoryController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
        public InventoryController(Cloud_AssignmentContext context)
        {
            _context = context;
        }
        public class InventoryViewModel
        {
            public List<InventoryRecord>? FoodList { get; set; }
            public List<FinancialRecord>? FinancialRecord { get; set; }

            public List<int> cantRemoveList { get; set; }

        }

        public async Task<IActionResult> Index()
        {
            var HistoryList = new InventoryViewModel();
            HistoryList.FoodList = new List<InventoryRecord>();
            HistoryList.FinancialRecord = new List<FinancialRecord>();
            var pendingFood = new List<int>();
            List<RequestRecord> requestList = new List<RequestRecord>();
            requestList = await _context.RequestRecord.ToListAsync();
            HistoryList.FoodList = await _context.InventoryRecord.ToListAsync();
            HistoryList.FinancialRecord = await _context.FinancialRecord.ToListAsync();
            foreach (var record in requestList)
            {
                if (record.RequestStatus == "pending" && !(pendingFood.Contains((int)record.FoodId)))
                {
                    pendingFood.Add((int)record.FoodId);
                }
            }
            HistoryList.cantRemoveList = pendingFood;
            return View(HistoryList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMoneyDonation()
        {
            List<FinancialRecord> financialList = await _context.FinancialRecord.ToListAsync();
            FinancialRecord specificlist;
            decimal balance = 0;
            if (financialList.Any())
            {
                specificlist = financialList.Last();
                balance = specificlist.Balance;
            }
            if(Request.Form["passedType"] == "Increase")
            {
                balance = balance + Decimal.Parse(Request.Form["PassedAmount"]);
            }
            else
            {
                balance = balance - Decimal.Parse(Request.Form["PassedAmount"]);
            }

            var Donation = new FinancialRecord
            {
                RecordType = Request.Form["PassedType"] == "Increase" ? "donation" : "distribute",
                UserId = Request.Form["UserId"],
                CurrencyType = "RM",
                Amount = Decimal.Parse(Request.Form["passedAmount"]),
                Balance = balance,
                DOR = DateTime.Now,
                Description = "Manual Modification. Reason: " + Request.Form["ChangeDesc"]
            };
            _context.Add(Donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodDonation()
        {
            int foodID;
            String DonationOptions = Request.Form["DonationOptions"];
            bool isEqual = String.Equals("ModifyOther", DonationOptions);
            var passedType = "donation";
            var passedAmount = 0;
            if (isEqual)
            {
                String FoodName = Request.Form["FoodType"];
                String FoodDes = Request.Form["FoodDesc"];
                passedAmount = int.Parse(Request.Form["FoodQuantityInput"]);
                var Food = new InventoryRecord
                {
                    FoodName = FoodName,
                    FoodTotal = int.Parse(Request.Form["FoodQuantityInput"]),
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
                passedType = Request.Form["PassedDonationType"] == "Increase" || Request.Form["PassedDonationType"] == "Increase-M" ? "donation" : "distribute";
                passedAmount = int.Parse(Request.Form["PassedDonationAmount"]);
                InventoryRecord SpecificInventory = await _context.InventoryRecord.FindAsync(foodID);
                SpecificInventory.FoodTotal = Request.Form["PassedDonationType"] == "Increase" ? SpecificInventory.FoodTotal + int.Parse(Request.Form["FoodQuantityInput"]) : Request.Form["PassedDonationType"] == "Decrease" ? SpecificInventory.FoodTotal - int.Parse(Request.Form["FoodQuantityInput"]) : int.Parse(Request.Form["FoodQuantityInput"]);
                await _context.SaveChangesAsync();
            }

            var Donation = new FoodRecord
            {
                RecordType = passedType,
                UserId = Request.Form["UserId"],
                FoodId = foodID,
                FoodQuantity = passedAmount,
                DOR = DateTime.Now,
                Description = "Food Donation"
            };

            _context.Add(Donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInventory(int ? FoodId)
        {
            InventoryRecord specificRecord = new InventoryRecord();
            if (_context != null)
            {
                specificRecord = await _context.InventoryRecord.FindAsync(FoodId);
            }
            return View(specificRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInventory()
        {
            InventoryRecord specificRecord = new InventoryRecord();
            specificRecord = await _context.InventoryRecord.FindAsync(int.Parse(Request.Form["FoodId"]));
            specificRecord.FoodName = Request.Form["FoodName"];
            specificRecord.FoodDesc = Request.Form["FoodDesc"];
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveInventory(int? fdId)
        {
            if(fdId == null)
            {
                return BadRequest("Invalid ID");
            }
            var SpecificFood = await _context.InventoryRecord.FindAsync(fdId);

            List<FoodRecord> foodList = new List<FoodRecord>();
            foodList = await _context.FoodRecord.ToListAsync();

            foreach(var item in foodList)
            {
                if(item.FoodId == fdId)
                {
                    item.FoodId = null;
                }
            }
            _context.FoodRecord.UpdateRange(foodList);
            await _context.SaveChangesAsync();
            List<RequestRecord> requestList = new List<RequestRecord>();
            requestList = await _context.RequestRecord.ToListAsync();

            foreach (var item in requestList)
            {
                if (item.FoodId == fdId)
                {
                    item.FoodId = null;
                }
            }
            _context.RequestRecord.UpdateRange(requestList);
            await _context.SaveChangesAsync();

            _context.InventoryRecord.Remove(SpecificFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
