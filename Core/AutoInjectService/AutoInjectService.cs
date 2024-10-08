﻿using Core.Attribute;
using Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AutoInjectService;
/// <summary>
/// 自动注入服务类
/// </summary>
public static class AutoInjectService
{
    /// <summary>
    /// 自动注入服务
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AutoRegistryService(this IServiceCollection serviceCollection,List<string> assemblyNames=null)
    {
        if (assemblyNames == null) return serviceCollection;
        var types = AssemblyHelper.GetAllAssemblies().Where(c=>
            assemblyNames.Contains(c.FullName)).SelectMany(c=>c.GetTypes());
        foreach (var serviceType in types)
            if (System.Attribute.IsDefined(serviceType, typeof(DynamicApiAttribute)))
            {
                var serviceRegistryAttribute =
                    (DynamicApiAttribute)serviceType.GetCustomAttributes(typeof(DynamicApiAttribute), false)
                        .FirstOrDefault()!;
                if (serviceRegistryAttribute == null) continue;
                var interfaces = serviceType.GetInterfaces();
                //获取首个接口
                var serviceInterfaceType = interfaces.FirstOrDefault();
                if (serviceInterfaceType == null) continue;
                switch (serviceRegistryAttribute.ServiceLifeCycle)
                {
                    case "Singleton":
                        serviceCollection.AddSingleton(serviceInterfaceType, serviceType);
                        break;
                    case "Scoped":
                        serviceCollection.AddScoped(serviceInterfaceType, serviceType);
                        break;
                    case "Transient":
                        serviceCollection.AddTransient(serviceInterfaceType, serviceType);
                        break;
                    default:
                        serviceCollection.AddScoped(serviceInterfaceType, serviceType);
                        break;
                }
            }

        return serviceCollection;
    }
}