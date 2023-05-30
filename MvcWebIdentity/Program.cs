using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcWebIdentity.Context;
using MvcWebIdentity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

ConnectionString(builder);
AddIdentity(builder);
ConfiguriLogin(builder);
AddCookei(builder);
AddAuthorizationRoles(builder);
AddRoleUser(builder);

void ConnectionString(WebApplicationBuilder builder)
{
    var connection = builder.Configuration.GetConnectionString("DefoultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connection));
}
void AddIdentity(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
}
void ConfiguriLogin(WebApplicationBuilder builder)
{
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequiredLength = 10;
        options.Password.RequiredUniqueChars = 3;
        options.Password.RequireNonAlphanumeric = false;
    });
}
void AddCookei(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "AspNetCore.Cookies";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.SlidingExpiration = true;
        });
}
void AddAuthorizationRoles(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(option =>
    {
        option.AddPolicy("RequireUserAdminGerente",
            policy => policy.RequireRole("User", "Admin", "Gerente"));
    });
}
void AddRoleUser(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

await CriarPerfisUsuariosAsync(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task CriarPerfisUsuariosAsync(WebApplication app)
{
    var scopedFatory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFatory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
        await service.SeedRolesAsync();
        await service.SeedUsersAsync();
    }

}