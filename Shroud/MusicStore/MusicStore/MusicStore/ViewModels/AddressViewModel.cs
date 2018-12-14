using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStoreEntity;
using System.ComponentModel.DataAnnotations;
using MusicStoreEntity.UserAndRole;

namespace MusicStore.ViewModels
{
    public class AddressViewModel
    {
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
        public List<PersonAddress> Addresslist { get; set; }
    }
}