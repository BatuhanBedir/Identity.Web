using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Areas.Admin.Models;

public record RoleUpdateViewModel
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [Display(Name = "Rol isim alanı")]
    public string Name { get; set; } = null!;
}