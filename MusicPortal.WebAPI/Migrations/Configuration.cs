namespace MusicPortal.WebAPI.Migrations
{
    using Domain_Models;
    using Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<MusicPortal.WebAPI.Data.MusicPortalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MusicPortalDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            seed_ekv(context);
            //TODO seed_rundek ^^
            //Makedo: https://api.soundcloud.com/tracks/97731606/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea
        }

        private void seed_ekv(MusicPortalDbContext context)
        {
            //S ove stranice uzeti naziv pjesme i stream_url: https://api.soundcloud.com/tracks?q=ekv&client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea
            var ekv = new Author { Name = "EKV - Ekatarina velika" };
            context.Authors.AddOrUpdate(
                a => a.Name, ekv
            );

           
            var songs = new List<Song> {
                new Song
                {
                    Name = "Ti si sav moj bol", Link = @"https://api.soundcloud.com/tracks/31657299/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "7 dana", Link = @"https://api.soundcloud.com/tracks/31573554/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Anestezija", Link = @"https://api.soundcloud.com/tracks/31573664/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Geto", Link = @"https://api.soundcloud.com/tracks/31573988?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Glad", Link = @"https://api.soundcloud.com/tracks/31574084/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Par godina za nas", Link = @"https://api.soundcloud.com/tracks/31657502/stream"
                },
                new Song
                {
                    Name = "Oci boje meda", Link = @"https://api.soundcloud.com/tracks/31657131/stream"
                }, 
                new Song
                {
                    Name = "Kao da je bilo nekad", Link = @"https://api.soundcloud.com/tracks/31574537/stream"
                },
                new Song
                {
                    Name = "Pored mene", Link = @"https://api.soundcloud.com/tracks/31657721/stream"
                },
                new Song
                {
                    Name = "Sinhro", Link = @"https://api.soundcloud.com/tracks/31575619/stream"
                },

            };
            

            foreach (var song in songs)
            {
                context.Songs.AddOrUpdate(s => s.Link, song);
                context.AuthorSongs.AddOrUpdate(new AuthorSong { Author = ekv, Song = song });
            }
        }
    }
}
