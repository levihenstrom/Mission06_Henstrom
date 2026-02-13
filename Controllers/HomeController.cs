using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission06_Henstrom.Models;

namespace Mission06_Henstrom.Controllers;

public class HomeController : Controller
{
    private readonly MovieContext _context;

    public HomeController(MovieContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetToKnowJoel()
    {
        return View("GetToKnowJoel");
    }

    [HttpGet]
    public IActionResult EnterMovie()
    {
        PopulateCategoryList();
        return View("MovieEntry", new Movie());
    }

    [HttpPost]
    public IActionResult EnterMovie(Movie response)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Add(response);
            _context.SaveChanges();

            return View("Confimration", response);
        }

        PopulateCategoryList(response.CategoryId);
        return View("MovieEntry", response);
    }

    [HttpGet]
    public IActionResult MovieList()
    {
        var movies = _context.Movies
            .Include(m => m.Category)
            .OrderBy(m => m.Title)
            .ToList();

        return View(movies);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var movie = _context.Movies.SingleOrDefault(m => m.MovieId == id);

        if (movie == null)
        {
            return NotFound();
        }

        PopulateCategoryList(movie.CategoryId);
        return View("MovieEntry", movie);
    }

    [HttpPost]
    public IActionResult Edit(Movie response)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Update(response);
            _context.SaveChanges();

            return RedirectToAction(nameof(MovieList));
        }

        PopulateCategoryList(response.CategoryId);
        return View("MovieEntry", response);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var movie = _context.Movies
            .Include(m => m.Category)
            .SingleOrDefault(m => m.MovieId == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost]
    public IActionResult Delete(Movie movie)
    {
        _context.Movies.Remove(movie);
        _context.SaveChanges();

        return RedirectToAction(nameof(MovieList));
    }

    private void PopulateCategoryList(int? selectedCategoryId = null)
    {
        var categories = _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToList();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = selectedCategoryId;
    }
}
