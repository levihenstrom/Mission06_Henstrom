using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission06_Henstrom.Models;

namespace Mission06_Henstrom.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
}