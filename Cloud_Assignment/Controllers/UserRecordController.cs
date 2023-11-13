using Microsoft.AspNetCore.Mvc;

namespace Cloud_Assignment.Controllers
{
    public class UserRecordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
