using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Data;
using ToDoPlatform.Models;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

public class UserService : IUserService
{
    // readonly: garante que o campo só pode ser atribuído no construtor.
    private readonly SignInManager<AppUser> _signInManager; // Login/Logout
    private readonly UserManager<AppUser> _userManager; // Gerencia de users e roles
    private readonly ILogger<UserService> _logger; // Sistema de logs
    private readonly AppDbContext _dbContext; // Acesso direto ao banco
    private readonly IHttpContextAccessor _httpContextAccessor; // Acessa a requisição HTTP atual

    // O ASP.NET lê este construtor e injeta automaticamente tudo que estiver
    // registrado no DI Container (injeção de dependências)
    public UserService(
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<UserService> logger
    )
    {
        // Cada parâmetro é atribuído ao campo privado correspondente
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    // Retorna os dados do usuário que está atualmente autenticado
    public async Task<UserVM> GetLoggedUser()
    {
        // Lê o ID do usuário a partir das Claims do cookie de autenticação
        // Claims: fichas com informações do usuário, gravadas no cookie após o login
        var userId = _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier); // campo padrão que armazena o ID

        if (userId == null) return null; // obs: O Controller que chamar este método deve tratar o null


        // Busca o usuário no banco pelo ID
        var user = await _dbContext.AppUsers
            .SingleOrDefaultAsync(u => u.Id == userId); // retorna 1 resultado ou null

        // Busca todas as roles do usuário e transforma em uma string
        var roles = string.Join(", ", await _userManager.GetRolesAsync(user));
        // Verifica se possui a role de Administrador (retorna bool)
        var isAdmin = await _userManager.IsInRoleAsync(user, "Administrador");

        // Monta e retorna o ViewModel com os dados relevantes para a View
        return new UserVM()
        {
            Id = userId,
            Name = user.Name,
            ProfilePicture = user.ProfilePicture,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles,
            IsAdmin = isAdmin
        };
    }

    // Método de login — recebe os dados do formulário via LoginVM
    public async Task<SignInResult> Login(LoginVM login)
    {
        // usa o e-mail como userName para a tentativa de login
        string userName = login.Email;
        // Tenta encontrar o usuário pelo e-mail para obter o UserName real
        var user = await _userManager.FindByEmailAsync(login.Email);
        // Se o usuário existir, usa o UserName cadastrado
        if (user != null) userName = user.UserName;

        // Tenta autenticar com senha
        // lockoutOnFailure: true → bloqueia a conta após várias tentativas erradas
        var result = await _signInManager.PasswordSignInAsync(
            userName, login.Password, login.RememberMe,
            lockoutOnFailure: true
        );

        // Loga informação de sucesso
        if (result.Succeeded)
            _logger.LogInformation($"Usuário '{userName}' acessou o sistema");

        // Loga aviso se a conta estiver bloqueada
        if (result.IsLockedOut)
            _logger.LogWarning($"Usuário '{userName}' está bloqueado!");

        // Retorna o result p/ o Controller decidir o que fazer (redireciojnar, mostrar erro)
        return result;
    }

    // Método de logout — encerra a sessão do usuário
    public async Task Logout()
    {
        // Registra o evento no log
        _logger.LogInformation($"Usuário saiu do sistena");

        // Remove o cookie/sessão de autenticação
        await _signInManager.SignOutAsync();
    }
}