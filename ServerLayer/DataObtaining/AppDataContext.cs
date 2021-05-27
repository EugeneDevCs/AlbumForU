using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServerLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.DataObtaining
{
    public class AppDataContext:IdentityDbContext<AppUser>
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
        }
    }
}
