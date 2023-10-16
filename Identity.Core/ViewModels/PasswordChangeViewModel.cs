using System.ComponentModel.DataAnnotations;

namespace Identity.Web.ViewModels;

public record PasswordChangeViewModel
{
    [DataType(DataType.Password)]
    [Display(Name = "Eski Şifre: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
    public string PasswordOld { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
    public string PasswordNew { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler birbiriyle uyuşmuyor")]
    [Display(Name = "Yeni Şifre Tekrar: ")]
    [Required(ErrorMessage = "Bu alan boş geçilemez")]
    [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
    public string PasswordNewConfirm { get; set; } = null!;
}