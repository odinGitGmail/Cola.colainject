using Cola.ColaJwt;
using Cola.ColaMiddleware.ColaIpRateLimit;
using Cola.ColaMiddleware.ColaMiddle;
using Cola.ColaMiddleware.ColaSwagger;
using Cola.ColaMiddleware.ColaVersioning;
using Cola.ColaWebApi;
using Cola.Core.ColaEx;
using Cola.Core.ColaLog;
using Cola.Core.ColaSnowFlake;
using Cola.SystemBuilder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaInject;

public static class ColaInjectCore
{
    public static IServiceCollection AddColaCore(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 异常注入
        services.AddColaExceptionSingleton();
        
        // 日志注入
        services.AddColaLogs(configuration);
        
        // 基本模块 当前user注入
        services.AddColaModules();
        
        // 雪花ID注入
        services.AddColaSnowFlake(configuration);
        
        // WebApi 注入
        services.AddColaWebApi(configuration);
        
        // api 版本控制注入
        services.AddColaVersioning(configuration);
        
        // jwt 注入
        services.AddColaJwt(configuration);
        
        // swagger 注入
        services.AddColaSwagger(configuration);
        
        // 真实IP 注入
        services.AddColaIpRateLimit(configuration);
        
        // 跨域配置 注入
        services.AddColaCors(configuration);
        
        // 健康检查注入
        services.AddHealthChecks();
        
        return services;
    }
}