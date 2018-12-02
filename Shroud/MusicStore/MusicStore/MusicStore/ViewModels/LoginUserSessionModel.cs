using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStoreEntity.UserAndRole;

namespace MusicStore.ViewModels
{
    public class LoginUserSessionModel
    {
        public ApplicationUser User { get; set; }//账号对象
        public Person Person { get; set; }//个人信息的对象
        public string RoleName { get; set; }//用户角色
       
    }
}