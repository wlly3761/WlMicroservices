using System.Diagnostics;
using MicroServicesWeb.ServiceHelper;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicesWeb.Controller;

[Controller]
public class HomeController:Microsoft.AspNetCore.Mvc.Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceHelper _serviceHelper;

    public HomeController(ILogger<HomeController> logger, IServiceHelper serviceHelper)
    {
        _logger = logger;
        _serviceHelper = serviceHelper;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.OrderData = await _serviceHelper.GetOrder();
        ViewBag.ProductData = await _serviceHelper.GetProduct();
        return View();
    }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new  { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
}