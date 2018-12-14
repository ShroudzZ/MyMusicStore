using MusicStoreEntity.UserAndRole;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreEntity
{
    public class PersonAddress
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

        public PersonAddress()
        {
            ID=Guid.NewGuid();
            OrderDate = DateTime.Now;
        }
    }
}
