using System.Diagnostics;
using BookApp.Data.EF.Access.Contexts;
using BookApp.DataGenerators;
using Microsoft.AspNetCore.Mvc;
using BookApp.Models;

namespace BookApp.Controllers;

public class HomeController : Controller
{
    private readonly MainDbContext mainDbContext;
    private readonly ILogger<HomeController> logger;

    public HomeController(MainDbContext mainDbContext, ILogger<HomeController> logger)
    {
        this.mainDbContext = mainDbContext;
        this.logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        await DataGenerator.CreateTagsForBooks(mainDbContext);
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}