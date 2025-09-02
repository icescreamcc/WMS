using DbRepository.Repository; 
using External.Aliyun.OSS;
using External.Cache;
using External.MQ;
using External.Common;
using External.Log; 
using Logic.LogicCommon.FileStorage;
using Logic.LogicDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting; 
using Microsoft.OpenApi.Models;
using System;
using System.IO;  
using WebApi.Filter; 
using WebApi.Token;
using DbRepository.Repository.Seed;
using Quartz;
using Minio;
using External.MinIOService;
using Microsoft.AspNetCore.DataProtection;
using NPOI.POIFS.Crypt;
using Minio.DataModel.Args;
using System.Threading.Tasks;
using External.Socket.Socket;
using System.Net.WebSockets;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        //private readonly IWebHostEnvironment _webHostEnvironment;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();   // Swagger 依赖的服务

            #region Swagger配置
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "API Show", Version = "v1" });
                //s.DocumentFilter<HiddenApiFilter>();
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile); 
                s.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            #region 跨域配置 
            services.AddCors(options =>
                {
                    var allowCors = Configuration.GetSection("AllowCors").Value;
                    var urls = allowCors.Split(',');
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins(urls).SetIsOriginAllowedToAllowWildcardSubdomains()  
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("authorization");
                        });
                });
            #endregion

            #region 验证授权  
            services.AddSingleton<TokenHelper>();
            #endregion

            #region 缓存工具注册(Redis/MemoryCache)
            var selected = Configuration.GetSection("Cache:Selected").Value; 
            if (selected == "Redis")
            {  
                services.AddSingleton(new RedisClient(Configuration));
                services.AddSingleton<ICacheProvider, RedisProvider>();
            }
            else
            {
                var expiration = int.Parse(Configuration.GetSection("Cache:Expiration").Value);
                services.AddMemoryCache(options =>
                {
                    options.ExpirationScanFrequency = TimeSpan.FromSeconds(expiration); 
                });
                services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            }
            #endregion

            #region RabbitMQ
            services.AddSingleton(sp =>
            { 
                return new RabbitMQClient(Configuration);
            });
            #endregion

            #region NLog
            services.AddSingleton<LogHelper>();
            #endregion

            #region 数据库 
            services.AddTransient<DbContext>();
            services.AddTransient<Repository>();
            services.AddTransient<SeedHelper>();
            #endregion

            #region 文件存储(OSS/本地)
            services.AddScoped<FileHelper>();
            var storageMethod = Configuration.GetSection("FileStorage:Method").Value;
            if (storageMethod == "Oss")
            {
                services.AddScoped<OssConfigs>();
                services.AddScoped<OssHelper>(); 
                services.AddScoped<IFileStorage, FileOssStorage>();
            }
            else if (storageMethod == "MinIO")
            {
                var endpoint = "";
                var accessKey = "";
                var secretKey = "";
                var bucketName = "";
                var sign = Configuration.GetSection("Factory:sign").Value;
                if (sign == "dl")
                {
                     endpoint = Configuration.GetSection("MinIO:Endpoint").Value;
                     accessKey = Configuration.GetSection("MinIO:AccessKey").Value;
                     secretKey = Configuration.GetSection("MinIO:SecretKey").Value;
                     bucketName = Configuration.GetSection("MinIO:BucketName").Value;
                }
                else
                if (sign == "w3")
                {
                     endpoint = Configuration.GetSection("MinIOW3:Endpoint").Value;
                     accessKey = Configuration.GetSection("MinIOW3:AccessKey").Value;
                     secretKey = Configuration.GetSection("MinIOW3:SecretKey").Value;
                     bucketName = Configuration.GetSection("MinIOW3:BucketName").Value;
                }
               
                //services.AddMinio(config=>config.WithEndpoint(endpoint).WithCredentials(accessKey,secretKey));
                var  minioClient = new MinioClient().WithEndpoint(endpoint).WithCredentials(accessKey, secretKey).Build();
                Task.Run(() =>
                {
                    var beArgs = new BucketExistsArgs().WithBucket(bucketName);
                    bool found = minioClient.BucketExistsAsync(beArgs).Result;
                    if (!found)
                    {
                        var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                        minioClient.MakeBucketAsync(mbArgs);
                    }
                });
                services.AddSingleton(minioClient);
                services.AddScoped<MinIOHelper>(); 
                services.AddScoped<IFileStorage, FileMinIoStorage>();
            }
            else
            { 
                services.AddScoped<IFileStorage, FileLocalStorage>(); 
            }
            #endregion

            #region 业务注入 
            ProjectInjectionService.ConfigureServices(services);
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
                options.Filters.Add(typeof(ActionFilter));
            }).AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebSocketService webSocketService)
        {
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await webSocketService.HandleWebSocketAsync(context, webSocket);
                }
                else
                {
                    await next();
                }
            });

            //if (env.IsDevelopment())
            //{ 
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Show.v1");
                    c.RoutePrefix = "";
                });
            //}
             
            app.UseHttpsRedirection();
              
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = new PathString("/static")
            });
              
            app.UseRouting();

            app.UseCors(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); 
        }
    }
}
