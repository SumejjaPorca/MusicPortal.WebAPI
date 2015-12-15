
using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.BL;
using MusicPortal.WebAPI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicPortal.WebAPI.Controllers {
    [RoutePrefix("api/user")]
    public class UserController : ApiController {

        private UserManager _mngr;
        private HttpResponseHelper<UserVM> _helper;
        private HttpResponseHelper<SongVM> _songHelper;

        public UserController() {
            _mngr = new UserManager();
            _helper = new HttpResponseHelper<UserVM>(this);
            _songHelper = new HttpResponseHelper<SongVM>(this);
        }

        [Route("")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetAll() {
            HttpResponseMessage responseMsg;
            try {
                List<UserVM> users = _mngr.GetAll();
                responseMsg = _helper.CreateCustomResponseMsg(users , HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        [Route("{id}")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetById(string id) {
            HttpResponseMessage responseMsg;
            try {
                UserVM user = _mngr.GetById(id);
                responseMsg = _helper.CreateCustomResponseMsg(new List<UserVM>() { user }, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

        [Route("{id}/heartedsong")]
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetHearted(string id) {
            HttpResponseMessage responseMsg;
            try {
                List<SongVM> songs = _mngr.GetUserHeartedSongs(id);
                responseMsg = _songHelper.CreateCustomResponseMsg(songs, HttpStatusCode.OK);
            } catch (Exception e) {
                responseMsg = _helper.CreateErrorResponseMsg(new HttpError(e.Message), HttpStatusCode.InternalServerError);
            }

            return responseMsg;
        }

    }
}
