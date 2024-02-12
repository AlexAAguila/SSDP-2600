using System.ComponentModel.DataAnnotations;

namespace WildPath.ViewModels
{
    public class UserVM
    {
        [Key]
        [Required]
        public string Email { get; set; }
    }
}
