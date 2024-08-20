using System.Data.Common;
using Blog.BaseConfigSerivce.DynamicAPi;
using Core.AutoInjectService;
using Core.Consul;
using Core.Filter;
using Core.Middleware;
using Core.SqlSugar;
using Core.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using OrderService.Models;

namespace OrderService;

public static class Init
{
    public static void InitializationApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //构建服务
        BuildServices(builder);
        //配置
        var app = builder.Build();
        Configure(app);
        app.Run();
    }

    private static void BuildServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //跨域
        builder.Services.AddCors(option =>
        {
            option.AddPolicy(name: "AllowCore", x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });
        });
        //添加Controller请求过滤器
        builder.Services.AddMvc(options => { options.Filters.Add<ApiFilter>(); });
        //添加httpClient后可以在项目任何地址构造函数中注入使用。
        builder.Services.AddHttpClient();
        //必须先构建Controller，否则后面Swagger无法构造，同时构建动态WebApi
        builder.Services.AddControllers().AddDynamicWebApi();
        // 添加Swagger
        builder.Services.AddSwaggerGenExtend();
        //自动注入WebApi服务接口
        builder.Services.AutoRegistryService(new List<string> { "Order.Service" });
        //添加SqlSugar服务
        builder.Services.AddSqlsugarSetup(builder.Configuration);
        DbConnection connection = new MySqlConnection(builder.Configuration.GetConnectionString("OrderContext"));
        //添加CAP，EventBus消息总线实现事件发布、订阅基于MQ实现
        builder.Services.AddDbContext<OrderContext>(opt => opt.UseMySql(connection,MySqlServerVersion.LatestSupportedServerVersion));
        builder.Services.AddCap(x =>
        {
            x.UseEntityFramework<OrderContext>();
            x.UseDashboard();
            x.UseRabbitMQ("host.docker.internal");

        });
    }

    private static void Configure(WebApplication app)
    {
        //配置全局异常处理
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); //配置MVC控制器路由  
        });
        //允许跨域
        app.UseCors("AllowCore");
        //使用Swagger  
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroServicesWeb.Core V1");
            c.RoutePrefix = "ApiDoc";
        });
        //服务注册
         // app.RegisterConsul(app.Configuration, app.Lifetime);
    }
}