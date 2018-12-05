using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "用户名不能为空!")]//必填
        [Display(Name = "用户名")]//字段的呈现名称
        public string UserName { get; set; }

        [DataType((DataType.Password))]//前端数据类型密码
        [Required(ErrorMessage = "密码不能为空!")]
        [Display(Name = "密码")]
        [StringLength(10,ErrorMessage = "长度大于{2}小于{0}")]
        public string PassWord { get; set; }

        
        [DataType((DataType.Password))]//前端数据类型密码
        [Display(Name = "密码")]
        [Compare("PassWord",ErrorMessage = "两次密码输入的不一致")]
        public string ConfirmPassWord { get; set; }

        [Required(ErrorMessage = "姓名不能为空!")]
        [Display(Name = "姓名")]
        public string MyName { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}