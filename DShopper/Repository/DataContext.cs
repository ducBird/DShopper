﻿using DShopper.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Repository
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories {get; set;} 
	}
}
