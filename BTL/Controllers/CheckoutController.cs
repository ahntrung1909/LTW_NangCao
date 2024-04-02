using BTL.Models;
using BTL.Repository;
using BTL.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BTL.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			List<CartItemModel> Cartitems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			// Kiểm tra người dùng đã đăng nhập hay chưa
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else if (Cartitems.Sum(x => x.Quantity) <= 0)
			{
				TempData["error"] = "Tạo thất bại, giỏ hàng trống";
				return RedirectToAction("Index", "Cart");
			}
			else
			{
				// Tạo đơn hàng mới
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				await _dataContext.SaveChangesAsync();

				// Cập nhật số lượng sản phẩm trong kho sau khi thanh toán
				foreach (var cartItem in Cartitems)
				{
					var product = await _dataContext.Products.FindAsync(cartItem.ProductId);

					// Kiểm tra số lượng sản phẩm trong kho đủ để thực hiện giao dịch hay không
					if (product.Quantity < cartItem.Quantity)
					{
						TempData["error"] = "Không đủ hàng trong kho";
						return RedirectToAction("Index", "Cart");
					}

					// Trừ đi số lượng sản phẩm trong kho tương ứng với số lượng trong giỏ hàng
					product.Quantity -= cartItem.Quantity;
					_dataContext.Update(product);
					await _dataContext.SaveChangesAsync();

					// Tạo chi tiết đơn hàng
					var orderDetail = new OrderDetails();
					orderDetail.UserName = userEmail;
					orderDetail.OrderCode = orderCode;
					orderDetail.ProductId = cartItem.ProductId;
					orderDetail.Price = cartItem.Price;
					orderDetail.Quantity = cartItem.Quantity;
					_dataContext.Add(orderDetail);
					await _dataContext.SaveChangesAsync();
				}

				// Xóa giỏ hàng sau khi thanh toán
				HttpContext.Session.Remove("Cart");

				TempData["success"] = "Tạo thành công, chờ duyệt đơn hàng";
				return RedirectToAction("Index", "Cart");
			}
		}

	}
}
