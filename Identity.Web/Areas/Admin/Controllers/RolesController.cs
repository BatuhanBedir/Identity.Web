using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Identity.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Areas.Admin.Controllers;


[Authorize(Roles = "admin")]
[Area("Admin")]
public class RolesController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
        {
            Id = x.Id,
            Name = x.Name!
        }).ToListAsync();

        return View(roles);
    }

    [Authorize(Roles = "role-action")]
    public IActionResult RoleCreate()
    {
        return View();
    }

    [Authorize(Roles = "role-action")]
    [HttpPost]
    public async Task<IActionResult> RoleCreate(RoleCreateViewModel model)
    {
        var result = await _roleManager.CreateAsync(new AppRole() { Name = model.Name });

        if (!result.Succeeded)
        {
            ModelState.AddModelErrorList(result.Errors);
            return View();
        }

        TempData["SuccessMessage"] = "Rol oluşturulmuştur.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> RoleUpdate(string id)
    {
        var roleToUpdate = (await _roleManager.FindByIdAsync(id))!;

        return View(new RoleUpdateViewModel() { Id = roleToUpdate.Id, Name = roleToUpdate.Name });
    }

    [Authorize(Roles = "role-action")]
    [HttpPost]
    public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel model)
    {
        var roleToUpdate = await _roleManager.FindByIdAsync(model.Id);
        roleToUpdate.Name = model.Name;
        await _roleManager.UpdateAsync(roleToUpdate);

        ViewData["SuccessMessage"] = "Güncellenmiştir";

        return View();
    }

    [Authorize(Roles = "role-action")]
    public async Task<IActionResult> RoleDelete(string id)
    {
        var roleToDelete = await _roleManager.FindByIdAsync(id);
        var result = await _roleManager.DeleteAsync(roleToDelete);
        if (!result.Succeeded)
        {
            ModelState.AddModelErrorList(result.Errors);
        }

        TempData["SuccessMessage"] = "Rol silinmiştir";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> AssignRoleToUser(string id)
    {
        var currentUser = await _userManager.FindByIdAsync(id);
        ViewBag.userId = id;
        var roles = await _roleManager.Roles.ToListAsync();
        var roleViewModelList = new List<AssignRoleToUserViewModel>();
        var userRoles = await _userManager.GetRolesAsync(currentUser);

        foreach (var item in roles)
        {
            var assignRoleToUserViewModel = new AssignRoleToUserViewModel() { Id = item.Id, Name = item.Name };

            if (userRoles.Contains(item.Name))
            {
                assignRoleToUserViewModel.Exist = true;
            }

            roleViewModelList.Add(assignRoleToUserViewModel);
        }

        return View(roleViewModelList);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> modelList)
    {
        var userToAssignRoles = await _userManager.FindByIdAsync(userId);

        foreach (var item in modelList)
        {
            if (item.Exist)
            {
                await _userManager.AddToRoleAsync(userToAssignRoles, item.Name);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(userToAssignRoles, item.Name);
            }
        }

        return RedirectToAction("UserList", "Home");
    }
}