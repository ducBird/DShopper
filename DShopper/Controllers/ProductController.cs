using DShopper.Models;
using DShopper.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Controllers
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
			return View();
		}

		public async Task<IActionResult> Details(int Id)
		{
			//CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();

			if (Id == null) return RedirectToAction("Index");

			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();

			return View(productsById);
		}
	}
}
