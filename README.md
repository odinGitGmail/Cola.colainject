### [Cola.ColaInject](https://github.com/odinsam/OdinCola/tree/master/Cola.Solution/Cola.ColaInject) 

[![Version](https://flat.badgen.net/nuget/v/Cola.ColaInject?label=version)](https://github.com/odinGitGmail/Cola.ColaInject) [![download](https://flat.badgen.net/nuget/dt/Cola.ColaInject)](https://www.nuget.org/packages/Cola.ColaInject) [![commit](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaInject)](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaInject) [![Blog](https://flat.badgen.net/static/blog/odinsam.com)](https://odinsam.com)

#### 配置文件样例
```json
{
  "ColaLogs":{
    "LogSeparator":"*",
    "LogTimeFormat":"yyyy-MM-dd HH:mm:ss",
    /*
    保存形式为 File、SqlServer、MySql、All、不配置， 效果分别为
    保存到 文件、sqlserver数据库、mysql数据库、所有（即保存文件又保存数据库）、仅打印
    配置方式如下
    "SaveType":"File,MySql" 或者 "SaveType":"All"  或者 "SaveType":""
    */
    "SaveType":"File",
    // 保存log的数据库链接字符串
    "DbConnectionString": "",
    // 日志文件夹名称
    "DirectoryName": "ColaLogs",
    // 日志保留时间 单位 天
    "KeepTime":10
  },

  "ColaSnowFlake": {
    "DatacenterId": 1,
    "WorkerId": 1
  },

  "ColaWebApi": [
    {
      "ClientName": "Cola",
      "BaseUri": "https://tenapi.cn",
      "TimeOut": 5000,
      "Cert": {
        "CertFilePath": "",
        "CertFilePwd": ""
      },
      /* 默认压缩方式  None,GZip,Deflate,Brotli,All */
      "Decompression": "All"
    },
    {
      "ClientName": "ColaNacos",
      "BaseUri": "http://192.168.202.132:8848",
      "TimeOut": 5000,
      "Cert": {
        "CertFilePath": "",
        "CertFilePwd": ""
      },
      /* 默认压缩方式  None,GZip,Deflate,Brotli,All */
      "Decompression": "All"
    }
  ],

  "ColaVersioning":{
    "ReportApiVersions": true,
    "AssumeDefaultVersionWhenUnspecified": true,
    "MajorVersion": 1,
    "MinorVersion": 0,
    "HttpHeaderName": "x-api-version"
  },

  "ColaJwt": {
    "SecretKey": "fFPdfMfCeysgZrHPeWWPTGrRphbvrunkGuktVkEmacazAlzFphWcGEaoHXBycxmDrWDtqomxmLfFabYTZQKocbRqNFzuSzIURBIsxruzqvzRRYhuMaxmNfviApzDGOZy@uK&&OEb",
    "IssUser": "odinsam.com",
    "Audience": "odinsam",
    "AccessExpiration": 30,
    "RefreshExpiration": 30
  },

  "ColaSwagger": {
    // 启用xml注释
    "EnableXmlComment": true,
    // 启用jwt
    "EnableJWT": true,
    "Version": "Ver:1.0.0",
    "Title": "Cola Swagger",
    "Description": "Cola Swagger Description",
    "OpenApiContact": {
      "Name": "OdinSam",
      "Url": "https://odinsam.com",
      "Email": "odinsam.cn@gmail.com"
    },
    "OpenApiLicense": {
      "Name": "License",
      "Url": "https://odinsam.com"
    }
  },

  // IP速率限制
  "ColaIpRateLimit": {
    // 这里暂时只支持 Memory 和 Redis 两种方式(默认使用 Memory).  如果使用 redis缓存，则需要再注入IpRateLimit之前优先注入redis
    "IpRateLimitCache": "Memory",
    // false则全局将应用限制，并且仅应用具有作为端点的规则* 。 true则限制将应用于每个端点，如{HTTP_Verb}{PATH}
    "EnableEndpointRateLimiting": true,
    // false则拒绝的API调用不会添加到调用次数计数器上
    "StackBlockedRequests": false,
　　 // 注意这个配置，表示获取用户端的真实IP，我们的线上经过负载后是 X-Forwarded-For，而测试服务器没有，所以是X-Real-IP
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 200,
    // 自定义返回的内容，所以必须设置HttpStatusCode和StatusCode为200
    "QuotaExceededResponse": {
      "Content": "{{\"code\":429,\"msg\":\"访问过于频繁，请稍后重试\",\"data\":null}}",
      "ContentType": "application/json",
      "StatusCode": 200
    },
    // IP白名单，本地调试或者UAT环境，可以加入相应的IP，略过策略的限制
    "IpWhitelist": [ ],
    // 端点白名单，如果全局配置了访问策略，设置端点白名单相当于IP白名单一样，略过策略的限制
    "EndpointWhitelist": [],
    "ClientWhitelist": [],
    // 具体的策略，根据不同需求配置不同端点即可， Period的单位可以是s, m, h, d，Limint是单位时间内的允许访问的次数
    "GeneralRules": [
    ]
  },

  "ColaCors": {
    "Cors": [
      {
        "CorsName": "LimitRequests",
        // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的。http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，需要写两个
        "AllowOriginsIp": "http://127.0.0.1:2364,http://localhost:2364,http://localhost:8080,http://localhost:8021,http://localhost:1818",
        "AllowHeaders": ""
      }
    ]
  }
}
```

#### 具体调用
```csharp
// 缓存 组件
builder.Services.AddColaInjectCore(config);


// 中间件注入

app.UseIpRateLimiting();
        
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseColaSwagger(urlAndName);


app.UseRouting();
app.UseCors();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseColaHealthChecks("/healthCheck");
```
