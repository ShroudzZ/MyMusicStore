using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: Store
        public ActionResult Index()
        {
            var list = _context.Genres.ToList();
            return View(list);
        }

        public ActionResult Detail(Guid id)
        {
	    var album = _context.Albums.Find(id);

            return View(album);
           
        }
    }
}