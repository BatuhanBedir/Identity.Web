using Identity.Web.Extensions;
using Identity.Web.Models;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Controllers;

[Authorize]
public class MemberController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
        var userViewModel = new UserViewModel
        {
            Email = currentUser!.Email,
            UserName = currentUser!.UserName,
            PhoneNumber = currentUser!.PhoneNumber
        };

        return View(userViewModel);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public IActionResult PasswordChange()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PasswordChange(PasswordChangeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

        var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, model.PasswordOld);
        if (!checkOldPassword)
        {
            ModelState.AddModelError(String.Empty, "Eski şifreniz yanlış");
            return View();
        }

        var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordNew);

        if (!resultChangePassword.Succeeded)
        {
            ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x=>x.Description).ToList());
            return View();
        }
        
        await _userManager.UpdateSecurityStampAsync(currentUser);
        await _signInManager.SignOutAsync();
        await _signInManager.PasswordSignInAsync(currentUser, model.PasswordNew, true, false);

        TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";

        return View();
    }
}