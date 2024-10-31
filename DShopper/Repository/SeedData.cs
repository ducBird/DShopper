using DShopper.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{
				CategoryModel macbook = new CategoryModel{ Name = "Macbook", Slug="macbook", Description="Macbook new", Status=1 };
				CategoryModel iphone = new CategoryModel { Name = "IPhone", Slug = "iphone", Description = "IPhone new", Status = 1 };
				CategoryModel ssphone = new CategoryModel { Name = "Samsung Phone", Slug = "samsungphone", Description = "SS phone new", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is large brand in the world", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samung", Slug = "samsung", Description = "Samsung is large brand in the world", Status = 1 };
				_context.Products.AddRange(
					new ProductModel { Name = "Macbook 11", Slug = "macbook11", Description = "Macbook 11 is nice", Image = "abc.jpg", Category = macbook, Price = 1300, Status = 1, Brand = apple },
					new ProductModel { Name = "S20Ultra", Slug = "s20ultra", Description = "This phone is nice", Image = "abc.jpg", Category = ssphone, Price = 1300, Status = 1, Brand = samsung },
					new ProductModel { Name = "Iphone 15", Slug = "iphone15", Description = "This phone is nice", Image = "abc.jpg", Category = iphone, Price = 1300, Status = 1, Brand = apple }
				);
				_context.SaveChanges();
			}
		}
	}
}
