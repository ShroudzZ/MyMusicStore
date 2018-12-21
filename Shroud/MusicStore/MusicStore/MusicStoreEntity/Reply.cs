using MusicStoreEntity.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreEntity
{
    public class Reply
    {

        public Guid ID { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }

        public virtual Person Person { get; set; }

        public virtual Album Album { get; set; }

        public virtual Reply ParentReply { get; set; }   

        public DateTime CreateDateTime { get; set; }  

        public Reply()
        {
            ID = Guid.NewGuid();
            CreateDateTime = DateTime.Now;
        }
    }

}
