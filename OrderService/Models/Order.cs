using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    /// <summary>
    /// 下单时间
    /// </summary>
    [Required]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 产品ID
    /// </summary>
    [Required]
    public int ProductID { get; set; }

    /// <summary>
    /// 购买数量
    /// </summary>
    [Required]
    public int Count { get; set; }
}
