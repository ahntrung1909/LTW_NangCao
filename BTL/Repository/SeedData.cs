using BTL.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "MacBook", Slug = "macbook", Description = "MacBook quả táo", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "PC", Slug = "pc", Description = "Máy tính để bàn", Status = 1 };
				CategoryModel dellgaming = new CategoryModel { Name = "DellGaming", Slug = "dell", Description = "Dell", Status = 1 };
				CategoryModel lenovogaming = new CategoryModel { Name = "LenovoGaming", Slug = "Lenovo", Description = "Lenovo", Status = 1 };

				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple quả táo", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Sam Sung", Slug = "sam sung", Description = "Samsung là thương hiệu điện tử", Status = 1 };
				BrandModel lenovo = new BrandModel { Name = "Lenovo", Slug = "lenovo", Description = "Lenovo", Status = 1 };
				BrandModel dell = new BrandModel { Name = "Dell", Slug = "Dell", Description = "Dell", Status = 1 };

				_context.Products.AddRange(

					new ProductModel { Name = "MacBook", Slug = "macbook", Description = "Macbook la so 1", Image = "1.jpg", Category = macbook, Brand = apple, Price = 12334 },
					new ProductModel { Name = "PC", Slug = "pc", Description = "Pc so 2 the gioi", Image = "1.jpg", Category = pc, Brand = samsung, Price = 20200 },
					new ProductModel { Name = "Dell 1", Slug = "dell", Description = "Dell", Image = "1.jpg", Category = dellgaming, Brand = dell, Price = 12334 },
					new ProductModel { Name = "Lenovo1", Slug = "pc", Description = "Pc so 2 the gioi", Image = "1.jpg", Category = lenovogaming, Brand = lenovo, Price = 20200 }
				);

				_context.SaveChanges();
			}
		}
	}
}
