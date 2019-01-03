using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.ViewModels;
using MusicStoreEntity;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: Store
        public ActionResult Index()
        {
            ViewBag.loginUserName = ((LoginUserSessionModel) (Session["LoginUserSessionModel"])).Person.Avarda;
            var list = _context.Genres.ToList();
            return View(list);
        }

        public ActionResult Detail(Guid id)
        {
            if(Session["LoginUserSessionModel"]!=null)
            ViewBag.loginUserName = ((LoginUserSessionModel)(Session["LoginUserSessionModel"])).Person.Avarda;
            var relylist = new List<Reply>();
            var list = _context.Replys.Where(x => x.Album.ID == id && x.ParentReply == null).ToList();
            foreach (var r in list)
            {
                relylist.Add(r);
            }
            var albumrelyVM = new AlbumRelyViewModel()
            {
                album = _context.Albums.Find(id),
                replys = relylist
            };
	       
            return View(albumrelyVM);
           
        }
    }
}