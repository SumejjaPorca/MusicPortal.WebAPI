using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.BL
{
    public class AIManager
    {
        
        private MusicPortalDbContext _db;

        public AIManager() {
            _db = new MusicPortalDbContext();
        }

        public List<SongVM> GetFlow(string userId) {
            //TODO: get preferred tags 
            //TODO: extract songs from those tags
            throw new NotImplementedException();
        }

        public void ReduceSubTree(string userId, int tagId) {

            int id = (from t in _db.TagUsers where t.Id == tagId && t.UserId == userId select t.ParentTagId).FirstOrDefault();
            List<TagUser> tags = (from tu in _db.TagUsers
                        where tu.UserId == userId && tu.ParentTagId == id
                        select new TagUser{
                            Id = tu.Id,
                            Popularity = tu.Popularity,
                            TagId = tu.TagId,
                            UserId = tu.UserId,
                            ParentTagId = tu.ParentTagId
                        }).ToList();
            double sum = Convert.ToDouble(tags.Sum(t => t.Popularity));
            foreach(TagUser t in tags){
                if(Convert.ToDouble(t.Popularity)/sum < 0.00001)
                    _db.TagUsers.Remove(t); //TODO: resolve cascade relationships between tag and it's parent
            }
            
        }

    }
}