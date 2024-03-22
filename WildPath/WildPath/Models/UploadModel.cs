using System.ComponentModel.DataAnnotations;
namespace WildPath.Models 
{ 
    public class UploadModel 
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; } 
    } 
}