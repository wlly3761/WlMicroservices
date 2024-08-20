namespace MicroServicesWeb.ServiceHelper;

public interface IServiceHelper
{
     Task<string> GetOrder();
     Task<string> GetProduct();
     /// <summary>
     /// 获取服务列表
     /// </summary>
     // void GetServices();
}