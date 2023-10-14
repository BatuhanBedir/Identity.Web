using Identity.Web.Extensions;
using Identity.Web.Models;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Identity.Web.Controllers;

[Authorize]
public class MemberController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileProvider _fileProvider;

    public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IFileProvider fileProvider)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _fileProvider = fileProvider;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
        var userViewModel = new UserViewModel
        {
            Email = currentUser!.Email,
            UserName = currentUser!.UserName,
            PhoneNumber = currentUser!.PhoneNumber,
            PictureUrl = currentUser.Picture
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

        var resultChangePassword =
            await _userManager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordNew);

        if (!resultChangePassword.Succeeded)
        {
            ModelState.AddModelErrorList(resultChangePassword.Errors);
            return View();
        }

        await _userManager.UpdateSecurityStampAsync(currentUser);
        await _signInManager.SignOutAsync();
        await _signInManager.PasswordSignInAsync(currentUser, model.PasswordNew, true, false);

        TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";

        return View();
    }

    public async Task<IActionResult> UserEdit()
    {
        ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
        var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
        var userEditViewModel = new UserEditViewModel()
        {
            UserName = currentUser.UserName!,
            Email = currentUser.Email!,
            Phone = currentUser.PhoneNumber!,
            BirthDate = currentUser.BirthDate,
            City = currentUser.City,
            Gender = currentUser.Gender,
        };

        return View(userEditViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UserEdit(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

        currentUser.UserName = model.UserName;
        currentUser.City = model.City;
        currentUser.Email = model.Email;
        currentUser.BirthDate = model.BirthDate;
        currentUser.Gender = model.Gender;
        currentUser.PhoneNumber = model.Phone;

        if (model.Picture is not null && model.Picture.Length > 0)
        {
            var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Picture.FileName)}";

            var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!,
                randomFileName);
            using var stream = new FileStream(newPicturePath, FileMode.Create);
            await model.Picture.CopyToAsync(stream);
            currentUser.Picture = randomFileName;
        }

        var updateToUserResult = await _userManager.UpdateAsync(currentUser);
        if (!updateToUserResult.Succeeded)
        {
            ModelState.AddModelErrorList(updateToUserResult.Errors);
            return View();
        }

        await _userManager.UpdateSecurityStampAsync(currentUser);
        await _signInManager.SignOutAsync();
        await _signInManager.SignInAsync(currentUser,true);
        
        TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir";
        
        return View();
    }

    public async Task<IActionResult> AccessDenied(string returnUrl)
    {
        string message = string.Empty;
        message = "Bu sayfayı görmeye yetkiniz yok.";

        ViewBag.message = message;
        
        return View();
    }
}