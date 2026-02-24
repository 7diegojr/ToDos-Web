using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Models;

namespace ToDoPlatform.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{

    public AppDbContext (DbContextOptions<AppDbContext> opt):base(opt)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Renomeando as tabelas do banco
        base.OnModelCreating(builder);

        #region Configuração das Tabelas do Identity
        builder.Entity<AppUser>().ToTable("users");
        builder.Entity<IdentityRole>().ToTable("roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims"); // Regra para cada usuário
        builder.Entity<IdentityUserToken<string>>().ToTable("user_token");
        builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("lore_claims"); // Regra para um grupo de usuário
        #endregion

    }

}