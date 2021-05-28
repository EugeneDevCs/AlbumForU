using Microsoft.AspNetCore.Identity;
using ServerLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLayer.DataObtaining
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            string managerEmail = "manager@gmail.com";
            string managerPassword = "_ManagerPassword";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
                AppUser manager = new AppUser { Email = managerEmail, UserName = managerEmail };
                IdentityResult adminResult = await userManager.CreateAsync(admin, password);
                IdentityResult manageResult = await userManager.CreateAsync(manager, managerPassword);

                if (adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
                if(manageResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "manager");
                }
            }
        }
    }
}
