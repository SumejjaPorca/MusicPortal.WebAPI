using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.BL;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Domain_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicPortal.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        MusicPortalDbContext _db;

        public TestController()
        {
            _db = new MusicPortalDbContext();
        }

        [AllowAnonymous]
        public string Get()
        {
            return "Luka je bio ovdje";
        }

        [AllowAnonymous]
        public List<HeartedSongVM> Get(string q, string s)
        {
            SongManager sm = new SongManager(_db);
            string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

            //"SELECT * FROM[aspnet - MusicPortal].[dbo].[Songs] ORDER BY[aspnet - MusicPortal].[dbo].[Levenshtein]([Name],  @q, 30) / CAST(LEN([Name]) AS decimal)"
            //SELECT * FROM [dbo].[Songs] ORDER BY [dbo].[Levenshtein]([Name],  @q, 30) / CAST(LEN([Name]) AS decimal)
            var query = "SELECT[Id] FROM[dbo].[Songs] ORDER BY[dbo].[Levenshtein]([Name],  @q, 30) / CAST(LEN([Name]) AS decimal) OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";
            var b = _db.Database.SqlQuery<long>(s, new SqlParameter("@q", q)).ToList();
            var c = _db.Songs.Where(so => b.Contains(so.Id));
            return  sm.MakeHeartedSong(c, userId).ToList().OrderBy(so => b.FindIndex(bo => bo == so.SongId)).ToList();
        }
    }
}
