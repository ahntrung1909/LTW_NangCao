using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ProductController : Controller 
    {
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}

		public IActionResult Index()
        {
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			return View();
        }

		public async Task<IActionResult> Details(int Id)
		{
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			if (Id == null)
			{
				return RedirectToAction("Index");
			}
			
			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			var recommendedItems = _dataContext.Products
										.Where(p => p.CategoryId == productsById.CategoryId && p.Id != Id)
										.Take(3) // Limit to 3 recommended items
										.ToList();

			ViewBag.RecommendedItems = recommendedItems;

			return View(productsById);
		}
		[HttpPost]
		[HttpPost]
		public async Task<IActionResult> Add(int Id, int quantity = 1)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			if (product == null)
			{
				return NotFound();
			}

			// Kiểm tra số lượng tồn kho
			if (product.Quantity < quantity)
			{
				TempData["error"] = "Sản phẩm không đủ số lượng trong kho.";
				return RedirectToAction("Details", "Product", new { Id = product.Id });
			}

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem == null)
			{
				cart.Add(new CartItemModel(product) { Quantity = quantity });
			}
			else
			{
				cartItem.Quantity += quantity;
			}

			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Sản phẩm đã được thêm vào giỏ hàng.";
			return RedirectToAction("Index", "Cart");
		}

	}
}
