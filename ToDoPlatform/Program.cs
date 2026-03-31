using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoPlatform.Data;
using ToDoPlatform.Models;
using ToDoPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Serviço de conexão com o Banco de Dados
string conexao = builder.Configuration.GetConnectionString("Conexao");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySQL(conexao)
);

// Serviço de configuração do Identity - Gestão de Usuários
builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt =>
    {
        opt.User.RequireUniqueEmail = true; // Não deixa repetir email
        opt.SignIn.RequireConfirmedAccount = false; // Não precisa confirmar o email para usar
        // Pode configurar opções de senha, etc
    }
)
.AddEntityFrameworkStores<AppDbContext>() // Onde e como guardar os dados
.AddDefaultTokenProviders(); // Geração automática de tokens

// Registro do serviço de usuário
// AddTransient(1 dos 3 tipos): uma nova instância do UserService será criada a cada vez que ele for solicitado
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Garante a existência do Banco
using (var scope = app.Services.CreateScope())
{
    var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
