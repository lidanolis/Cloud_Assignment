using Cloud_Assignment.Areas.Identity.Data;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace Cloud_Assignment.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
        private readonly UserManager<Cloud_AssignmentUser> _userManager;
        private readonly SignInManager<Cloud_AssignmentUser> _signInManager;
        public ProfileController(Cloud_AssignmentContext context, UserManager<Cloud_AssignmentUser> userManager, SignInManager<Cloud_AssignmentUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string formType, string userId)
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        public async Task<IActionResult> removeAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteAccount(string userId)
        {
            String UId = Request.Form["UserId"];
            List<FoodRecord> foodList = await _context.FoodRecord.ToListAsync();
            List<FinancialRecord> financialList = await _context.FinancialRecord.ToListAsync();
            List<DistributionSchedule> distributionList = await _context.DistributionSchedule.ToListAsync();
            List<RequestRecord> requestList = await _context.RequestRecord.ToListAsync();

            if (foodList.Count > 0)
            {
                foreach (var item in foodList)
                {
                    if (item.UserId == UId)
                    {
                        item.UserId = null;
                    }
                }
                _context.FoodRecord.UpdateRange(foodList);
                await _context.SaveChangesAsync();
            }

            if (financialList.Count > 0)
            {
                foreach (var item in financialList)
                {
                    if (item.UserId == UId)
                    {
                        item.UserId = null;
                    }
                }
                _context.FinancialRecord.UpdateRange(financialList);
                await _context.SaveChangesAsync();
            }

            if (distributionList.Count > 0)
            {
                foreach (var item in distributionList)
                {
                    if (item.DistributorId == UId)
                    {
                        item.DistributorId = null;
                    }

                }
                _context.DistributionSchedule.UpdateRange(distributionList);
                await _context.SaveChangesAsync();
            }

            if (requestList.Count > 0)
            {
                foreach (var item in requestList)
                {
                    if (item.UserId == UId)
                    {
                        item.UserId = null;
                    }
                    else if (item.StaffId == UId)
                    {
                        item.StaffId = null;
                    }
                }
                _context.RequestRecord.UpdateRange(requestList);
                await _context.SaveChangesAsync();
            }
            var userToDelete = await _userManager.FindByIdAsync(UId);
            await _userManager.DeleteAsync(userToDelete);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
