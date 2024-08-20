using System.Collections.Concurrent;
using Consul;
using RestSharp;

namespace MicroServicesWeb.ServiceHelper;
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrder()
        {

            //每次随机访问一个服务实例
            var Client = new RestClient(_configuration["ConsulSetting:APIGatewayAddress"]);
            var request = new RestRequest("/orders/GetOrder");

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {

            //每次随机访问一个服务实例
            var Client = new RestClient(_configuration["ConsulSetting:APIGatewayAddress"]);
            var request = new RestRequest("/products/GetProduct");

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        // public void GetServices()
        // {
        //     var serviceNames = new[] { "OrderService", "ProductService" };
        //     Array.ForEach(serviceNames, p =>
        //     {
        //         Task.Run(() =>
        //         {
        //             //WaitTime默认为5分钟
        //             var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromMinutes(10) };
        //             while (true)
        //             {
        //                 GetServices(queryOptions, p);
        //             }
        //         });
        //     });
        // }
        // private void GetServices(QueryOptions queryOptions, string serviceName)
        // {
        //     var res = _consulClient.Health.Service(serviceName, null, true, queryOptions).Result;
        //     
        //     //控制台打印一下获取服务列表的响应时间等信息
        //     Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");
        //
        //     //版本号不一致 说明服务列表发生了变化
        //     if (queryOptions.WaitIndex != res.LastIndex)
        //     {
        //         queryOptions.WaitIndex = res.LastIndex;
        //
        //         //服务地址列表
        //         var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();
        //
        //         if (serviceName == "OrderService")
        //             _orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
        //         else if (serviceName == "ProductService")
        //             _productServiceUrls = new ConcurrentBag<string>(serviceUrls);
        //     }
        // }
    }
