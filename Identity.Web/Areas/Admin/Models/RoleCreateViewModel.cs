using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Areas.Admin.Models;

public record RoleCreateViewModel
{
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [Display(Name = "Rol isim alanı")]
    public string Name { get; set; }
}