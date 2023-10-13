using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.CustomValidations;

public class UserValidator : IUserValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
    {
        var errors = new List<IdentityError>();
        var isNumeric = int.TryParse(user.UserName[0].ToString(), out _);
        if(isNumeric)
            errors.Add(new (){Code = "UserNameContainFirstLetterDigit",Description = "Kulanıcı karakterin ilk karakteri sayısal karakter olamaz"});
        return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
    }
}