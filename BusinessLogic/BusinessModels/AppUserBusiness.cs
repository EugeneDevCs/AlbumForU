using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.BusinessModels
{
    public class AppUserBusiness
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
    }
}
