using Core.Attribute;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Service;

[DynamicApi]
public class OrderService:IOrderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ICapPublisher _capBus;
    private readonly OrderContext _context;

    public OrderService(ILogger<OrderService> logger, IConfiguration configuration, ICapPublisher capPublisher, OrderContext context, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _capBus = capPublisher;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<string> GetOrderAsync()
    {
        return Task.FromResult( $"【订单服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                                $"{_httpContextAccessor.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}");
    }


    /// <summary>
    /// 下单 发布下单事件
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> CreateOrder(Order order)
    {
        using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
        {
            //业务代码
            order.CreateTime = DateTime.Now;
            _context.Orders.Add(order);

            var r = await _context.SaveChangesAsync() > 0;

            if (r)
            {
                //发布下单事件
                await _capBus.PublishAsync("order.services.createorder", new CreateOrderMessageDto() { Count = order.Count, ProductID = order.ProductID });
                return "成功发布下单事件";
            }
            return "发布下单事件失败";
        }

    }
}