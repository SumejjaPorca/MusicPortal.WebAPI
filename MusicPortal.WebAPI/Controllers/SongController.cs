

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
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }
            return new AsyncResult(responseMsg);
        }

		// GET api/Song/searchQuery
        /*[Route("{searchQuery}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string searchQuery)
        {
            HttpResponseMessage responseMsg;
            try
            {
                List<Domain_Models.Song> songs = _mngr.GetFuzzy(searchQuery);
                responseMsg = _helper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }
            return new AsyncResult(responseMsg);
        }*/
    }
}
