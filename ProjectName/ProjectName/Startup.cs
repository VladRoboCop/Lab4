using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyNamespace
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "library",
                    pattern: "Library",
                    defaults: new { controller = "Library", action = "Welcome" });
                endpoints.MapControllerRoute(
                    name: "books",
                    pattern: "Library/Books",
                    defaults: new { controller = "Library", action = "Books" });
                endpoints.MapControllerRoute(
                    name: "profile",
                    pattern: "Library/Profile/{id?}",
                    defaults: new { controller = "Library", action = "Profile" });
            });
        }
    }
}
