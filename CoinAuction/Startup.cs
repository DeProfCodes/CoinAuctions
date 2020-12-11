using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CoinAuction.Data;
using CoinAuction.Tasks;
using CPMAPortal.Client.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace CoinAuction
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

           //services.AddDbContext<CoinAuctionContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CoinAuctionContext")));

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton(provider => GetScheduler());

            services.AddScoped<ISweetAlert, SweetAlert>();
           //AuctionsTask auction = new AuctionsTask(GetScheduler()); 
            //auction.RunAuctionJobs();
           // auction.RunContinuosJobs();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {       
            //new commit
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //var context = serviceScope.ServiceProvider.GetRequiredService<CoinAuctionContext>();
                //var context = new CoinAuctionContext();
                //context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
                //app.UseStatusCodePages();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePages();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IScheduler GetScheduler()
        {
            var properties = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "QuartzWithCore",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "3",
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",
            };
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.Start();
            return scheduler;
        }
    }
}
