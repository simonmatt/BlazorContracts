using BlazorContracts.API.Data;
using BlazorContracts.Shared.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;

namespace BlazorContracts.API
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
            services.AddOData();
            //services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                 {
                     options.Authority = "http://localhost:5100";
                     options.RequireHttpsMetadata = false;

                     options.ApiName = "blazorcontracts-api";
                 });

            services.AddAuthorization();

            services.AddDbContext<ContractsDbContext>(options =>
            {
                options.UseInMemoryDatabase("Contracts");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Select().Filter().OrderBy().Expand().Count().MaxTop(50);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
            });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Contract>("Contracts");
            return builder.GetEdmModel();
        }
    }
}