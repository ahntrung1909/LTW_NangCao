using BTL.Models.ViewModels;
using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext _Context)
		{
			_dataContext = _Context;
		}
		public IActionResult Index()
		{
			
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			ViewData["CartCount"] = Cartitems.Sum(x => x.Quantity);
			CartItemViewModel cartVM = new()
			{
				CartItems = Cartitems,
				GrandTotal = Cartitems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}
		public IActionResult Checkout()
		{
			return View();
		}
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

		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");


			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			TempData["success"] = "Decrease Item quantity to cart Successfully";

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.FirstOrDefault(c => c.ProductId == Id);

			// Kiểm tra số lượng tồn kho
			var product = _dataContext.Products.FirstOrDefault(p => p.Id == Id);
			if (product != null && cartItem != null && cartItem.Quantity < product.Quantity)
			{
				++cartItem.Quantity;
				TempData["success"] = "Increase Item quantity to cart Successfully";
			}
			else
			{
				TempData["error"] = "Sản phẩm không đủ số lượng trong kho.";
				// Hoặc thực hiện xử lý khác tùy thuộc vào yêu cầu của bạn
			}
			// Tiếp tục lưu giỏ hàng và chuyển hướng
			HttpContext.Session.SetJson("Cart", cart);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Remove Item of cart Successfully";

			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			CartItemModel cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

			if (cartItem != null)
			{
				cartItem.Quantity = quantity;
				HttpContext.Session.SetJson("Cart", cart);
				return Ok(); // Return OK status if update is successful
			}

			return NotFound(); // Return Not Found status if item is not found in cart
		}

		[HttpGet]
		public IActionResult CheckQuantity(int productId, int quantity)
		{
			var product = _dataContext.Products.FirstOrDefault(p => p.Id == productId);

			if (product != null && quantity <= product.Quantity)
			{
				return Json(new { result = true }); // Return true if quantity is valid
			}
			else
			{
				return Json(new { result = false }); // Return false if quantity is invalid
			}
		}

		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Clear all Item of cart Successfully";
			return RedirectToAction("Index");
		}
	}
}
