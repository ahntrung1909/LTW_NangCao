using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BTL.Controllers
{
    public class CategoryController : Controller
    {
        private DataContext _entityContext { get; }
        public IActionResult Index()
        {
            return View();
        }

    }
}
