using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels;

public record ResetPasswordViewModel
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
    [Display(Name = "Yeni şifre :")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
    [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz")]
    [Display(Name = "Yeni şifre Tekrar :")]
    public string PasswordConfirm { get; set; } = null!;
}