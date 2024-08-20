namespace ProductService.Models;

public class CreateOrderMessageDto
{
    /// <summary>
    /// 产品ID
    /// </summary>
    public int ProductID { get; set; }

    /// <summary>
    /// 购买数量
    /// </summary>
    public int Count { get; set; }
}