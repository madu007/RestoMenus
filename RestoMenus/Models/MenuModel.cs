using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestoMenus.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select a category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public string SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public List<string> ImagePaths { get; set; }
        [DisplayName("Images")]
        public List<IFormFile> Images { get; set; }

    }

}