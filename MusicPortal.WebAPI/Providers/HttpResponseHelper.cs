using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace MusicPortal.WebAPI.Providers
{
    public class HttpResponseHelper<T>
    {
        private ApiController _ctrl;
        public HttpResponseHelper(ApiController ctrl)
        {
            _ctrl = ctrl;
        }
        public HttpResponseMessage CreateCustomResponseMsg(List<T> obj, HttpStatusCode code)
        {
            IContentNegotiator negotiator = _ctrl.Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult result = negotiator.Negotiate(typeof(List<T>), _ctrl.Request,
                _ctrl.Configuration.Formatters);
            if (result == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                throw new HttpResponseException(response);
            }
            return new HttpResponseMessage()
            {
                Content = new ObjectContent<List<T>>(
                    obj,		        // What we are serializing 
                    result.Formatter,           // The media formatter
                    result.MediaType.MediaType  // The MIME type
                ),
                StatusCode = code
            };


        }

        internal HttpResponseMessage CreateErrorResponseMsg(HttpError httpError, HttpStatusCode httpStatusCode)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(httpError.Message),
                StatusCode = httpStatusCode
            };
        }
    }
}