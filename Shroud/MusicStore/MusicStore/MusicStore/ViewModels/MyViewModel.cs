using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class MyViewModel
    {
        [Display(Name = "姓名")]
        public string Name { get; set; } // 全名
        [Display(Name = "性别")]
        public bool Sex { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; } // 人员简要编码
        [Display(Name = "固定电话")]
        [StringLength(20)]
        public string TelephoneNumber { get; set; } // 电话号码
        [Display(Name = "手机电话")]
        [StringLength(20)]
        public string MobileNumber { get; set; } // 手机号码
        [Display(Name = "邮箱")]
        [StringLength(100)]
        public string Email { get; set; } // 电子邮箱
        public string Address { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}