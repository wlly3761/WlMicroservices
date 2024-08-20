using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Service;
using ProductService.Models;

namespace ProductService.Controller;

[Route("Product")]
public class ShopController:ControllerBase
{
    private ProductContext _context;
    private IProductService _productService;
    
    public ShopController(ProductContext context,IProductService productService)
    {
        _context = context;
        _productService = productService;
    }
    [HttpPost]
    public async Task ReduceStock(CreateOrderMessageDto message)
    {
        _productService.ReduceStock(message);
    }
}