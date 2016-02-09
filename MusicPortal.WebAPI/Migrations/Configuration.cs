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
            //AutomaticMigrationDataLossAllowed = true;
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
            seed_genres(context); // comment this out after first seed
            seed_ekv(context);
            seed_rundek(context);
            seed_u2(context);
            seed_dubioza(context);
            //Makedo: https://api.soundcloud.com/tracks/97731606/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea
        }

        private void seed_genres(MusicPortalDbContext context)
        {

            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, alternative);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, blues);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, classic);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, country);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, electronic);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, rap);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, indie);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, pop);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, jazz);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, reggae);
            context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, rock);

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
                context.Tags.AddOrUpdate(t => new { t.Name, t.ParentId }, tag);
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
                t => new { t.Name, t.ParentId }, subgenre1
            );
            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, subgenre2
            );
            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, subgenre3
            );
            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, subgenre4
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
                    Name = "Geto", Link = @"https://api.soundcloud.com/tracks/31573988/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
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
                context.AuthorSongs.AddOrUpdate(a => new {a.SongId, a.AuthorId}, new AuthorSong { Author = ekv, Song = song });
                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = rock });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = subgenre1 });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = subgenre2 });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = subgenre3 });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = subgenre4 });

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
                t => new { t.Name, t.ParentId }, subgenre5
                ); 
                context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, subgenre6
                );

                context.TagSongs.AddOrUpdate(ts => new {ts.SongId, ts.TagId}, new TagSong { Song = song, Tag = subgenre5 });
                context.TagSongs.AddOrUpdate(ts => new {ts.SongId, ts.TagId}, new TagSong { Song = song, Tag = subgenre6 });

            }
        }

        private void seed_rundek(MusicPortalDbContext context)
        {
            //https://api.soundcloud.com/tracks?linked_partitioning=2&client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea&offset=10&q=rundek&limit=10
            var rundek = new Author { Name = "Darko Rundek" };
            context.Authors.AddOrUpdate(
                a => a.Name, rundek
            );

            Tag world = new Tag
            {
                Name = "World",
                ParentId = null,
                Popularity = 0
            };

            Tag funk = new Tag
            {
                Name = "Funk rock",
                ParentTag = rock,
                Popularity = 0
            };
            
            Tag authorTag1 = new Tag
            {
                Name = "Darko Rundek",
                ParentTag = funk,
                Popularity = 0
            };
            Tag authorTag2 = new Tag
            {
                Name = "Darko Rundek",
                ParentTag = world,
                Popularity = 0
            };

           context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, world
            );

            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, funk
            );
            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, authorTag1
            );
            context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, authorTag2
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
                context.AuthorSongs.AddOrUpdate(a => new {a.AuthorId, a.SongId}, new AuthorSong { Author = rundek, Song = song });

                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = rock });
                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = world });
                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = funk });
                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = authorTag1 });
                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = authorTag2 });

                Tag songNameTag1 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag1,
                    Popularity = 0
                };
                Tag songNameTag2 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag2,
                    Popularity = 0
                };
                context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, songNameTag1
                );
                context.Tags.AddOrUpdate(
                 t => new {t.ParentId, t.Name}, songNameTag2
                );

                context.TagSongs.AddOrUpdate(ts => new {ts.TagId, ts.SongId}, new TagSong { Song = song, Tag = songNameTag1 });
                context.TagSongs.AddOrUpdate(ts => new {ts.SongId, ts.TagId}, new TagSong { Song = song, Tag = songNameTag2 });

            }
        }

        void seed_u2(MusicPortalDbContext context)
        {
            //https://api.soundcloud.com/tracks?linked_partitioning=1&client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea&offset=0&q=u2&limit=30
            var u2 = new Author { Name = "U2" };
            context.Authors.AddOrUpdate(
                a => a.Name, u2
            );

            Tag postpunk = new Tag
            {
                Name = "Post punk",
                ParentTag = rock,
                Popularity = 0
            };

            Tag alt = new Tag
            {
                Name = "Alternative rock",
                ParentTag = alternative,
                Popularity = 0
            };

            Tag authorTag1 = new Tag
            {
                Name = "U2",
                ParentTag = alt,
                Popularity = 0
            };
            Tag authorTag2 = new Tag
            {
                Name = "U2",
                ParentTag = postpunk,
                Popularity = 0
            };

            var songs = new List<Song> {
                new Song
                {
                    Name = "Sunday Bloody Sunday", Link = @"https://api.soundcloud.com/tracks/39654171/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Beautiful Day", Link = @"https://api.soundcloud.com/tracks/88295642/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Ordinary Love", Link = @"https://api.soundcloud.com/tracks/135296438/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "One", Link = @"https://api.soundcloud.com/tracks/33825002/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Invisible", Link = @"https://api.soundcloud.com/tracks/33825002/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Stuck In A Moment You Can't Get Out Of", Link = @"https://api.soundcloud.com/tracks/57577256/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "With Or Without You", Link = @"https://api.soundcloud.com/tracks/50356349/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "City Of Blinding", Link = @"https://api.soundcloud.com/tracks/134374508/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Pride (In The Name Of Love)", Link = @"https://api.soundcloud.com/tracks/97731096/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                }
            };

            foreach (var song  in songs)
            {
                context.Songs.AddOrUpdate(s => s.Link, song);
                context.AuthorSongs.AddOrUpdate(a => new { a.AuthorId, a.SongId }, new AuthorSong { Author = u2, Song = song });

                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = rock });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = postpunk });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = alt });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = alternative });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = authorTag1 });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = authorTag2 });

                Tag songNameTag1 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag1,
                    Popularity = 0
                };
                Tag songNameTag2 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag2,
                    Popularity = 0
                };
                context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, songNameTag1
                );
                context.Tags.AddOrUpdate(
                 t => new { t.ParentId, t.Name }, songNameTag2
                );

                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = songNameTag1 });
                context.TagSongs.AddOrUpdate(ts => new { ts.SongId, ts.TagId }, new TagSong { Song = song, Tag = songNameTag2 });

            }
        }

        void seed_dubioza(MusicPortalDbContext context)
        {
            var dub = new Author { Name = "Dubioza Kolektiv" };
            context.Authors.AddOrUpdate(
                a => a.Name, dub
            );

            Tag raprock = new Tag
            {
                Name = "Rap Rock",
                ParentTag = rap,
                Popularity = 0
            };


            Tag dubTag = new Tag
            {
                Name = "Dub",
                ParentTag = reggae,
                Popularity = 0
            };

            Tag authorTag1 = new Tag
            {
                Name = "Dubioza Kolektiv",
                ParentTag = dubTag,
                Popularity = 0
            };
            Tag authorTag2 = new Tag
            {
                Name = "Dubioza Kolektiv",
                ParentTag = raprock,
                Popularity = 0
            };

            var songs = new List<Song> {
                new Song
                {
                    Name = "USA", Link = @"https://api.soundcloud.com/tracks/11848259/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Kažu", Link = @"https://api.soundcloud.com/tracks/71379514/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Balkan Funk", Link = @"https://api.soundcloud.com/tracks/71379518/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Walter", Link = @"https://api.soundcloud.com/tracks/71379510/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Recesija", Link = @"https://api.soundcloud.com/tracks/71379511/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Free MP3 (The Pirate Bay Song)", Link = @"https://api.soundcloud.com/tracks/223222238/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Euro Song", Link = @"https://api.soundcloud.com/tracks/11846016/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Vidi vidi vidi", Link = @"https://api.soundcloud.com/tracks/71379515/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Um klade valja", Link = @"https://api.soundcloud.com/tracks/71379509/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },

                new Song
                {
                    Name = "Volio BIH", Link = @"https://api.soundcloud.com/tracks/223967333/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "No Rscape (From Balkan)", Link = @"https://api.soundcloud.com/tracks/223222392/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Brijuni", Link = @"https://api.soundcloud.com/tracks/223967534/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },

                new Song
                {
                    Name = "Krivo je more", Link = @"https://api.soundcloud.com/tracks/223967049/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Tranzicija", Link = @"https://api.soundcloud.com/tracks/223967756/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Kupi", Link = @"https://api.soundcloud.com/tracks/223967175/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                },
                new Song
                {
                    Name = "Domacica", Link = @"https://api.soundcloud.com/tracks/225131942/stream?client_id=02gUJC0hH2ct1EGOcYXQIzRFU91c72Ea"
                }
    
            };

            foreach (var song in songs)
            {
                context.Songs.AddOrUpdate(s => s.Link, song);
                context.AuthorSongs.AddOrUpdate(a => new { a.AuthorId, a.SongId }, new AuthorSong { Author = dub, Song = song });

                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = rap });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = reggae });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = raprock });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = dubTag });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = authorTag1 });
                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = authorTag2 });

                Tag songNameTag1 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag1,
                    Popularity = 0
                };
                Tag songNameTag2 = new Tag
                {
                    Name = song.Name,
                    ParentTag = authorTag2,
                    Popularity = 0
                };
                context.Tags.AddOrUpdate(
                t => new { t.Name, t.ParentId }, songNameTag1
                );
                context.Tags.AddOrUpdate(
                 t => new { t.ParentId, t.Name }, songNameTag2
                );

                context.TagSongs.AddOrUpdate(ts => new { ts.TagId, ts.SongId }, new TagSong { Song = song, Tag = songNameTag1 });
                context.TagSongs.AddOrUpdate(ts => new { ts.SongId, ts.TagId }, new TagSong { Song = song, Tag = songNameTag2 });

            }
        }
    }
}
