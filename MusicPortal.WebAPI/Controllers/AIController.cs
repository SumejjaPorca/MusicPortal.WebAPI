using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.BL;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicPortal.WebAPI.Controllers
{
    [RoutePrefix("api/AI")]
    public class AIController : ApiController
    {
        private MusicPortalDbContext _db;
        private HttpResponseHelper<HeartedSongVM> _helper;
        private AIManager _mngr;
        private SongManager _songMngr;
        public AIController()
        {
            _db = new MusicPortalDbContext();
            _helper = new HttpResponseHelper<HeartedSongVM>(this);
            _mngr = new AIManager(_db);
            _songMngr = new SongManager(_db);
        }
        // GET api/AI/flow
        [Route("flow")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetFlow()
        {
            HttpResponseMessage responseMsg;
            try
            {
                string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
                List<HeartedSongVM> songs = _mngr.GetFlow(userId);

                responseMsg = _helper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.BadRequest);
            }

            return responseMsg;
        }
    }
}
