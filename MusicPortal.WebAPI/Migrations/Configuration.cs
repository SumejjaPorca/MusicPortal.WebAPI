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
        private static Tag alternative = new Tag
        {
            Name = "Alternative",
            ParentId = null,
            Popularity = 0
        };
        private static Tag blues = new Tag
        {
            Name = "Blues",
            ParentId = null,
            Popularity = 0
        };
        private static Tag classic = new Tag
        {
            Name = "Classic",
            ParentId = null,
            Popularity = 0
        };
        private static Tag country = new Tag
        {
            Name = "Country",
            ParentId = null,
            Popularity = 0
        };
        private static Tag electronic = new Tag
        {
            Name = "Electronic",
            ParentId = null,
            Popularity = 0
        };
        private static Tag rap = new Tag
        {
            Name = "Hip-Hop/Rap",
            ParentId = null,
            Popularity = 0
        };
        private static Tag indie = new Tag
        {
            Name = "Indie-Pop",
            ParentId = null,
            Popularity = 0
        };
        private static Tag pop = new Tag
        {
            Name = "Pop",
            ParentId = null,
            Popularity = 0
        };
        private static Tag jazz = new Tag
        {
            Name = "Jazz",
            ParentId = null,
            Popularity = 0
        };
        private static Tag reggae = new Tag
        {
            Name = "Reggae",
            ParentId = null,
            Popularity = 0
        };
        private static Tag rock = new Tag
        {
            Name = "Rock",
            ParentId = null,
            Popularity = 0
        };

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
            seed_genres(context);
            seed_ekv(context);
            seed_rundek(context);
            //TODO seed_rundek ^^
            //Makedo: https://api.soundcloud.com/tracks/97731606/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea
        }

        private void seed_genres(MusicPortalDbContext context)
        {
           
            context.Tags.AddOrUpdate(t => t.Name, alternative);
            context.Tags.AddOrUpdate(t => t.Name, blues);
            context.Tags.AddOrUpdate(t => t.Name, classic);
            context.Tags.AddOrUpdate(t => t.Name, country);
            context.Tags.AddOrUpdate(t => t.Name, electronic);
            context.Tags.AddOrUpdate(t => t.Name, rap);
            context.Tags.AddOrUpdate(t => t.Name, indie);
            context.Tags.AddOrUpdate(t => t.Name, pop);
            context.Tags.AddOrUpdate(t => t.Name, jazz);
            context.Tags.AddOrUpdate(t => t.Name, reggae);
            context.Tags.AddOrUpdate(t => t.Name, rock);

            List<Tag> tags = new List<Tag> { 
                new Tag{
                Name = "Art Punk",
                ParentTag = alternative,
                Popularity = 0 
            },
                new Tag {
                Name = "Indie Rock",
                ParentTag = alternative,
                Popularity = 0                 
            }, 
                new Tag {
                Name = "Punk",
                ParentTag = alternative,
                Popularity = 0                 
            },
                new Tag {
                Name = "Progressive Rock",
                ParentTag = alternative,
                Popularity = 0                 
            },
                new Tag {
                Name = "Chicago Blues",
                ParentTag = blues,
                Popularity = 0                 
            },   
                new Tag {
                Name = "Avant-Garde",
                ParentTag = classic,
                Popularity = 0                 
            },
               new Tag {
                Name = "Urban Cowboy",
                ParentTag = country,
                Popularity = 0                 
            },
                new Tag {
                Name = "Industrial",
                ParentTag = electronic,
                Popularity = 0                 
            },
                new Tag {
                Name = "Urban Cowboy",
                ParentTag = country,
                Popularity = 0                 
            },
                new Tag {
                Name = "Hip-hop",
                ParentTag = rap,
                Popularity = 0                 
            },
               new Tag {
                Name = "Acid Jazz",
                ParentTag = jazz,
                Popularity = 0                 
            },
               new Tag {
                Name = "Dance/Pop",
                ParentTag = pop,
                Popularity = 0                 
            },
               new Tag {
                Name = "Pop/Rock",
                ParentTag = pop,
                Popularity = 0                 
            },
               new Tag {
                Name = "Roots Reggae",
                ParentTag = reggae,
                Popularity = 0                 
            }, new Tag {
                Name = "Roots Rock",
                ParentTag = rock,
                Popularity = 0                 
            }, new Tag {
                Name = "Hard Rock",
                ParentTag = rock,
                Popularity = 0                 
            }, new Tag {
                Name = "Metal",
                ParentTag = rock,
                Popularity = 0                 
            }};

             foreach (var tag in tags)
            {
                context.Tags.AddOrUpdate(t => t.Name, tag);
            }

        }

        private void seed_ekv(MusicPortalDbContext context)
        {
            //S ove stranice uzeti naziv pjesme i stream_url: https://api.soundcloud.com/tracks?q=ekv&client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea
            var ekv = new Author { Name = "EKV - Ekatarina velika" };
            context.Authors.AddOrUpdate(
                a => a.Name, ekv
            );

            Tag subgenre1 = new Tag {
                Name = "Psychedelic",
                ParentTag = rock,
                Popularity = 0                 
            };
            Tag subgenre2 = new Tag {
                Name = "Art Rock",
                ParentTag = rock,
                Popularity = 0                 
            };
            Tag subgenre3 = new Tag
            {
                Name = "EKV - Ekatarina velika",
                ParentTag = subgenre1,
                Popularity = 0
            };
            Tag subgenre4 = new Tag
            {
                Name = "EKV - Ekatarina velika",
                ParentTag = subgenre2,
                Popularity = 0
            };
           
            context.Tags.AddOrUpdate(
                t => t.Name, subgenre1
            );
            context.Tags.AddOrUpdate(
                t => t.Name, subgenre2
            );
            context.Tags.AddOrUpdate(
                t => t.Name, subgenre3
            );
            context.Tags.AddOrUpdate(
                t => t.Name, subgenre4
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
                    Name = "Par godina za nas", Link = @"https://api.soundcloud.com/tracks/31657502/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Oci boje meda", Link = @"https://api.soundcloud.com/tracks/31657131/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                }, 
                new Song
                {
                    Name = "Kao da je bilo nekad", Link = @"https://api.soundcloud.com/tracks/31574537/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Pored mene", Link = @"https://api.soundcloud.com/tracks/31657721/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Sinhro", Link = @"https://api.soundcloud.com/tracks/31575619/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },

            };
            

            foreach (var song in songs)
            {
                context.Songs.AddOrUpdate(s => s.Link, song);
                context.AuthorSongs.AddOrUpdate(new AuthorSong { Author = ekv, Song = song });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = rock });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre1 });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre2 });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre3 });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre4 });

                Tag subgenre5 = new Tag
                {
                    Name = song.Name,
                    ParentTag = subgenre3,
                    Popularity = 0
                };
                Tag subgenre6 = new Tag
                {
                    Name = song.Name,
                    ParentTag = subgenre4,
                    Popularity = 0
                };
                context.Tags.AddOrUpdate(
                t => t.Name, subgenre5
                ); 
                context.Tags.AddOrUpdate(
                 t => t.Name, subgenre6
                );

                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre5 });
                context.TagSongs.AddOrUpdate(new TagSong { Song = song, Tag = subgenre6 });

            }
        }

        private void seed_rundek(MusicPortalDbContext context)
        {
            //https://api.soundcloud.com/tracks?linked_partitioning=2&client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea&offset=10&q=rundek&limit=10
            var rundek = new Author { Name = "Darko Rundek" };
            context.Authors.AddOrUpdate(
                a => a.Name, rundek
            );


            var songs = new List<Song> {
                new Song
                {
                    Name = "Apokalipso", Link = @"https://api.soundcloud.com/tracks/97728906/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Ruke", Link = @"https://api.soundcloud.com/tracks/97731809/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Makedo", Link = @"https://api.soundcloud.com/tracks/97731606/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Sanjam", Link = @"https://api.soundcloud.com/tracks/97732176/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Kurdistan", Link = @"https://api.soundcloud.com/tracks/87898110/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Ljubav se ne trzi", Link = @"https://api.soundcloud.com/tracks/97730424/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Sal od svile (Live)", Link = @"https://api.soundcloud.com/tracks/42324268/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Grane smo na vjetru", Link = @"https://api.soundcloud.com/tracks/14899253/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Kuba", Link = @"https://api.soundcloud.com/tracks/97731096/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Sanjala si da si sretna", Link = @"https://api.soundcloud.com/tracks/96996302/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },

            };


            foreach (var song in songs)
            {
                context.Songs.AddOrUpdate(s => s.Link, song);
                context.AuthorSongs.AddOrUpdate(new AuthorSong { Author = rundek, Song = song });
            }
        }
    }
}
