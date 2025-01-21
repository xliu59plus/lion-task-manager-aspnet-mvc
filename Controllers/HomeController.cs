using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Models.Constants;

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
        if (User.IsInRole(RoleConstants.Poster))
        {
            // Redirect to the PosterIndex in TasksController
            return RedirectToAction("Index", "Poster");
        }

        // Check if the user has the role "Contractor"
        if (User.IsInRole(@RoleConstants.Taker))
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
        if(User.IsInRole(RoleConstants.Admin))
        {
            // Redirect to the PosterIndex in TasksController
            return RedirectToAction("Index", "Admin");
        } else if (User.IsInRole(RoleConstants.Poster) || User.IsInRole(@RoleConstants.Taker))
        {
            TempData["SuccessMessage"] = "You are an active user now"; 
            TempData.Keep("SuccessMessage");
            return RedirectToAction("Index", "Home");
        } 
        else if(User.IsInRole(RoleConstants.InactivePoster)) {
            return RedirectToAction("EditProfile", "Poster");
        } 
        else if(User.IsInRole(@RoleConstants.InactiveTaker)) {
            return RedirectToAction("EditProfile", "Taker");
        }

        Console.WriteLine("HomeController: CompleteProfile: User doesn't have a proper role");

        // If the user doesn't have either role, redirect to the homepage or access denied
        return RedirectToAction("Index", "Home");
    }
}
