using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Domain_Models;
using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Providers;
using MusicPortal.WebAPI.BL;

namespace MusicPortal.WebAPI.Controllers
{
    [RoutePrefix("api/playlists")]
    [Authorize]
    public class PlaylistsController : ApiController
    {
        private MusicPortalDbContext _db;
        private PlaylistManager _mngr;
        public PlaylistsController()
        {
            _db = new MusicPortalDbContext();
            _mngr = new PlaylistManager(_db);
        }

        [Route("")]
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(List<PlaylistShortVM>))]
        public HttpResponseMessage GetPlaylistFromUser()
        {
            HttpResponseMessage responseMsg;
            HttpResponseHelper<PlaylistShortVM> _helper = new HttpResponseHelper<PlaylistShortVM>(this);
                
            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlists = _mngr.getPlaylists(userId);
                responseMsg = _helper.CreateCustomResponseMsg(playlists.ToList(), HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        //GET api/playlists/full
        [Route("full")]
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(List<PlaylistVM>))]
        public HttpResponseMessage GetPlaylistFullFromUser()
        {
            HttpResponseMessage responseMsg;
            HttpResponseHelper<PlaylistVM> _helper = new HttpResponseHelper<PlaylistVM>(this);

            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlists = _mngr.getPlaylistsFull(userId);
                responseMsg = _helper.CreateCustomResponseMsg(playlists.ToList(), HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        //GET api/playlists/13213
        [Route("{id}")]
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(PlaylistVM))]
        public IHttpActionResult GetPlaylistById(long id)
        {
            
            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlist = _mngr.GetPlaylistById(id, userId, true);

                return Ok(playlist);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
        //POST api/playlists/add
        [Route("add")]
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(PlaylistVM))]
        public IHttpActionResult NewPlaylist(NewPlaylistVM model)
        {
            if (model.Title.Equals(String.Empty))
                return BadRequest("Playlist title is required");
            
            try
            {

                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlist = _mngr.makePlaylist(model.Title, userId);
                PlaylistVM playlistVM = new PlaylistVM()
                {
                    Id = playlist.Id,
                    Title = playlist.Title,
                    Songs = new List<HeartedSongVM>()
                };

                return Ok(playlistVM);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        //POST api/playlists/song
        [Route("song")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult AddSongToPlaylist([FromBody] PlaylistAddSongVM model)
        {

            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlist = _db.Playlists.Where(p => p.Id == model.PlaylistId).FirstOrDefault();
                if (playlist != null && !playlist.OwnerId.Equals(userId))
                    return BadRequest("You are not owner of this playlist.");

                _mngr.addSong(model.PlaylistId, model.SongId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        //DELETE api/playlists/song
        [Route("song")]
        [Authorize]
        [HttpDelete]
        public IHttpActionResult RemoveSongFromPlaylist([FromBody] PlaylistAddSongVM model)
        {

            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                var playlist = _db.Playlists.Where(p => p.Id == model.PlaylistId).FirstOrDefault();
                if (playlist != null && !playlist.OwnerId.Equals(userId))
                    return BadRequest("You are not owner of this playlist.");

                _mngr.removeSong(model.PlaylistId, model.SongId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}