using DShopper.Models;
using DShopper.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }

        //[Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        //[Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            // check column in model
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(c => c.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Brand already exists");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add brand successful";
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
            return View(brand);
        }

        //[Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            return View(brand);
        }

        //[Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, BrandModel brand)
        {
            var existed_brand = _dataContext.Brands.Find(brand.Id); // tim san pham theo Id
            // check column in model
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                // Update other brand properties
                existed_brand.Name = brand.Name;
                existed_brand.Description = brand.Description;
                existed_brand.Status = brand.Status;
                // ... other properties

                _dataContext.Update(existed_brand); //Update the existing brand object
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update brand successful";
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
            return View(brand);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            if (brand == null)
            {
                return NotFound(); // Handle brand not found
            }
            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Deleted brand";
            return RedirectToAction("Index");
        }
    }
}

