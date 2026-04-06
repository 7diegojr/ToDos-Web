using Microsoft.AspNetCore.Mvc;
using ToDoPlatform.Services;
using ToDoPlatform.ViewModels;

namespace Vereda_Cafeteria.Controllers;

// gerencia todas as ações relacionadas à conta do usuário
public class AccountController : Controller // dá acesso a View(), RedirectToAction(), TempData, ModelState, etc.
{
    // Dependências injetadas — readonly: só podem ser atribuídas no construtor
    private readonly ILogger<AccountController> _logger; // Registro e logs
    private readonly IUserService _userService; // Lógica de login/logout

    // Construtor — o ASP.NET injeta automaticamente via DI Container
    public AccountController(ILogger<AccountController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet] // responde quando o usuário ACESSA a página /Account/Login pelo navegador
    public IActionResult Login(string returnUrl)
    {
        // Cria o ViewModel já com a URL de retorno preenchida
        // returnUrl: ASP.NET passa automaticamente quando o usuário tenta acessar
        // uma página protegida por [Authorize] sem estar logado
        LoginVM loginVM = new()
        {
            // Se returnUrl for null (acesso direto ao login), usa a raiz do site "~/"
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };

        // Envia o ViewModel para a View — ela usa para montar o formulário
        return View(loginVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Valida o token oculto gerado automaticamente no formulário (proteção CSRF)
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (ModelState.IsValid) // verifica se os campos passaram nas validações do ViewModel
        {
            // Atribui a responsabilidade da autenticação ao UserService
            // O Controller só sabe o resultado e não como funciona
            var result = await _userService.Login(loginVM);

            // Analisa o resultado e define a mensagem de feedback para o usuário
            // TempData: dicionário que sobrevive a apenas UMA requisição
            // Útil para exibir mensagens após redirecionamentos
            if (result.Succeeded)
                TempData["Success"] = "Login realizado com sucesso! Redirecionando...";

            else if (result.IsLockedOut)
                TempData["Failure"] = "Usuário bloqueado por muitas tentativas.";
            // Conta bloqueada após N tentativas erradas (configurado no Program.cs)

            else if (result.IsNotAllowed)
                TempData["Failure"] = "Usuário sem permissão para acessar o sistema.";
            // Ex: e-mail não confirmado (se RequireConfirmedAccount = true)

            else
                TempData["Failure"] = "E-mail ou senha incorretos. Tente novamente";
            // Credenciais simplesmente erradas
        }
        else
            // O ViewModel chegou com dados inválidos (campo vazio, formato errado, etc.)
            TempData["Failure"] = "Dados inválidos. Verifique os campos preenchidos.";

        // Retorna a mesma View com o loginVM — mantém os dados preenchidos no formulário
        // e permite que a View exiba as mensagens do TempData
        return View(loginVM);
    }

    // Logout só aceita POST — nunca GET
    [HttpPost]
    [ValidateAntiForgeryToken] // também com proteção CSRF
    public async Task<IActionResult> Logout()
    {
        // // Atribui a responsabilidade de logout ao UserService (que chama SignOutAsync internamente)
        await _userService.Logout();

        // Após deslogar, redireciona para a tela de login
        // RedirectToAction("Action", "Controller") — sem o sufixo "Controller"
        return RedirectToAction("Login", "Account");
    }

    // Ações ainda sem implementação — retornam apenas a View correspondente
    // Serão desenvolvidas futuramente (cadastro e perfil do usuário)
    public IActionResult Register() => View();
    public IActionResult Profile() => View();
}