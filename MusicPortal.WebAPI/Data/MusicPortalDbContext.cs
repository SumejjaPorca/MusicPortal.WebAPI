using Microsoft.AspNet.Identity.EntityFramework;
using MusicPortal.WebAPI.Domain_Models;
using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Data {

    public class MusicPortalDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<HeartedSong> HeartedSongs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagSong> TagSongs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<AuthorSong> AuthorSongs { get; set; }

        public MusicPortalDbContext() : base("DefaultConnection", throwIfV1Schema: false) {
            
        }

        public static MusicPortalDbContext Create() {
            return new MusicPortalDbContext();
        }

        public DbSet<TagUser> TagUsers { get; set; }
    }

}