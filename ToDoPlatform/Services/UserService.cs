using Microsoft.AspNetCore.Identity;
using ToDoPlatform.Data;
using ToDoPlatform.Models;

namespace ToDoPlatform.Services;

public class UserService
{
    // readonly: garante que o campo só pode ser atribuído no construtor.
    private readonly SignInManager<AppUser> _signInManager; // Login/Logout
    private readonly UserManager<AppUser> _userManagaer; // Gerencia de users e roles
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
        _userManagaer = userManager;
        _logger = logger;
    }
}