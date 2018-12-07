using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreEntity.UserAndRole;

namespace MusicStoreEntity
{
    public class Cart
    {
        public Guid ID { get; set; }
        public string CartID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Count { get; set; } = 1;
        public virtual Album Album { get; set; }
        public string AlbumID { get; set; }
        public virtual Person Person { get; set; }

        public Cart()
        {
            ID=Guid.NewGuid();
            CreateDateTime=DateTime.Now;
        }
    }
}
