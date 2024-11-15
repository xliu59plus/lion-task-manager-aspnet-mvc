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
            return RedirectToAction("Index", "Poster");
        }

        // Check if the user has the role "Taker"
        if (User.IsInRole("Taker"))
        {
            // Redirect to the TakerIndex in TasksController
            return RedirectToAction("TakerIndex", "Taker");
        }

        // If the user doesn't have either role, redirect to the homepage or access denied
        return RedirectToAction("Index", "Home");
    }

    public IActionResult CompleteProfile()
    {
        // Check if the user has the role "Poster"
        if (User.IsInRole("Poster") || User.IsInRole("Taker"))
        {
            TempData["SuccessMessage"] = "You are an active user now"; 
            TempData.Keep("SuccessMessage");
            return RedirectToAction("Index", "Home");
        } 
        else if(User.IsInRole("Inactive_Poster")) {
            return RedirectToAction("EditProfile", "Poster");
        } 
        else if(User.IsInRole("Inactive_Taker")) {
            return RedirectToAction("EditProfile", "Taker");
        }

        Console.WriteLine("HomeController: CompleteProfile: User doesn't have a proper role");

        // If the user doesn't have either role, redirect to the homepage or access denied
        return RedirectToAction("Index", "Home");
    }
}
