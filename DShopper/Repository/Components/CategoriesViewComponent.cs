using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DShopper.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public CategoriesViewComponent (DataContext context)
		{
			_dataContext = context;
		}
		//public Task<ViewComponentResult> InvokeAsync() => View(await _dataContext.Categories.ToListAsync());

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// Lấy dữ liệu từ context
			var categories = await Task.FromResult(_dataContext.Categories);

			// Trả về ViewComponentResult đúng kiểu
			return View(categories);
		}
	}
}
