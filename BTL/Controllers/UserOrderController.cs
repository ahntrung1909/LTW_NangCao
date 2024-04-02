using BTL.Models;
using BTL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTL.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly DataContext _dataContext;

        public UserOrderController(DataContext context)
        {
            _dataContext = context;
        }

		public async Task<IActionResult> Index()
		{
			// Lấy email của người dùng đã đăng nhập
			var userEmail = User.FindFirstValue(ClaimTypes.Email);

			// Kiểm tra xem người dùng đã đăng nhập chưa
			if (string.IsNullOrEmpty(userEmail))
			{
				return RedirectToAction("Login", "Account");
			}

			// Truy vấn tất cả các chi tiết đơn hàng cho người dùng đã đăng nhập
			var orderDetails = await _dataContext.OrderDetails
				.Include(od => od.Product) // Bao gồm thông tin sản phẩm
				.Where(od => od.UserName == userEmail) // Lọc theo email người dùng
				.ToListAsync(); // Chuyển kết quả thành danh sách

			// Trả về danh sách chi tiết đơn hàng cho người dùng đã đăng nhập
			return View(orderDetails);
		}

	}
}
