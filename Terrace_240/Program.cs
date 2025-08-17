using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Terrace_240.Services;
using Terrace_240.DAL.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration["ConnectionStrings:Db"];
builder.Services.AddDbContext<MainContext>(servcie =>
{
    servcie.UseSqlServer(connectionString);
});

// Simple cookie auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.Cookie.Name = "Tarrace240Admin";
    });

builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainContext>();
    db.Database.EnsureCreated();

    // Seed admin password if not exists
    if (!db.AdminPasswords.Any())
    {
        db.AdminPasswords.Add(new AdminPassword
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            CreatedAt = DateTime.UtcNow
        });
        db.SaveChanges();
    }

    // Seed movies if empty
    if (!db.Movies.Any())
    {
        db.Movies.AddRange(new[]
        {
            new Movie { Title="Интерстеллар", Year=2014, Genre="sci-fi", Rating=8.6, PosterUrl="", Description="Фантастический фильм.", ScheduleTimes="20:00,22:30", Price=300 },
            new Movie { Title="Побег из Шоушенка", Year=1994, Genre="drama", Rating=9.3, PosterUrl="", Description="Драма о надежде.", ScheduleTimes="19:30,22:00", Price=300 }
        });
        db.SaveChanges();
    }
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
