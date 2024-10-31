using DShopper.Models;
using DShopper.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Route("Admin/Product")]
	//[Authorize]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		// su dung de block file 
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}

		//[Route("Index")]
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}

		//[Route("Create")]
        public IActionResult Create()
        {
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

		//[Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			// check column in model
			if (ModelState.IsValid)
			{
                // 
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p =>	p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Product already exists");
					return View(product);
				}
				if(product.ImageUpload != null)
				{
					//upload anh vao wwwroot
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					// tao ra ten hinh anh ngau nhien
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadsDir, imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					product.Image = imageName;
				}
				
				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add product successful";
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
			return View(product);
		}

		//[Route("Edit")]
		public async Task<IActionResult> Edit(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            return View(product);
		}

		//[Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			var existed_product = _dataContext.Products.Find(product.Id); // tim san pham theo Id
            // check column in model
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
				if (product.ImageUpload != null)
                {
					// upload new image
                    //upload anh vao wwwroot
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    // tao ra ten hinh anh ngau nhien
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

					// delete old image
                    string oldFilePath = Path.Combine(uploadsDir, existed_product.Image);
                    try
                    {
						// if oldFilePath already exist -> delete image
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;
                }
				// Update other product properties
				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.CategoryId = product.CategoryId;
				existed_product.BrandId = product.BrandId;
				// ... other properties

                _dataContext.Update(existed_product); //Update the existing product object
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update product successful";
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
            return View(product);
        }

		public async Task<IActionResult> Delete(int Id)
		{
            ProductModel product = await _dataContext.Products.FindAsync(Id);
			if (product == null) 
			{
				return NotFound(); // Handle product not found
			}

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldFilePath = Path.Combine(uploadsDir, product.Image);

			try
			{
				if (System.IO.File.Exists(oldFilePath))
				{
					System.IO.File.Delete(oldFilePath);
				}
			}
			catch (Exception ex) 
			{
				ModelState.AddModelError("", "An error occurred while deleting the product image");
			}

            _dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();
            TempData["success"] = "Deleted product";
			return RedirectToAction("Index");
		}
    }
}
