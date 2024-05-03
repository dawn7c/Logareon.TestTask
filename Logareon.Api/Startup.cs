using Microsoft.OpenApi.Models;
using Logareon.Domain.Repository;
using Logareon.Domain.Models;
using Logareon.Application.Services;
using Logareon.Application.Abstractions;

namespace Logareon.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
             
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Logareon_Service", Version = "1.0" });
            });

            services.AddTransient(typeof(IReportBuilder), typeof(ReportBuilder));
            services.AddTransient(typeof(IReporter), typeof(Reporter));
            services.AddSingleton<IRequestIdentifierService, RequestIdentifierService>();
            services.AddSingleton<IReportBuildingService,ReportBuildingService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
