namespace ToDoPlatform.ViewModels;

// representa o usuário autenticado
// usada para trafegar os dados do usuário logado pela aplicação sem expor diretamente o AppUser
public class UserVM
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
    public string Roles { get; set; }
    public bool IsAdmin { get; set; } = false; // facilita verificações condicionais nas Views (Ex:ocultar menus admin)
}
// expõe apenas as informações necessárias para a interface