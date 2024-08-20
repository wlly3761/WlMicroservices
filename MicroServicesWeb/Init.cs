using Core.Middleware;
using MicroServicesWeb.ServiceHelper;
using NLog.Web;


namespace MicroServicesWeb;

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
        builder.Services.AddMvc();
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IServiceHelper, ServiceHelper.ServiceHelper>();
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
        //注入http请求上下文
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private static void Configure(WebApplication app)
    {
        //配置全局异常处理
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(  
                name: "default",  
                pattern: "{controller=Home}/{action=Index}/{id?}");   
        });
        //允许跨域
        app.UseCors("AllowCore");
        //程序启动时 获取服务列表
        // new ServiceHelper.ServiceHelper(app.Configuration).GetServices();
    }
}