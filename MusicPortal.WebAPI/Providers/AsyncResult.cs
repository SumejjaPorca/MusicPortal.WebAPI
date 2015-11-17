using MusicPortal.WebAPI.Binding_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MusicPortal.WebAPI.Providers
{
    public class AsyncResult:IHttpActionResult
    {
        HttpResponseMessage _msg;
        public AsyncResult(HttpResponseMessage msg)
        {
            _msg = msg;
        }
        public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(_msg);
        }
    }
}