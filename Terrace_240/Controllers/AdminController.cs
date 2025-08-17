using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Terrace_240.Services;
using Microsoft.EntityFrameworkCore;

namespace Terrace_240.Controllers
{
    public class AdminController : Controller
    {
        private readonly MainContext _db;
        private readonly IFileService _files;

        public AdminController(MainContext db, IFileService files)
        {
            _db = db; 
            _files = files; 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string password)
        {
            var entry = await _db.AdminPasswords.FirstOrDefaultAsync(a => a.Id != 0);
            if (entry != null && BCrypt.Net.BCrypt.Verify(password ?? "", entry.PasswordHash))
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "admin")
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(identity));
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Неверный пароль";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var movies = await _db.Movies.OrderByDescending(m => m.Id).ToListAsync();
            return View(movies);
        }

        [Authorize]
        public IActionResult Create() => View(new Movie());

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Movie model, IFormFile? poster)
        {
            //if (!ModelState.IsValid) return View(model);
            if (poster != null) model.PosterUrl = await _files.SavePosterAsync(poster) ?? "";
            _db.Movies.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie model, IFormFile? poster)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            //if (!ModelState.IsValid) return View(model);

            movie.Title = model.Title;
            movie.Year = model.Year;
            movie.Genre = model.Genre;
            movie.Rating = model.Rating;
            movie.Description = model.Description;
            movie.ScheduleTimes = model.ScheduleTimes;
            movie.Price = model.Price;

            if (poster != null)
            {
                _files.DeleteIfExists(movie.PosterUrl);
                movie.PosterUrl = await _files.SavePosterAsync(poster) ?? "";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            _files.DeleteIfExists(movie.PosterUrl);
            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult ChangePassword() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var entry = await _db.AdminPasswords.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            if (entry == null || !BCrypt.Net.BCrypt.Verify(currentPassword ?? "", entry.PasswordHash))
            {
                ViewBag.Error = "Текущий пароль неверен";
                return View();
            }
            entry.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword ?? "");
            entry.CreatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            ViewBag.Message = "Пароль изменён";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
