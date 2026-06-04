using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InquiryDesk.Controllers;

public sealed class HomeController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        ViewData["ErrorMessage"] = exception?.Message;
        return View();
    }
}
