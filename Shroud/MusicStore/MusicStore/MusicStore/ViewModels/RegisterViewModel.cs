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
        public string PassWord { get; set; }
    }
}