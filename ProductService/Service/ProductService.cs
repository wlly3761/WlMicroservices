using Core.Attribute;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace OrderService.Service;

[DynamicApi(ServiceLifeCycle = "Transient")]
public class ProductService:IProductService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ProductService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ProductContext _context;

    public ProductService(ILogger<ProductService> logger, IConfiguration configuration,  ProductContext context, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<string> GetProductAsync()
    {
        return Task.FromResult( $"【产品服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                                $"{_httpContextAccessor.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}");
    }

    /// <summary>
    /// 减库存 订阅下单事件
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [NonAction]
    [CapSubscribe("order.services.createorder")]
    public async Task ReduceStock(CreateOrderMessageDto message)
    {
        Console.WriteLine("订阅事件成功");
        //业务代码
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == message.ProductID);
        product.Stock -= message.Count;
    
        await _context.SaveChangesAsync();
    }
}