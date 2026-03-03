using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Models;

namespace ToDoPlatform.Data;

public class AppDbSeed
{
    public AppDbSeed(ModelBuilder builder) // Construtor
    {
        #region Popular dados de Perfil de Usuário
        List<IdentityRole> roles = new()
        {
            new IdentityRole()
            {
                Id = "fee66274-eb99-4be9-b891-4fe65a91783a",
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole()
            {
                Id = "0781b0f7-3fe3-4ce0-a089-24879aab241c",
                Name = "Usuário",
                NormalizedName = "USUÁRIO"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Popular dados de Usuário
        List<AppUser> users = new()
        {
            new AppUser()
            {
                Id = "7c42a74e-53e4-4cd6-acd3-72d4c9b0e795",
                Email = "junihulk7@gmail.com",
                NormalizedEmail = "JUNIHULK7@GMAIL.COM",
                UserName = "junihulk7@gmail.com",
                NormalizedUserName = "JUNIHULK7@GMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                Name = "Diego Fernando dos Santos Junior",
                // ProfilePicture = "/img/users/7c42a74e-53e4-4cd6-acd3-72d4c9b0e795.png",
            },
            new AppUser()
            {
                Id = "f83236be-5cf6-4e48-9083-d46ffe50c32c",
                Email = "dfsntsjunior@gmail.com",
                NormalizedEmail = "DFSNTSJUNIOR@GMAIL.COM",
                UserName = "dfsntsjunior@gmail.com",
                NormalizedUserName = "DFSNTSJUNIOR@GMAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                Name = "Diego Fernando dos Santos Junior",
                // ProfilePicture = "/img/users/f83236be-5cf6-4e48-9083-d46ffe50c32c",
            },
        };
        foreach (var user in users) // Criar senha para cada usuário
        {
            PasswordHasher<IdentityUser> pass = new ();
            user.PasswordHash = pass.HashPassword(user, "123456");
        }
        builder.Entity<AppUser>().HasData(users);
        #endregion

        #region Popular Dados de Usuário Perfil
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>()
            {
                UserId = users[0].Id,
                RoleId = roles[0].Id, // O primeiro usuário é o Administrador
            },
            new IdentityUserRole<string>()
            {
                UserId = users[1].Id,
                RoleId = roles[1].Id, // O segundo usuário é Comum
            },
        };
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion

        #region Popular as Tarefas do Usuário
        List<ToDo> toDos = new()
        {
            new ToDo()
            {
                Id = 1,
                Title = "Fazer a redação da Meire",
                Description = "Finalizar até 09/03",
                UserId = users[0].Id
            },
            new ToDo()
            {
                Id = 2,
                Title = "Ler livro do seminário da Meire",
                Description = "Finalizar até 23/03",
                UserId = users[0].Id
            },
            new ToDo()
            {
                Id = 3,
                Title = "Terminar camada de dados do Vereda",
                Description = "Finalizar até o final de semana",
                UserId = users[1].Id
            },
        };
        builder.Entity<ToDo>().HasData(toDos);
        #endregion
    }
}