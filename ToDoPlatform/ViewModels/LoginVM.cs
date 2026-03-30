using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoPlatform.ViewModels;

//ViewModels: transporta dados entre a View e o Controller(moldadas conforme a necessidade de cada tela)

public class LoginVM // dados do formulário de login
{
    [Display(Name = "E-mail", Prompt = "seu@email.com")]
    [Required(ErrorMessage = "O e-mail de acesso é obrigatório")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Senha", Prompt = "********")]
    [Required(ErrorMessage = "A senha de acesso é obrigatória!")]
    public string Password { get; set; }
    
    [Display(Name = "Manter conectado?")]
    public bool RememberMe { get; set; } = false;

    [HiddenInput] // armazena para qual página o usuário deve ser redirecionado após o login
    public string ReturnUrl { get; set; }
}