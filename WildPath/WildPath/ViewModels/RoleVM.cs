using System.ComponentModel.DataAnnotations;

namespace WildPath.ViewModels
{
    public class RoleVM
    {
        [Display(Name = "Role Id")]
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

}
