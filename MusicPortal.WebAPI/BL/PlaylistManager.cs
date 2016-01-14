using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MusicPortal.WebAPI.BL
{
    public class PlaylistManager
    {

        private MusicPortalDbContext _db;

        public PlaylistManager(MusicPortalDbContext db) {
            _db = db;
        }

        public IQueryable<PlaylistShortVM> getPlaylists(string user_id)
        {
            return _db.Playlists.Where(p => p.OwnerId.Equals(user_id)).Select(p =>
                                new PlaylistShortVM
                                {
                                    Id = p.Id,
                                    Title = p.Title
                                });
        }

        public IQueryable<PlaylistVM> getPlaylistsFull(string user_id)
        {
            SongManager sm = new SongManager(_db);

            var playlists = from pl in _db.Playlists.Where(p => p.OwnerId.Equals(user_id))
                            join pls in _db.PlaylistSongs.Include(pp => pp.Song) on pl.Id equals pls.PlaylistId into pls
                            from plsg in pls.DefaultIfEmpty()
                            group plsg by pl into plays
                            select new PlaylistVM
                            {
                                Id = plays.Key.Id,
                                Title = plays.Key.Title,
                                Songs = (from song in plays.Select(p => p.Song)
                                        join sa in _db.AuthorSongs on song.Id equals sa.SongId
                                        join authors in _db.Authors on sa.AuthorId equals authors.Id into auths
                                        join hs in _db.HeartedSongs on song.Id equals hs.SongId into hs
                                        from hearted in hs.DefaultIfEmpty()
                                        where hearted.UserId.Equals(user_id) || hearted == null
                                        select new HeartedSongVM
                                        {
                                            SongId = song.Id,
                                            Name = song.Name,
                                            Link = song.Link,
                                            IsHearted = hearted == null ? false : hearted.IsHearted,
                                            Authors = auths.Select(a => new AuthorVM { Id = a.Id, Name = a.Name })
                                        }) //sm.MakeHeartedSong(plays.Select(p => p.Song).AsQueryable(), user_id)
                            };
            return playlists;
        }
        
        public PlaylistVM GetPlaylistById(long playlistId, string user_id, bool check_if_owner = false)
        {
            var playlist = _db.Playlists.Where(pl => pl.Id == playlistId).FirstOrDefault();
            if (playlist == null)
                throw new Exception("Playlist with this ID doesn't exist");

            if (user_id != null && check_if_owner && !playlist.OwnerId.Equals(user_id))
                throw new Exception("This user isn't owner of this playlist");

            SongManager sm = new SongManager(_db);

            var songs_query = _db.PlaylistSongs.Where(pls => pls.PlaylistId == playlistId).Join(_db.Songs, p => p.SongId, s => s.Id, (p, s) => s);
            var songs = sm.MakeHeartedSong(songs_query, user_id);

            return new PlaylistVM
            {
                Id = playlist.Id,
                Title = playlist.Title,
                Songs = songs.ToList()
            };
        }

        public void addSong(long playlistId, long songId)
        {
            var playlist = _db.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();
            if (playlist == null)
                throw new Exception("There is no playlist with this ID");

            if (_db.PlaylistSongs.Where(ps => ps.PlaylistId == playlistId && ps.SongId == songId).FirstOrDefault() != null)
                throw new Exception("This song is already in playlist");

            _db.PlaylistSongs.Add(new Domain_Models.PlaylistSong { PlaylistId = playlistId, SongId = songId });
            _db.SaveChanges();
        }

        public void removeSong(long playlistId, long songId)
        {
            var playlist = _db.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();
            if (playlist == null)
                throw new Exception("There is no playlist with this ID.");

            var ps = _db.PlaylistSongs.Where(p => p.PlaylistId == playlistId && p.SongId == songId).FirstOrDefault();
            if (ps == null)
                throw new Exception("This song isn't in playlist already.");

            _db.PlaylistSongs.Remove(ps);
            _db.SaveChanges();
        }

        public Domain_Models.Playlist makePlaylist(string title, string user_id){
            Domain_Models.Playlist newP = new Domain_Models.Playlist() { Title = title, OwnerId = user_id };
            _db.Playlists.Add(newP);
            _db.SaveChanges();

            return newP;
        }

        public void deletePlaylist(long playlistId)
        {
            var playlist = _db.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();

            if (playlist == null)
            {
                throw new Exception("There is no playlist with this ID.");
            }

            _db.Playlists.Remove(playlist);
            _db.SaveChanges();
        }
    }
}