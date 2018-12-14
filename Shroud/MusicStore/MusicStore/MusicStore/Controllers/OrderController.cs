using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Helper;
using MusicStoreEntity;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly static MusicContext _context = new MusicContext();
        public ActionResult Buy()
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
            var carts = _context.Carts.Where(x => x.Person.ID == p.ID).ToList();
            decimal? allPrice = (from item in carts select item.Count * item.Album.Price).Sum();
            var order = new Order()
            {
                AddressPerson = p.Name,
                Phone = p.MobileNumber,
                Person = _context.Persons.Find(p.ID),
                TotalPrice = allPrice ?? 0.00M
            };

            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in carts)
            {
                var detail = new OrderDetail()
                {
                    Album = _context.Albums.Find(item.Album.ID),
                    AlbumID = item.AlbumID,
                    Count = item.Count,
                    Price = item.Album.Price
                };
                order.OrderDetails.Add(detail);
            }

            Session["Order"] = order;
            return View(order);
        }

        [HttpPost]
        public ActionResult RemoveDetailT(Guid id)
        {
            if (Session["Order"] == null) return RedirectToAction("Buy");

            var order = Session["Order"] as Order;
            var deleteDetail = order.OrderDetails.SingleOrDefault(x => x.ID == id);
            order.OrderDetails.Remove(deleteDetail);

            var orderdetails = order.OrderDetails;
            order.TotalPrice = (from item in order.OrderDetails select item.Count * item.Album.Price).Sum();
            var htmlString = "";
            Session["Order"] = order;
            foreach (var item in orderdetails)
            {
                htmlString += "<tr>";
                htmlString += "<td><a href='../store/detail/" + item.ID + "'>" + item.Album.Title + "</a></td>";
                htmlString += "<td>" + item.Album.Price.ToString("C") + "</td>";
                htmlString += "<td >" + item.Count + "</td>";
                htmlString += "<td><a class=\"btn btn-danger\" href=\"javascript:;\" onclick='Del(" + item.ID + ")' \">我不喜欢了</a></td></tr>";
            }
            htmlString += "<tr><td ></td><td></td><td>总价</td><td>" + order.TotalPrice.ToString("C") + "</td ></tr>";
            return Json(htmlString);
        }


        [HttpPost]
        public ActionResult RemoveDetail(Guid id)
        {
            if (Session["LoginUserSessionModel"] == null)
                return RedirectToAction("login", "Account", new { returnUrl = Url.Action("index", "Cart") });
            var person = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;

            var cart = (_context.Carts.SingleOrDefault(x => x.Person.ID == person.ID && x.Album.ID == id));
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
                htmlString += "<td ><a href=\"javascript:; \" class=\"glyphicon glyphicon-minus\" data-id=" + item.Album.ID +
                              "></a>&nbsp;&nbsp;" + item.Count + "&nbsp;&nbsp;<a href=\"javascript:; \" class=\"glyphicon glyphicon-plus\" data-id=" +
                              item.Album.ID + "></a></td>";
                htmlString += "<td><a class=\"btn btn-danger\" href=\"#\" data-id=" + item.Album.ID + ";\">我不喜欢了</a></td></tr>";
            }
            htmlString += "<tr><td ></td><td></td><td>总价</td><td>" + totalPrice.ToString("C") + "</td ></tr>";

            return Json(htmlString);
        }

        [HttpPost]
        public ActionResult Buy(Order order)
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
                return RedirectToAction("login", "Account");
            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
            order.OrderDetails = new List<OrderDetail>();
            order.Person = _context.Persons.Find(p.ID);
            var details = ((Order)Session["Order"]).OrderDetails;
            foreach (var item in details)
            {
                item.Album = _context.Albums.Find(item.Album.ID);
                order.OrderDetails.Add(item);
            }

            order.TotalPrice = (from item in order.OrderDetails select item.Count * item.Album.Price).Sum();
            if (ModelState.IsValid)
            {
                LockedHelp.ThreadLocked(order.ID);
                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    var carts = _context.Carts.Where(x => x.Person.ID == p.ID);
                    foreach (var item in details)
                    {
                        _context.Carts.Remove(carts.SingleOrDefault(x=>x.Album.ID==item.ID));
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    LockedHelp.ThreadUnLocked(order.ID);
                }
                return RedirectToAction("Alipay", "Pay", new {id = order.ID});
            }
            return View();
        }
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
    }
}