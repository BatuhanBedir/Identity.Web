using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Areas.Admin.Controllers;

[Authorize(Roles = "admin")]
[Area("Admin")]
public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> UserList()
    {
        var users = await _userManager.Users.ToListAsync();

        var userVm = users.Select(x => new UserViewModel()
        {
            Id = x.Id,
            Email = x.Email,
            UserName = x.UserName
        }).ToList();
        
        return View(userVm);
    }
}