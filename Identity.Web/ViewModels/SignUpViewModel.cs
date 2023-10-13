using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels;

public record SignUpViewModel
{
    [Display(Name = "Kullanıcı Adı: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string UserName { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email formatı yanlış")]
    [Display(Name = "Email: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string Email { get; set; } = null!;

    [Display(Name = "Telefon: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    public string Phone { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Şifre: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [MinLength(6,ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Şifreler birbiriyle uyuşmuyor")]
    [Display(Name = "Şifre Tekrar: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [MinLength(6,ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
    public string PasswordConfirm { get; set; } = null!;
}