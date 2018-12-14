using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using MusicStoreEntity.UserAndRole;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class DetailController : Controller
    {
        private static MusicContext _context = new MusicContext();
        // GET: Detail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartDetail()
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account",new {returnUrl=Url.Action("index", "home")});
            }
            var p = Session["LoginUserSessionModel"] as LoginUserSessionModel;
            var list = _context.Orders.Where(x => x.Person.ID == p.Person.ID).ToList();
            return View(list);
        }
    }
}