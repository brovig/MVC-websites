using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> Index()
    {
        HomeIndexViewModel model = new
        (
            VisitorCount: (new Random()).Next(1, 1001),
            Categories: await db.Categories.ToListAsync(),
            Products: await db.Products.Where(p => p.UnitsInStock < 5 && p.UnitsInStock > 0).ToListAsync()
        );
        return View(model);
    }

    public async Task<IActionResult> Products()
    {
        IEnumerable<Product> model = await db.Products.ToListAsync();

        if (model == null)
        {
            return NotFound("No products in stock");
        }

        return View(model);
    }

    public async Task<IActionResult> Categories()
    {
        IEnumerable<Category> model = await db.Categories.ToListAsync();

        if (model == null)
        {
            return NotFound("Hm, there should have been some categories here, but there aren't any");
        }

        return View(model);
    }

    public async Task<IActionResult> ProductDetail(int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/27");
        }

        Product? model = await db.Products.SingleOrDefaultAsync(p => p.ProductId == id);
        
        if (model == null)
        {
            return NotFound($"ProductID {id} not found.");
        }

        return View(model);
    }

    public async Task<IActionResult> Category(int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("You must pass a category ID in the route, for example, /Home/Category/1");
        }

        IEnumerable<Product> model = await db.Products.Include(p => p.Supplier).Where(p => p.CategoryId == id).ToListAsync();

        if (model == null)
        {
            return NotFound($"CategoryID {id} not found.");
        }

        ViewData["CategoryName"] = db.Categories.SingleOrDefault(c => c.CategoryId == id).CategoryName.ToString();
        return View(model);
    }

    public async Task<IActionResult> ProductsThatCostMoreThan(decimal? price)
    {
        if (!price.HasValue)
        {
            return BadRequest("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
        }

        IEnumerable<Product> model = await db.Products.Include(p => p.Category).Include(p => p.Supplier).Where(p => p.UnitPrice > price).ToListAsync();

        if (!model.Any())
        {
            return NotFound($"No products cost more than {price:C}.");
        }

        ViewData["MaxPrice"] = price.Value.ToString("C");
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
