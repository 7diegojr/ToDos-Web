using Microsoft.AspNetCore.Mvc;
using ToDoPlatform.Services;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;

    // Injeção de Dependência
    public AccountController(ILogger<AccountController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    // Traz a página para o usuário
    public IActionResult Login(string returnUrl = null)
    {
        // O usuário ja está logado(manda para a home)
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        // Página que o usuário estava tentando acessar
        var model = new LoginVM
        {
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.Login(login);

            if (result.Succeeded)
                return LocalRedirect(login.ReturnUrl); // redireciona para a página que o usuário queria abrir
            if (result.IsLockedOut)
                return RedirectToAction("Lockout");
            if (result.IsNotAllowed)
                return RedirectToAction("AccessDenied");
            ModelState.AddModelError("", "Usuário e/ou Senha inválidos");
        }
        return View(login);

    }



    public IActionResult Logout()
    {
        // Fazer o logout
        return RedirectToAction("Login");
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}