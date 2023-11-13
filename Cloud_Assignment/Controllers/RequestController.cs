using Microsoft.AspNetCore.Mvc;

namespace Cloud_Assignment.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateFoodRequest()
        {
            //view havent created yet
            return View();
        }
        public IActionResult CreateMoneyRequest()
        {
            //view havent created yet
            return View();
        }
    }
}
