using DShopper.Models;
using DShopper.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}

        //[Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        //[Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            // check column in model
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exists");
                    return View(category);
                }

                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add category successful";
                return RedirectToAction("Index");
            }
            else
            {
                // check _NotificationPartial
                TempData["error"] = "Model have something error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(category);
        }

        //[Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id); 
            return View(category);
        }

        //[Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, CategoryModel category)
        {
            var existed_category = _dataContext.Categories.Find(category.Id); // tim san pham theo Id
            // check column in model
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                // Update other category properties
                existed_category.Name = category.Name;
                existed_category.Description = category.Description;
                existed_category.Status = category.Status;
                // ... other properties

                _dataContext.Update(existed_category); //Update the existing category object
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update category successful";
                return RedirectToAction("Index");
            }
            else
            {
                // check _NotificationPartial
                TempData["error"] = "Model have something error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            if (category == null)
            {
                return NotFound(); // Handle category not found
            }
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Deleted category";
            return RedirectToAction("Index");
        }
    }
}
