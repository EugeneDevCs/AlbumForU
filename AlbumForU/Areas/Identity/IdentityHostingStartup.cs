using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerLayer.DataObtaining;
using ServerLayer.Models;

[assembly: HostingStartup(typeof(AlbumForU.Areas.Identity.IdentityHostingStartup))]
namespace AlbumForU.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<AppDataContext>(options =>
            //        options.UseSqlServer(
            //            context.Configuration.GetConnectionString("DefaultConnection")));
                
            //    services.AddDefaultIdentity<AppUser>(options =>
            //    {
            //        options.SignIn.RequireConfirmedEmail = false;
            //        options.Password.RequireUppercase = false;
            //        options.Password.RequireLowercase = false;
            //        options.Password.RequireNonAlphanumeric = false;
            //    }).AddEntityFrameworkStores<AppDataContext>();
            //});
            
        }
    }
}