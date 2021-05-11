using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using WebHooks.BLL.Interfaces;
using WebHooks.BLL.Services;
using WebHooks.DAL.EF;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;
using WebHooks.DAL.Repositories;
using WebHooks.Service.Authentication;

namespace WebHooks.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddHttpContextAccessor();
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddTransient<IUowService>(s => new UowService(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<WebHooksContext>(
            options => { 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //options.LogTo(System.Console.WriteLine);
            });


            services.AddTransient<IRepository<EventHook>, GenericRepository<EventHook>>();
            services.AddTransient<IRepository<Subscription>, GenericRepository<Subscription>>();
            services.AddTransient<IRepository<Account>, GenericRepository<Account>>();

            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IEventHookService, EventHookService>();
            services.AddTransient<IAccountService, AccountService>();

            // Set the comments path for the Swagger JSON and UI.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebHookApi",
                    Contact = new OpenApiContact
                    {
                        Name = "Administrator",
                        Email = "nicklson1@yandex.ru"
                    },
                    Description = "<h1>Описание</h1>" +
                    "<h2>Это API создано для пользователей, которые хотят подключить Webhook'и для своих целей</h2><br>" +
                    "<h1>Аудентификация</h1>" +
                    "<h2>Для авторизации используется система Basic</h2>" +
                    "<h2>Для аутентификации с каждым запросом передается заголовок (header) Authorization<br> со значением Basic {accountName}:{password}</h2>" +
                    "<h2>{accountName}:{password} в кодировке base64</h2>"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvcCore();

            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseBasicAuthenticationMiddleware();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
