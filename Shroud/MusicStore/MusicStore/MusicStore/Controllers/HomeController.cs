using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: test
        public ActionResult Index()
        {
            var list = _context.Albums.OrderByDescending(x=>x.PublisherDate).Take(20).ToList();
            return View(list);
        }

        public ActionResult Genre(Guid id)
        {
            var list = _context.Albums.Where(g=>g.Genre.ID==id).OrderByDescending(x => x.PublisherDate).ToList();
            return View(list);
        }
    }
}