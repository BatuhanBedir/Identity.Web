using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.CustomValidations;

public class PasswordValidator : IPasswordValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
    {
        var errors = new List<IdentityError>();
        if (password!.ToLower().Contains(user.UserName!.ToLower()))
        {
            errors.Add(new(){Code = "PasswordContainUserName",Description = "Şifre alanı kullanıcı adı içeremez"});
        }
        if (password!.ToLower().StartsWith("1234"))
        {
            errors.Add(new(){Code = "PasswordContain1234",Description = "Şifre alanı ardışık sayı içeremez"});
        }

        return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
    }
}