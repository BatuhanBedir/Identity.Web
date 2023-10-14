using System.Security.Claims;
using Identity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.ClaimProviders;

public class UserClaimProvider : IClaimsTransformation
{
    private readonly UserManager<AppUser> _userManager;

    public UserClaimProvider(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    //tabloda var olan değeri claims tablosuna bir daha taşımamak için
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identityUser = principal.Identity as ClaimsIdentity;
        var currentUser = await _userManager.FindByNameAsync(identityUser.Name);


        if (string.IsNullOrEmpty(currentUser.City)) return principal;

        if (principal.HasClaim(x => x.Type != "city"))
        {
            Claim cityClaim = new Claim("city", currentUser.City);
            identityUser.AddClaim(cityClaim);
        }

        return principal;
    }
}