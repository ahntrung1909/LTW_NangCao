using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
    }
}
