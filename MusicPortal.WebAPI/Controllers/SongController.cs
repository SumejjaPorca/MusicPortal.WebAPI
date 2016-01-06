

using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.BL;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MusicPortal.WebAPI.Controllers
{
    [RoutePrefix("api/Song")]
    public class SongController : ApiController
    {
        private MusicPortalDbContext _db;
        private SongManager _mngr;
        private HttpResponseHelper<SongVM> _helper;

        //heart_helper ... he-he :P
        private HttpResponseHelper<HeartedSongVM> _heartpler;
        public SongController()
        {
            _db = new MusicPortalDbContext();
            _mngr = new SongManager(_db);
            _helper = new HttpResponseHelper<SongVM>(this);
            _heartpler = new HttpResponseHelper<HeartedSongVM>(this);
        }

        // GET api/Song
        [Route("")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            HttpResponseMessage responseMsg;
            try
            {
                List<SongVM> songs = await _mngr.GetAllAsync();
                responseMsg = _helper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }
            return new AsyncResult(responseMsg);
        }

        // GET api/Song/{id}
        [Route("{id}")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetById(long id) {
            HttpResponseMessage responseMsg;
            try {
                SongVM song =  _mngr.GetById(id);
                responseMsg = _helper.CreateCustomResponseMsg(new List<SongVM>() { song }, HttpStatusCode.OK);

            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        // GET api/Song/search?query=Apokalipso
        [Route("search")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage SearchByQuery(string query) {
            HttpResponseMessage responseMsg;
            try {

                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                List<HeartedSongVM> songs = _mngr.GetFuzzy(query, userId);
                
                responseMsg = _heartpler.CreateCustomResponseMsg(songs, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _heartpler.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }

            return responseMsg;
        }

        // GET api/Song/search?tagName=rock
        [Route("search")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage SearchByTagName(string tagName) {
            HttpResponseMessage responseMsg;
            try {
                List<SongVM> songs = _mngr.GetByTagName(tagName);

                responseMsg = _helper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }

            return responseMsg;
        }

        // GET api/Song/{id}/play
        [Route("{id}/play")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage PlaySong(int id)
        {
            HttpResponseMessage responseMsg;
            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                if (userId != null)
                    _mngr.PlaySong(id, userId);
                responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }

            return responseMsg;
        }

        // POST api/Song
        [Route("")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage CreateSong(NewSongVM song) {
            HttpResponseMessage responseMsg;
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            try {
                SongVM createdSong = _mngr.CreateSong(song);

                responseMsg = _helper.CreateCustomResponseMsg(new List<SongVM>() { createdSong }, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        // POST api/Song/{id}/heart
        [Route("{id}/heart")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage HeartSong(long id) {
            HttpResponseMessage responseMsg;
            

            if (_db.Songs.FirstOrDefault(s => s.Id == id) == null) //song doesn't exist
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            try {
                string user_id = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                
                _mngr.HeartSong(id, user_id);
                responseMsg = new HttpResponseMessage(HttpStatusCode.OK);

            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        // POST api/Song/{id}/unheart
        [Route("{id}/unheart")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage UnheartSong(long id)
        {
            HttpResponseMessage responseMsg;


            if (_db.Songs.FirstOrDefault(s => s.Id == id) == null) //song doesn't exist
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            try
            {
                string user_id = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

                _mngr.HeartSong(id, user_id);
                responseMsg = new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        //TODO: manage this method, either delete it or add creatorId in Song entity
        // DELETE api/Song
        [Route("{songId}")]
        [Authorize]
        [HttpDelete]
        public HttpResponseMessage DeleteSong(long songId) {
            HttpResponseMessage responseMsg;
            try {
                SongVM deletedSong = _mngr.DeleteSong(songId);

                responseMsg = _helper.CreateCustomResponseMsg(new List<SongVM>() { deletedSong }, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
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
