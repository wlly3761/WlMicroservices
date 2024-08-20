
using OrderService.Models;

namespace OrderService.Service;

public interface IOrderService
{
    Task<string> GetOrderAsync();

    Task<string> CreateOrder(Order order);
}