using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.BL;
using MusicPortal.WebAPI.Domain_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicPortal.WebAPI.Controllers
{
    [RoutePrefix("api/authors")]
    public class AuthorsController : ApiController
    {
        Data.MusicPortalDbContext _db = new Data.MusicPortalDbContext();

        //GET api/authors/54516
        [Route("{id}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetId(long id)
        {
            Author author = _db.Authors.Where(a => a.Id == id).Include(a => a.Songs).FirstOrDefault();

            if (author == null)
                return BadRequest("There is no author with this ID.");

            IQueryable<Song> songs = (from aut_son in _db.AuthorSongs
                                    where aut_son.AuthorId == id
                                    join s in _db.Songs on aut_son.SongId equals s.Id
                                    select s);
            string userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);

            AuthorFullVM model = new AuthorFullVM()
            {
                Id = author.Id,
                Name = author.Name,
                Songs = (new SongManager(_db)).MakeHeartedSong(songs, userId).ToList()
            };

            return Ok(model);
        }

    }
}
