using Microsoft.AspNetCore.Mvc;

namespace DShopper.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
