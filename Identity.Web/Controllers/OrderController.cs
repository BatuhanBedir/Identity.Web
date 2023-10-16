using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers;

public class OrderController : Controller
{
    [Authorize(Policy = "Permission.Order.Read")]
    public IActionResult Index()
    {
        return View();
    }
}