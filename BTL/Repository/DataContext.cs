using BTL.Models;
using BTL.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BTL.Repository
{
	public class DataContext : IdentityDbContext<ApplicationUser>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}



		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }

	}

	public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		public DataContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseSqlServer("Data Source=WINDOWS-10;Initial Catalog=Shopping_Tutorial;Integrated Security=True;Trust Server Certificate=True");

			return new DataContext(optionsBuilder.Options);
		}
	}
}
