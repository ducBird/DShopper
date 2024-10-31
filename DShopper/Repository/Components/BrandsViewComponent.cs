using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Repository.Components
{
	public class BrandsViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public BrandsViewComponent (DataContext context)
		{
			_dataContext = context;
		}
		//public Task<ViewComponentResult> InvokeAsync() => View(await _dataContext.Categories.ToListAsync());

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// Lấy dữ liệu từ context
			var brands = await Task.FromResult(_dataContext.Brands);

			// Trả về ViewComponentResult đúng kiểu
			return View(brands);
		}
	}
}
