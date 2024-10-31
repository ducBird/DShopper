using System.ComponentModel.DataAnnotations;

namespace DShopper.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4,ErrorMessage ="Name is required")]
		public string Name { get; set; }

		[Required, MinLength(4, ErrorMessage = "Description is required")]
		public string Description { get; set; }

		public string Slug { get; set; }

		public int Status { get; set; }
	}
}
