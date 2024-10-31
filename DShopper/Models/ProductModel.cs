using DShopper.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DShopper.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage = "Name need more 4 letter")]
		public string Name { get; set; }
        [Required]
        public double Price { get; set; }

		[Required, MinLength(4, ErrorMessage = "Description is required")]
		public string Description { get; set; }

		public string Slug { get; set; }

		public int Status { get; set; }

		public string Image { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage ="Category is required")]
		public int CategoryId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Brand is required")]
        public int BrandId { get; set; } 

		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
	}
}
