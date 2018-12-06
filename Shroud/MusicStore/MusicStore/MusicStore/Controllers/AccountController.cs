using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicStore.ViewModels;
using MusicStoreEntity;
using MusicStoreEntity.UserAndRole;
using MusicStore.Helper;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if ((ModelState.IsValid))
            {
                var idManger = new IdentityManager();
                var p1 = new Person()
                {
                    FirstName = model.MyName.Substring(0, 1),
                    LastName = model.MyName.Substring(1, model.MyName.Length - 1),
                    Name = model.MyName,
                    CredentialsCode = "4545454545454545",
                    Birthday = DateTime.Parse("2000-1-1"),
                    MobileNumber = "1111111",
                    Email = model.Email,
                    CreateDateTime = DateTime.Now,
                    TelephoneNumber = "123456",
                    Description = "",
                    UpdateTime = DateTime.Now,
                    InquiryPassword = "",
                };
                var loginUser = new ApplicationUser()
                {
                    FirstName = p1.FirstName,
                    LastName = p1.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    MobileNumber = "1111111",
                    ChineseFullName = model.MyName,
                    Person = p1
                };
                bool isregister = idManger.CreateUser(loginUser, model.PassWord) && idManger.AddUserToRole(loginUser.Id, "User");
                if (isregister)
                {
                    return Content("<script>location.href='" + @Url.Action("index", "Home") + "'; alert('注册成功')</script>");
                }
            }
            return View();
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
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var lStatus = new LoginUserStatus()
                {
                    IsLogin = false,
                    Message = "用户或密码错误！"
                };

                var userManage = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MusicContext()));
                var user = userManage.Find(model.UserName, model.PassWord);
                if (user != null)
                {
                    if(model.VerificationCode!=Session["ValidateCode"].ToString())
                    {
                        lStatus.Message = "验证码错误";
                        ViewBag.LoginUserStatus = lStatus;
                        return View(model);
                    }
                    var roleName = "";
                    foreach (var role in user.Roles)
                    {
                        roleName += ((ApplicationRole)_context.Roles.Find(role.RoleId)).DisplayName + ",";
                    }

                    lStatus.IsLogin = true;
                    lStatus.Message = "登录成功！用户角色：" + roleName;
                    lStatus.GotoAction = "Index";
                    lStatus.GotoController = "Home";
                    Session["loginStatus"] = lStatus;

                    var lUModel = new LoginUserSessionModel()
                    {
                        User = user,
                        Person = user.Person,
                        RoleName = roleName
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
                    return View(model);
                }
            }
            if (string.IsNullOrEmpty(returnUrl))
                ViewBag.ReturnUrl = Url.Action("index", "home");
            else
                ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult LoginOut()
        {
            Session.Remove("LoginUserSessionModel");
            Session.Remove("loginStatus");
            return RedirectToAction("index", "Home");
        }

        public ActionResult ChangePassword()
        {
            if (Session["LoginUserSessionModel"]==null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var loginuser = Session["LoginUserSessionModel"] as LoginUserSessionModel;
            var userManage = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MusicContext()));
            var user = userManage.Find(loginuser.User.UserName, model.PassWord);
            if (user!=null)
            {
                userManage.ChangePassword(user.Id, model.PassWord, model.NewPassWord);
                user = userManage.Find(loginuser.User.UserName, model.NewPassWord);
                if (user!=null)
                {
                    return Content("<script>location.href='" + @Url.Action("index", "Home") + "'; alert('修改密码成功')</script>");
                }
            }
            else
            {
                ModelState.AddModelError("","原密码错误");
                return View(model);
            }
            return View(model);
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}