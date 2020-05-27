using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using library1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;
using Microsoft.Extensions.Hosting;
using library1.Services;
using Microsoft.AspNetCore.Authentication.Google;
using library1.Services.User;

namespace library1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            // define Connection String
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefualtConnection")));
            // For create and use identity in database
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // External login
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    
                    //options.ClientId = googleAuthNSection[""];
                    //options.ClientSecret = googleAuthNSection[""];
                });

            // Add framwork service
            services.AddMvc();
            //services.AddAutoMapper();
            // pagning
            services.AddPaging();
            // Service
            // service of adamin
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookGroupService, BookGroupService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IManageTransactionService, ManageTransactionService>();
            services.AddTransient<IManageRequestBookService, ManageRequestBookService>();
            // service of user
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IUserProfileService, UserProfileService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Create Automatic Database
            using (var ServiceScop = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = ServiceScop.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    context.Database.Migrate();
                }
                catch
                {
                }
            }
            // ---

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                //app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(

                    name: "Admin",
                    //areaName: "Admin",
                    pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "User",
                    //areaName: "admin",
                    pattern: "{area:exists}/{controller=UserProfile}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            #region core 2.1
            //app.UseStaticFiles();
            //app.UseAuthentication();
            ////app.UseIdentity();
            //// This methode is absolate and will be removed in a future version. The recommended alternative is AuthAppBuilderExtensions.UseAuthentication(IApplicationBuilder)
            //// Enable ASP.NET identity for the current application.
            
            //app.UseMvc(routes =>
            //{
            //    // New area for Admin
            //    routes.MapRoute(
            //        name: "Admin",
            //        template: "{area:exists}/{controller=User}/{action=Index}/{id?}"
            //        );
            //
            //
            //    // New area for User
            //    routes.MapRoute(
            //        name: "UserProfile",
            //        template: "{area:exists}/{controller=UserProfile}/{action=Index}/{id?}"
            //        );
            //
            //
            //    // Traditional routing
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            #endregion

        }
    }
}
