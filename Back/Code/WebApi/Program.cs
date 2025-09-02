using Logic.ScheduleJob;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System; 
using System.Net; 

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var jobLauncher = provider.GetService<JobLauncher>();
                if (jobLauncher != null)
                {
                    jobLauncher.JobStart();
                }
                else
                {
                    var logger = provider.GetService<ILogger<Program>>();
                    logger?.LogWarning("JobLauncher Î´×¢²á£¬Ìø¹ý JobStart()");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.UseStartup<Startup>(); 
            }).UseNLog();
        }
 
    }
}
