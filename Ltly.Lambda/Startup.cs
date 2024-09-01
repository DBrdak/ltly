using Ltly.Lambda.Data;
using Ltly.Lambda.Data.Repositories;
using Ltly.Lambda.Domain.Urls;
using Microsoft.Extensions.DependencyInjection;

namespace Ltly.Lambda;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<LoggingService>();
        services.AddSingleton<RequestContextAccessor>();
        services.AddScoped<DbContext>();
        services.AddScoped<IUrlRepository, UrlRepository>();
    }
}
