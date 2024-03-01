using System.ComponentModel.DataAnnotations;
namespace WildPath.Models 
{ 
    public class UploadModel 
    { 
        [Required(ErrorMessage = "Please select a file.")] 
        public IFormFile ImageFile { get; set; } 
    } 
}