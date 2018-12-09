using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreEntity.UserAndRole;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreEntity
{
    public class Order
    {
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }
        [ScaffoldColumn(false)]
        public DateTime OrderDate { get; set; }
        public virtual Person Person { get; set; }
        [Display(Name = "收货人")]
        [Required(ErrorMessage = "收件人不能为空!")]
        public string AddressPerson { get; set; }
        [Display(Name = "收货地址")]
        [Required(ErrorMessage = "收货地址不能为空!")]
        public string Address { get; set; }
        [Display(Name = "联系电话")]
        [Required(ErrorMessage = "手机不能为空!")]
        public string Phone { get; set; }
        [ScaffoldColumn(false)]
        public  decimal TotalPrice { get; set; }
        [ScaffoldColumn(false)]
        public string TradeNo { get; set; }
        [ScaffoldColumn(false)]
        public bool PaySueccess { get; set; }
        [ScaffoldColumn(false)]
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
