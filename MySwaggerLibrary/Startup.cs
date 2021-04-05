using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySwaggerLibrary.Models;
using System;
using System.IO;
using System.Reflection;

namespace MySwaggerLibrary
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
            services.AddDbContext<MySwaggerDbContext>(opts =>
           {
               opts.UseSqlServer(Configuration["ConnectionString"]);
           });

            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("productV1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Product API",
                    Description = "CRUD API",
                    Contact = new OpenApiContact
                    {
                        Name = "t4h4",
                        Email = "t4h4@t4h4.net"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // dll tarafindan uretilen xml dosyasinin ismi alindi.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // xml'in pathini aliyoruz. combine method'u bunlari birlestirmekle gorevli. ortaya path cikiyor.
                gen.IncludeXmlComments(xmlPath);
            });

            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/productV1/swagger.json", "Product API");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}