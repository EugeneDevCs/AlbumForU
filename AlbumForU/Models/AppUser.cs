using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlbumForU.Models
{
    public class AppUser:IdentityUser
    {
        [PersonalData]
        [Column(TypeName ="nvarchar(100)")]
        public string Firstname { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Lastname { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(60)")]
        public string Nickname { get; set; }
    }
}
