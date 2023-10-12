using Identity.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Identity.Web.Extensions;

namespace Identity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid) return View();

            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = signUpViewModel.UserName,
                PhoneNumber = signUpViewModel.Phone,
                Email = signUpViewModel.Email,
            }, signUpViewModel.Password);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir.";
                return RedirectToAction();
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());


            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }

            var signInResult =
                await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamazsınız" });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış",$"Başarısız giriş sayısı: {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}