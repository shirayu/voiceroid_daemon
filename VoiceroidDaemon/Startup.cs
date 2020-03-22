using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VoiceroidDaemon
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
            // HTMLエンティティを防止する
            services.Configure<Microsoft.Extensions.WebEncoders.WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All);
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var addresses = Setting.System.CorsAddresses;
            if (addresses != null)
            {
                foreach (var adr in Setting.System.CorsAddresses.Split(','))
                {
                    var myadr = adr.Trim();
                    if (myadr == "*")
                    {
                        app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                    }
                    else if (myadr.Length > 0)
                    {
                        app.UseCors(builder => builder
                        .WithOrigins(myadr)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                    }
                };
            };


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });*/
        }
    }
}
