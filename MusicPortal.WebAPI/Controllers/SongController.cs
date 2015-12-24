

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
        public SongController()
        {
            _db = new MusicPortalDbContext();
            _mngr = new SongManager(_db);
            _helper = new HttpResponseHelper<SongVM>(this);
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

        // GET api/Song
        [Route("")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage SearchByQuery(string searchQuery) {
            HttpResponseMessage responseMsg;
            try {
                List<SongVM> songs = _mngr.GetFuzzy(searchQuery);

                responseMsg = _helper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }

            return responseMsg;
        }

        // GET api/Song
        [Route("")]
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

        // POST api/Song
        [Route("")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage CreateSong(NewSongVM song) {
            HttpResponseMessage responseMsg;
            try {
                SongVM createdSong = _mngr.CreateSong(song);

                responseMsg = _helper.CreateCustomResponseMsg(new List<SongVM>() { createdSong }, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        // PUT api/Song
        [Route("")]
        [Authorize]
        [HttpPut]
        public HttpResponseMessage UpdateSong(SongVM song) {
            HttpResponseMessage responseMsg;
            try {
                SongVM updatedSong = _mngr.UpdateSong(song);

                responseMsg = _helper.CreateCustomResponseMsg(new List<SongVM>() { updatedSong }, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

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

    }
}
