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
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
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
            if (Session["LoginUserSessionModel"] == null)
                return RedirectToAction("login", "Account", new { returnUrl = Url.Action("index", "ShoppingCart") });
            var person = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;

            var cart = _context.Carts.Find(id);
            var count = cart.Count;
            if (cart.Count > 1) cart.Count -= 1;
            else
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
            var carts = _context.Carts.Where(x => x.Person.ID == person.ID).ToList();
            var totalPrice = (from item in carts select item.Count * item.Album.Price).Sum();
            var htmlString = "";
            foreach (var item in carts)
            {
                htmlString += "<tr>";
                htmlString += "<td><a href='../store/detail/" + item.ID + "'>" + item.Album.Title + "</a></td>";
                htmlString += "<td>" + item.Album.Price.ToString("C") + "</td>";
                htmlString += "<td>" + item.Count + "</td>";
                htmlString += "<td><a href=\"#\" data-id=" + item.ID + ";\">我不喜欢了</a></td></tr>";
            }
            htmlString += "<tr><td ></td><td></td><td>总价</td><td>" + totalPrice.ToString("C") + "</td ></tr>";

            return Json(htmlString);
        }

        [HttpPost]
        public ActionResult ConfirmCount(Guid id)
        {
            return Json("");
        }

    }
}