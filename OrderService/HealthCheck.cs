using Core.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace OrderService;

[DynamicApi]
public class HealthCheck
{
    [HttpGet("/HealthCheck")]
    public string Get()
    {
        return "成功";
    }
}