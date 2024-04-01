using System.ComponentModel.DataAnnotations;
using WildPath.EfModels;

namespace WildPath.ViewModels
{
    public class ProductVM
    {
        public WildPath.EfModels.Item Item { get; set; }
        public ImageStore ImageStore { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
