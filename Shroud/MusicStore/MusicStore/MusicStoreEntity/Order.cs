using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreEntity.UserAndRole;

namespace MusicStoreEntity
{
    public class Order
    {
        public Guid ID { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Person Person { get; set; }
        public string AddressPerson { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public  decimal TotalPrice { get; set; }
        public string TradeNo { get; set; }
        public bool PaySueccess { get; set; }
        public virtual EnumOrderStatus EnumOrderStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            ID = Guid.NewGuid();
            OrderDate=DateTime.Now;
            TradeNo = OrderDate.ToString("yyyyMMddhhmmssffff");
        }
    }
}
