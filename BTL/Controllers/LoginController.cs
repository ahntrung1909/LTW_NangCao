using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
