using Microsoft.AspNetCore.Mvc;


namespace BTL.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
