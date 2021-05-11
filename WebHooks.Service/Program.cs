using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebHooks.BLL.Services;

namespace WebHooks.Service
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                });
    }
#pragma warning restore CS1591
}
