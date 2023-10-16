using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels;

public record ForgetPasswordViewModel
{
    [EmailAddress(ErrorMessage = "Email formatı yanlış")]
    [Display(Name = "Email: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string Email { get; set; } = null!;
}