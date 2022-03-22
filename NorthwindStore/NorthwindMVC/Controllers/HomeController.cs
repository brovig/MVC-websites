﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NorthwindDatabase;
using NorthwindMVC.Models;

namespace NorthwindMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext db;

    public HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext)
    {
        _logger = logger;
        db = injectedContext;
    }

    public IActionResult Index()
    {
        HomeIndexViewModel model = new
        (
            VisitorCount: (new Random()).Next(1, 1001),
            Categories: db.Categories.ToList(),
            Products: db.Products.ToList()
        );
        return View(model);
    }

    public IActionResult ProductDetail(int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/27");
        }

        Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
        
        if (model == null)
        {
            return NotFound($"ProductID {id} not found.");
        }

        return View(model);
    }

    [Route("private")]
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