using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.ViewModels;
using MusicStoreEntity;
using MusicStoreEntity.UserAndRole;

namespace MusicStore.Controllers
{
    public class CartController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: Cart
        public ActionResult Index()
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel)==null)
            {
                return RedirectToAction("login", "Account");
            }
            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
           
            var list = _context.Carts.Where(x => x.Person.ID == p.ID).ToList();
            decimal? allPrice = (from item in list select item.Count * item.Album.Price).Sum();
            var cartVM = new ShoppingCartViewModel()
            {
                CartList = list,
                AllPrice = allPrice ?? decimal.Zero
            };
            return View(cartVM);
        }

        public ActionResult AddCart(Guid id)
        {
            if (Session["LoginUserSessionModel"] == null)
            {
                return Json("nologin");
            }

            var p1 = ((LoginUserSessionModel)Session["LoginUserSessionModel"]).Person;
            var cartItem = _context.Carts.SingleOrDefault(x => x.Person.ID == p1.ID && x.Album.ID == id);
            var msg = "";
            if (cartItem == null)
            {
                cartItem = new Cart()
                {
                    AlbumID = id.ToString(),
                    Album = _context.Albums.Find(id),
                    Person = _context.Persons.Find(p1.ID),
                    Count = 1,
                    CartID = (_context.Carts.Where(x => x.Person.ID == p1.ID).ToList().Count() + 1).ToString()
                };
                _context.Carts.Add(cartItem);
                _context.SaveChanges();
                msg = "添加" + _context.Albums.Find(id).Title + "到购物车成功";
            }
            else
            {
                cartItem.Count++;
                _context.SaveChanges();
                msg = _context.Albums.Find(id).Title + "已帮你添加到购物车";
            }

            return Json(msg);
        }
        [HttpPost]
        public ActionResult DelCart(Guid id)
        {
            var cart = _context.Carts.Find(id);
            var count = cart.Count;
            if (cart.Count>1)
            {
                cart.Count -= 1;
                _context.SaveChanges();
            }
            else
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
            if (_context.Carts.SingleOrDefault(x=>x.ID==id)==null||cart.Count!=count)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }
    }
}