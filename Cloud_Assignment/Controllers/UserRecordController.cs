using Cloud_Assignment.Areas.Identity.Data;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cloud_Assignment.Controllers
{
    public class UserRecordController : Controller
    {
        private readonly UserManager<Cloud_AssignmentUser> _userManager;
        private readonly SignInManager<Cloud_AssignmentUser> _signInManager;
        private readonly Cloud_AssignmentContext _context;
        private readonly ILogger<Cloud_AssignmentUser> _logger;
        public UserRecordController(ILogger<Cloud_AssignmentUser> logger, SignInManager<Cloud_AssignmentUser> signInManager, UserManager<Cloud_AssignmentUser> userManager, Cloud_AssignmentContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public class userRecordViewModel
        {
            public Cloud_Assignment.Areas.Identity.Pages.Account.RegisterModel? registerMode { get; set; }
            public List<Cloud_AssignmentUser>? userList { get; set; }

            public List<String> emailList { get; set; }
        }
        public async Task<IActionResult> Index()
        {
            var userRecords = new userRecordViewModel();
            userRecords.registerMode = null;
            userRecords.userList = new List<Cloud_AssignmentUser>();
            userRecords.userList = await _userManager.Users.ToListAsync();
            userRecords.emailList = new List<String>();
            foreach(var item in userRecords.userList)
            {
                userRecords.emailList.Add(item.Email);
            }
            return View(userRecords);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerNewUser(string returnUrl)
        {
            var userEmail = Request.Form["userEmail"];
            var userPassword = Request.Form["userPassword"];
            var userFullName = Request.Form["userFullName"];
            var userDob = Request.Form["userDob"];
            var userType = Request.Form["userType"];

            var user = new Cloud_AssignmentUser();
            user.UserFullName = userFullName;
            user.UserDOB = DateTime.Parse(userDob);
            user.EmailConfirmed = true;
            user.UserRole = userType;
            user.Email = userEmail;
            user.UserName = userEmail;
            user.EmailConfirmed = true;
            var result = await _userManager.CreateAsync(user, userPassword);

            if (result.Succeeded)
            {

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                _logger.LogError("Errors here:");
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
                return RedirectToAction("Index", "userRecord");
            }

        }
    }
}
