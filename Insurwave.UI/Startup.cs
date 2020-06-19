using Insurwave.Domain;
using Insurwave.UI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Net.Http;

namespace Insurwave.UI
{
    public class Startup
    {
        private string _insurwaveWeatherApiKey = null;
        private string _insurwaveWeatherApiDomain = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<WeatherApiClient>("WeatherApiClient", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            //.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            //{
            //    TimeSpan.FromSeconds(5),
            //    TimeSpan.FromSeconds(10)
            //})); // Polly want to act up - need to investigate alternative in core 3.1
            //https://stackoverflow.com/questions/58953178/how-to-register-polly-on-a-ihttpclient-already-registered

            _insurwaveWeatherApiKey = Configuration["Insurwave:WeatherApiKey"];
            _insurwaveWeatherApiDomain = Configuration["Insurwave:WeatherApiDomain"];

            services.AddScoped<IWeatherService>(s => {
                return new WeatherService(new LoggerFactory().CreateLogger<WeatherService>(), 
                                          s.GetService<WeatherApiClient>().Client, 
                                          _insurwaveWeatherApiKey, 
                                          _insurwaveWeatherApiDomain);
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Weather Api", 
                    Version = "v1",
                    Description = "Simple Meteorological Api",
                    Contact = new OpenApiContact
                    {
                        Email = "eryn.macdonald@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/erynmacdonald/"),
                    }
                });
                c.OperationFilter<AssignContentTypeFilter>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Insurwave.UI.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Insurwave.Model.xml"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Api v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // TODO: We can discuss various approaches to authentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
