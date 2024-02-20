using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace BTL.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CheckOut()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
    }
}
