using Microsoft.AspNetCore.Mvc;

namespace Cloud_Assignment.Controllers
{
    public class RequestRecordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
