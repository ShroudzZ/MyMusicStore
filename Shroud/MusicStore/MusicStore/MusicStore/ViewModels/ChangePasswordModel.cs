using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class ChangePasswordModel
    {
        [DataType((DataType.Password))]//前端数据类型密码
        [Required(ErrorMessage = "密码不能为空!")]
        [Display(Name = "密码")]
        [StringLength(10, ErrorMessage = "长度大于{2}小于{0}")]
        public string PassWord { get; set; }

        [DataType((DataType.Password))]//前端数据类型密码
        [Required(ErrorMessage = "新密码不能为空!")]
        [Display(Name = "新密码")]
        [StringLength(10, ErrorMessage = "长度大于{2}小于{0}")]
        public string NewPassWord { get; set; }


        [DataType((DataType.Password))]//前端数据类型密码
        [Display(Name = "确认新密码")]
        [Compare("NewPassWord", ErrorMessage = "两次密码输入的不一致")]
        public string ConfirmNewPassWord { get; set; }
    }
}