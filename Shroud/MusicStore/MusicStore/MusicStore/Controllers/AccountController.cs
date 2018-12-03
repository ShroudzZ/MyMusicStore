using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStore.ViewModels;
using MusicStoreEntity;
using MusicStoreEntity.UserAndRole;

namespace MusicStore.Controllers
{
    public class AccountController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET
        public ActionResult Register()
        {
            return
            View();
        }

        public ActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = Url.Action("index", "home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var lStatus=new LoginUserStatus()
                {
                    IsLogin = false,
                    Message = "用户或密码错误！"
                };
                
                var userManage = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MusicContext()));
                var user = userManage.Find(model.UserName, model.PassWord);
                if(user!=null)
                {
                    var roleName = "";
                    foreach (var role in user.Roles)
                    {
                        roleName += ((ApplicationRole) _context.Roles.Find(role.RoleId)).DisplayName + ",";
                    }

                    lStatus.IsLogin = true;
                    lStatus.Message = "登录成功！用户角色：" + roleName;
                    lStatus.GotoAction = "Index";
                    lStatus.GotoController = "Home";
                    Session["loginStatus"] = lStatus;

                    var lUModel = new LoginUserSessionModel()
                    {
                        User=user,
                        Person =user.Person,
                        RoleName =roleName
                    };

                    Session["LoginUserSessionModel"] = lUModel;

                    var identity = userManage.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    return Redirect(returnUrl);
                }
                else
                {
                    if (string.IsNullOrEmpty(returnUrl))
                        ViewBag.ReturnUrl = Url.Action("index", "home");
                    else
                        ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginUserStatus = lStatus;
                    return View();
                }
            }
            if (string.IsNullOrEmpty(returnUrl))
                ViewBag.ReturnUrl = Url.Action("index", "home");
            else
                ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}