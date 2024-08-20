using ProductService.Models;

namespace OrderService.Service;

public interface IProductService
{
    Task<string> GetProductAsync();
    Task ReduceStock(CreateOrderMessageDto message);
}