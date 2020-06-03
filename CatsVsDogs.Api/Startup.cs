using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatsVsDogs.Core.Dto;
using CatsVsDogs.Core.Helpers;
using CatsVsDogs.Core.Interfaces;
using CatsVsDogs.Core.Interfaces.Repositories;
using CatsVsDogs.Core.Interfaces.Services;
using CatsVsDogs.Core.Services;
using CatsVsDogs.Infrastructure.Data;
using CatsVsDogs.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;

namespace CatsVsDogs.Api
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

            var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));


            services.AddCors(options =>
                {
                    options.AddPolicy(name: "Client",
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                        });
                });

            services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile(modelName: "ImageModel",
                    filePath:
                    "model.zip",
                    watchForChanges: true);

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable(Constants.DbConnection)));

            services.AddScoped<IPredictionRepository, PredictionRepository>();
            services.AddScoped<IPredictionService, PredictionService>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseRouting();

            app.UseCors("Client");

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}