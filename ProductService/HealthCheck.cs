using Core.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace ProductService;

[DynamicApi]
public class HealthCheck
{
    [HttpGet("/HealthCheck")]
    public string Get()
    {
        return "成功";
    }
}