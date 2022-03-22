using NorthwindDatabase;

namespace NorthwindMVC.Models;
public record HomeIndexViewModel
(
    int VisitorCount,
    IList<Category> Categories,
    IList<Product> Products
);