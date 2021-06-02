using Microsoft.AspNetCore.Identity;
using AlbumForU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumForU.ViewModels.AdminViewModels
{
    public class UserRoleRelation
    {
        public AppUser user { get; set; }
        public List<IdentityRole>roles { get; set; }
        public string chosenRoleId { get; set; }
    }
}
