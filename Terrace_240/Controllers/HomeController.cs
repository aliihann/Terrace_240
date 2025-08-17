using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terrace_240.Models;

namespace Terrace_240.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MainContext _db;


        public HomeController(ILogger<HomeController> logger, MainContext db)
        {
            _logger = logger;
            _db = db;
        }

                                                       //жанр    //рейтинг
        public async Task<IActionResult> Index(string? genre, string? sort)
        {
            var q = _db.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(genre) && genre != "all")
                q = q.Where(m => m.Genre == genre);

            q = sort switch
            {
                "title" => q.OrderBy(m => m.Genre),
                "year" => q.OrderByDescending(m => m.Year),
                _ => q.OrderByDescending(m => m.Rating)
            };

            var list = await q.ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return PartialView("_DetailsPartial", movie);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
