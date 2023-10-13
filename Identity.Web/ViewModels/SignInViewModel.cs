using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels;

public record SignInViewModel
{
    [EmailAddress(ErrorMessage = "Email formatı yanlış")]
    [Display(Name = "Email: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Şifre: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string Password { get; set; } = null!;

    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }
}