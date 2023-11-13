using Microsoft.AspNetCore.Mvc;

namespace Cloud_Assignment.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            //havent create view yet (refer tutorial)
            return View();
        }
    }
}
