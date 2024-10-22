using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LionTaskManagementApp.Models;

namespace LionTaskManagementApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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

    // This is the "View My Task" function
    public IActionResult ViewMyTask()
    {
        // Check if the user has the role "Poster"
        if (User.IsInRole("Poster"))
        {
            // Redirect to the PosterIndex in TasksController
            return RedirectToAction("PosterIndex", "Tasks");
        }

        // Check if the user has the role "Taker"
        if (User.IsInRole("Taker"))
        {
            // Redirect to the TakerIndex in TasksController
            return RedirectToAction("TakerIndex", "Tasks");
        }

        // If the user doesn't have either role, redirect to the homepage or access denied
        return RedirectToAction("Index", "Home");
    }
}
