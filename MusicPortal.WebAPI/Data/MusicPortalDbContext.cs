using Microsoft.AspNet.Identity.EntityFramework;
using MusicPortal.WebAPI.Domain_Models;
using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Data
{
    public class MusicPortalDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Song> Songs { get; set; }
         public MusicPortalDbContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
 
            }

         public static MusicPortalDbContext Create()
            {
                return new MusicPortalDbContext();
            }
    }
}