using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission06_Henstrom.Models;

namespace Mission06_Henstrom.Controllers;

public class HomeController : Controller
{
    private MovieContext _context;
    
    public HomeController(MovieContext temp) //Constructor
    {
        _context = temp;
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
        return View("MovieEntry");
    }

    [HttpPost]
    public IActionResult EnterMovie(Movie response)
    {
        if (!ModelState.IsValid)
        {
            return View("MovieEntry", response);
        }

        _context.Movies.Add(response); //add record to database
        _context.SaveChanges();
        
        return View("Confimration", response);
    }   
    
}
