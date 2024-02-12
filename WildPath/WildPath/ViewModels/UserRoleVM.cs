using System.ComponentModel.DataAnnotations;

namespace WildPath.ViewModels
{
    public class UserRoleVM
    {
        [Key]
        public int? ID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
