using Microsoft.AspNetCore.Identity;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Services;

// interface: o contrato que uma classe deve cumprir, sem determinar como cada operação será implementada
public interface IUserService
{
    // Task: operações assíncronas
    Task<UserVM> GetLoggedUser(); // obter o usuário logado
    Task<SignInResult> Login(LoginVM login); // realizar o login
    Task Logout(); // realizar o logout
}